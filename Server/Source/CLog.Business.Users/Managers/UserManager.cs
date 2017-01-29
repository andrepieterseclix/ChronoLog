using CLog.Business.Contracts.Users;
using CLog.Business.Users.Messages;
using CLog.Common.Logging;
using CLog.DataAccess.Contracts.Repositories.Access;
using CLog.Framework.Business.Managers;
using CLog.Framework.Business.Models.Results;
using CLog.Infrastructure.Contracts.Security;
using CLog.Models;
using CLog.Models.Access;
using CLog.Models.Users;
using System;
using System.Linq;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace CLog.Business.Users.Managers
{
    /// <summary>
    /// Represents the Users business manager.
    /// </summary>
    /// <seealso cref="CLog.Framework.Business.Managers.BusinessManager" />
    /// <seealso cref="CLog.Business.Contracts.Users.IUserManager" />
    public sealed class UserManager : BusinessManager, IUserManager
    {
        #region Fields

        private readonly IPasswordHelper _passwordHelper;

        private readonly ILoginTokenHelper _loginTokenHelper;

        private readonly IUserRepository _userRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserManager"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="passwordHelper">The password helper.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public UserManager(ILogger logger, IPasswordHelper passwordHelper, ILoginTokenHelper loginTokenHelper, IUserRepository userRepository)
            : base(logger)
        {
            if (passwordHelper == null)
                throw new ArgumentNullException(nameof(passwordHelper));
            if (loginTokenHelper == null)
                throw new ArgumentNullException(nameof(loginTokenHelper));
            if (userRepository == null)
                throw new ArgumentNullException(nameof(userRepository));

            _passwordHelper = passwordHelper;
            _loginTokenHelper = loginTokenHelper;
            _userRepository = userRepository;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="userDetails">The user.</param>
        /// <param name="userPassword">The user password.</param>
        /// <returns>
        /// The business result.
        /// </returns>
        [PrincipalPermission(SecurityAction.Demand)]
        public BusinessResult<Session> UpdateUser(UserDetails userDetails)
        {
            BusinessResult<Session> result = new BusinessResult<Session>();

            Execute(() =>
            {
                // Validate request
                if (userDetails == null)
                {
                    result.Errors.Add(ErrorMessages.InvalidRequestArguments);
                    return;
                }

                string userName = null;
                User user = null;

                // Update user details
                if (userDetails != null)
                {
                    if (!ValidateUserName((userName = userDetails.UserName), result))
                        return;

                    if (!GetUser(out user, userName, result))
                        return;

                    // Validate
                    ValidateText(userDetails.Email, ModelConstants.REGEX_EMAIL, result, ErrorMessages.InvalidEmailAddress);
                    ValidateText(userDetails.Name, ModelConstants.NAME_REGEX, result, ErrorMessages.InvalidNameOrSurname);
                    ValidateText(userDetails.Surname, ModelConstants.NAME_REGEX, result, ErrorMessages.InvalidNameOrSurname);

                    if (result.HasErrors)
                        return;

                    user.Email = userDetails.Email;
                    user.Name = userDetails.Name;
                    user.Surname = userDetails.Surname;
                }
                
                // Update Session
                string regeneratedKey = _loginTokenHelper.GenerateSecurityToken(user);

                result.Result = user.Sessions
                    .Where(s => s.IsActive)
                    .OrderByDescending(s => s.LoginTimeUtc)
                    .FirstOrDefault();

                if (result.Result != null)
                    result.Result.SessionKey = regeneratedKey;

                // Update User
                _userRepository.Update(user);
            });

            return result;
        }

        #endregion

        #region Helper Methods

        private bool ValidateUserName(string userName, BusinessResult result)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                result.Errors.Add(ErrorMessages.UserNameNotSpecified);
                return false;
            }

            return true;
        }

        private bool GetUser(out User user, string userName, BusinessResult result)
        {
            user = _userRepository.Get(u => u.UserName == userName);

            if (user == null)
            {
                result.Errors.Add(ErrorMessages.UserNotFound);
                return false;
            }

            return true;
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
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
