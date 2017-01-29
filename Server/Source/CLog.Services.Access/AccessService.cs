using CLog.Business.Security.Contracts.Access;
using CLog.Common.Logging;
using CLog.Framework.Business.Models.Results;
using CLog.Framework.Services.Extensions;
using CLog.Models.Access;
using CLog.Services.Access.Extensions;
using CLog.Services.Common;
using CLog.Services.Models.Access.DataTransfer;
using CLog.Services.Security.Contracts.Access;
using System;
using System.Security.Permissions;

namespace CLog.Services.Access
{
    /// <summary>
    /// Represents the Access micro-service implementation.
    /// </summary>
    /// <seealso cref="CLog.Framework.Services.ServiceBase" />
    /// <seealso cref="CLog.Services.Access.Contracts.IAccessService" />
    public class AccessService : ServiceBase, IAccessService
    {
        #region Fields

        private readonly IAccessManager _accessManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="accessManager">The access manager.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public AccessService(ILogger logger, IAccessManager accessManager)
            : base(logger)
        {
            if (accessManager == null)
                throw new ArgumentNullException(nameof(accessManager));

            _accessManager = accessManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs the login request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The login response.
        /// </returns>
        public LoginResponse Login(LoginRequest request)
        {
            return Execute<LoginResponse>(response =>
            {
                BusinessResult<Session> result = _accessManager.Login(request.UserName, request.Password);

                // Map Errors
                response.AddMessages(result);

                // Map Response
                response.IsLoggedIn =
                    !result.HasErrors &&
                    result.HasResult &&
                    result.Result.User != null;

                if (response.IsLoggedIn)
                {
                    response.Session = result.Result.Map();
                    response.User = result.Result.User.Map();
                }
            });
        }

        /// <summary>
        /// Performs the Logout request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The logout response.
        /// </returns>
        [PrincipalPermission(SecurityAction.Demand)]
        public LogoutResponse Logout(LogoutRequest request)
        {
            return Execute<LogoutResponse>(response =>
            {
                BusinessResult result = _accessManager.Logout(request.UserName);

                // Map Errors
                response.AddMessages(result);
            });
        }

        /// <summary>
        /// Updates the user password.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>
        /// The update user password response.
        /// </returns>
        [PrincipalPermission(SecurityAction.Demand)]
        public UpdateUserPasswordResponse UpdateUserPassword(UpdateUserPasswordRequest request)
        {
            return Execute<UpdateUserPasswordResponse>(response =>
            {
                BusinessResult result = _accessManager.UpdatePassword(
                    request.UserPassword?.UserName,
                    request.UserPassword?.OldPassword,
                    request.UserPassword.NewPassword);

                // Map Errors
                response.AddMessages(result);
            });
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
                    if (_accessManager != null)
                        _accessManager.Dispose();
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
