using CLog.Business.Contracts.Users;
using CLog.Common.Logging;
using CLog.Framework.Business.Models.Results;
using CLog.Framework.Services.Extensions;
using CLog.Models.Access;
using CLog.Services.Common;
using CLog.Services.Contracts.Users;
using CLog.Services.Models.Users.DataTransfer;
using CLog.Services.Users.Extensions;
using System;
using System.Security.Permissions;

namespace CLog.Services.Users
{
    /// <summary>
    /// Represents the user service implementation.
    /// </summary>
    /// <seealso cref="CLog.Framework.Services.ServiceBase" />
    /// <seealso cref="CLog.Services.Contracts.Users.IUserService" />
    public class UserService : ServiceBase, IUserService
    {
        #region Fields

        private readonly IUserManager _userManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="accessManager">The access manager.</param>
        /// <param name="userManager">The user manager.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public UserService(ILogger logger, IUserManager userManager)
            : base(logger)
        {
            if (userManager == null)
                throw new ArgumentNullException(nameof(userManager));

            _userManager = userManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The update user response.
        /// </returns>
        [PrincipalPermission(SecurityAction.Demand)]
        public UpdateUserResponse UpdateUser(UpdateUserRequest request)
        {
            return Execute<UpdateUserResponse>(response =>
            {
                BusinessResult<Session> result = _userManager.UpdateUser(request.UserDetails.Map());

                // Map Errors
                response.AddMessages(result);

                // Map Response
                response.Session = result.Result.Map();
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
                    if (_userManager != null)
                        _userManager.Dispose();
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
