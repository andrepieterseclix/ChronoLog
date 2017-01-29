using CLog.Business.Access.Messages;
using CLog.Business.Security.Contracts.Access;
using CLog.Common.Logging;
using CLog.DataAccess.Contracts.Repositories.Access;
using CLog.Framework.Business.Managers;
using CLog.Framework.Business.Models.Results;
using CLog.Framework.Models;
using CLog.Infrastructure.Contracts.Security;
using CLog.Models;
using CLog.Models.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Permissions;

namespace CLog.Business.Access.Managers
{
    /// <summary>
    /// Represents the Access business manager.
    /// </summary>
    /// <seealso cref="CLog.Framework.Business.Managers.BusinessManager" />
    /// <seealso cref="CLog.Business.Access.Contracts.IAccessManager" />
    public sealed class AccessManager : BusinessManager, IAccessManager
    {
        #region Fields

        private readonly IPasswordHelper _passwordHelper;

        private readonly ILoginTokenHelper _loginTokenHelper;

        private readonly IUserRepository _userRepository;

        private readonly ISessionRepository _sessionRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessManager" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="passwordHelper">The password helper.</param>
        /// <param name="loginTokenHelper">The login token helper.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="sessionRepository">The session repository.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public AccessManager(ILogger logger, IPasswordHelper passwordHelper, ILoginTokenHelper loginTokenHelper, IUserRepository userRepository, ISessionRepository sessionRepository)
            : base(logger)
        {
            if (passwordHelper == null)
                throw new ArgumentNullException(nameof(passwordHelper));
            if (loginTokenHelper == null)
                throw new ArgumentNullException(nameof(loginTokenHelper));
            if (userRepository == null)
                throw new ArgumentNullException(nameof(userRepository));
            if (sessionRepository == null)
                throw new ArgumentNullException(nameof(sessionRepository));

            _passwordHelper = passwordHelper;
            _loginTokenHelper = loginTokenHelper;
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Logs in the specified user if the correct password was specified.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// The business result with a <see cref="T:CLog.Models.Access.Session" /> if the login was successful.
        /// </returns>
        public BusinessResult<Session> Login(string userName, string password)
        {
            BusinessResult<Session> result = new BusinessResult<Session>();

            Execute(() =>
            {
                User user;

                if (!FindUser(userName, result, out user))
                    return;

                if (!CheckPassword(user, password, result, ErrorMessages.IncorrectPassword))
                    return;

                if (user.State == DataState.AwaitingApproval)
                    result.Errors.Add(ErrorMessages.UserNotApproved);
                else if (user.State == DataState.Suspended)
                    result.Errors.Add(ErrorMessages.UserSuspended);
                else if (user.State != DataState.Active && user.State != DataState.Approved)
                    result.Errors.Add(ErrorMessages.UserInconsistentState);

                // Log
                foreach (ErrorMessage error in result.Errors)
                    LoggerHelper.Warning(Logger, error.ToString());

                if (result.HasErrors)
                    return;

                // Make sure the user is logged out of previous sessions
                LogOut(user);

                // Create new session
                Session session = Session.New(Guid.NewGuid().ToString(), user);
                session.SessionKey = _loginTokenHelper.GenerateSecurityToken(user);
                _sessionRepository.Add(session);
                LoggerHelper.Info(Logger, "Session '{0}' created for user '{1}'", session.RefId, user.UserName);

                result.Result = session;
            });

            return result;
        }

        /// <summary>
        /// Logout the user associated with the session specified.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>
        /// The business result.
        /// </returns>
        [PrincipalPermission(SecurityAction.Demand)]
        public BusinessResult Logout(string userName)
        {
            BusinessResult result = new BusinessResult();

            Execute(() =>
            {
                User user;

                if (!FindUser(userName, result, out user))
                    return;

                LogOut(user);
            });

            return result;
        }

        /// <summary>
        /// Validates the session.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="sessionKey">The session key.</param>
        /// <returns>
        /// The business result.
        /// </returns>
        public BusinessResult<SessionState> ValidateSession(string userName, Guid sessionId, string sessionKey)
        {
            BusinessResult<SessionState> result = new BusinessResult<SessionState>(new SessionState());

            Execute(() =>
            {
                User user;

                if (!FindUser(userName, result, out user))
                    return;

                result.Result.UserId = user.Id;
                Session session = user.Sessions.FirstOrDefault(s => s.RefId == sessionId);

                if (session == null)
                {
                    result.Errors.Add(ErrorMessages.SessionNotFound);
                    LoggerHelper.Warning(Logger, ErrorMessages.SessionNotFound.ToString());

                    return;
                }

                bool sessionExpired = false;

                if (session.SessionKey != sessionKey || !_loginTokenHelper.VerifySecurityToken(session, out sessionExpired))
                {
                    result.Result.IsExpired = sessionExpired;
                    result.Errors.Add(ErrorMessages.InvalidSession);
                    LoggerHelper.Warning(Logger, ErrorMessages.InvalidSession.ToString());
                }

                session.LastActiveUtc = DateTime.UtcNow;
                session.IsActive = !result.HasErrors;
                _sessionRepository.Update(session);

                LoggerHelper.Info(Logger, "Session '{0}' is {1} at {2} (UTC).", session.RefId, (session.IsActive ? "still active" : "inactivated"), session.LastActiveUtc);
            });

            return result;
        }

        /// <summary>
        /// Updates the password.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="oldPassword">The old password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns></returns>
        [PrincipalPermission(SecurityAction.Demand)]
        public BusinessResult UpdatePassword(string userName, string oldPassword, string newPassword)
        {
            BusinessResult result = new BusinessResult();

            Execute(() =>
            {
                User user;

                if (!FindUser(userName, result, out user))
                    return;

                if (!CheckPassword(user, oldPassword, result, ErrorMessages.IncorrectOldPassword))
                    return;

                if (!ValidateText(newPassword, ModelConstants.REGEX_PASSWORD, result, ErrorMessages.InvalidPassword))
                    return;

                user.Salt = _passwordHelper.GetRandomSalt();
                user.Password = _passwordHelper.ComputeHash(newPassword, user.Salt);

                _userRepository.Update(user);

                LoggerHelper.Info(Logger, "Password updated for user '{0}'.", userName);
            });

            return result;
        }

        #endregion

        #region Helper Methods

        private bool FindUser(string userName, BusinessResult result, out User user, [CallerMemberName]string caller = null)
        {
            user = _userRepository.Get(x => x.UserName == userName);

            if (user == null)
            {
                result.Errors.Add(ErrorMessages.UserNotFound);
                LoggerHelper.Warning(Logger, "{0}:  {1}", caller, ErrorMessages.UserNotFound.ToString());
            }

            return (user != null);
        }

        private bool CheckPassword(User user, string password, BusinessResult result, ErrorMessage errorIfInvalid, [CallerMemberName]string caller = null)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            string hash = _passwordHelper.ComputeHash(password, user.Salt);
            bool passwordValid = (hash == user.Password);

            if (!passwordValid)
            {
                result.Errors.Add(errorIfInvalid);
                LoggerHelper.Warning(Logger, "{0}:  {1}", caller, errorIfInvalid.ToString());
            }

            LoggerHelper.Info(Logger, "Password validated for user '{0}'.", user.UserName);

            return passwordValid;
        }

        private void LogOut(User user, [CallerMemberName]string caller = null)
        {
            Session[] sessions = user.Sessions
                .Where(s => s.IsActive)
                .OrderByDescending(x => x.LoginTimeUtc)
                .ToArray();

            for (int i = 0; i < sessions.Length; i++)
            {
                sessions[i].IsActive = false;
                sessions[i].LastActiveUtc = DateTime.UtcNow;
                _sessionRepository.Update(sessions[i]);

                if (i > 0)
                    LoggerHelper.Warning(Logger, "User '{0}' had multiple active sessions, logging out Session '{1}'.", user.UserName, sessions[i].RefId);
                else
                    LoggerHelper.Info(Logger, "User '{0}' is logging out of Session '{1}'.", user.UserName, sessions[i].RefId);
            }
        }

        #endregion

        #region IDisposable Implementation

        /// <summary>
        /// Keep this private, and create and maintain one for every derived class.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_userRepository != null)
                        _userRepository.Dispose();

                    if (_sessionRepository != null)
                        _sessionRepository.Dispose();
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
