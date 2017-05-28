using ChronoLog.Host.WebApi.Models.Access;
using CLog.Common.Logging;
using CLog.Framework.Services.WebApi.Controllers;
using CLog.Services.Contracts.Users;
using CLog.Services.Models.Access;
using CLog.Services.Models.Access.DataTransfer;
using CLog.Services.Models.Users;
using CLog.Services.Models.Users.DataTransfer;
using CLog.Services.Security.Contracts.Access;
using System;
using System.Web.Http;
using System.Web.Http.Description;

namespace ChronoLog.Host.WebApi.Controllers
{
    /// <summary>
    /// Represents the Access REST service.
    /// </summary>
    /// <seealso cref="CLog.Framework.Services.WebApi.Controllers.ApiControllerBase" />
    public class AccessController : ApiControllerBase
    {
        #region Fields

        private IAccessService _accessService;

        private IUserService _userService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiControllerBase" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="accessService">The service.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public AccessController(ILogger logger, IAccessService accessService, IUserService userService)
            : base(logger)
        {
            if (accessService == null)
                throw new ArgumentNullException(nameof(accessService));
            if (userService == null)
                throw new ArgumentNullException(nameof(userService));

            _accessService = accessService;
            _userService = userService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs the login request.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns>The <see cref="LoginResponse"/> based response.</returns>
        [HttpGet]
        [Route("Access/Login")]
        [ResponseType(typeof(LoginResponse))]
        public IHttpActionResult Login(string userName, string password)
        {
            return ExecuteGet(() =>
            {
                LoginRequest request = new LoginRequest(userName, password);

                return _accessService.Login(request);
            });
        }

        /// <summary>
        /// Performs the Logout request.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="sessionKey">The session key.</param>
        /// <returns>The <see cref="LogoutResponse"/> based response.</returns>
        [HttpPost]
        [Route("Access/Logout")]
        [ResponseType(typeof(LogoutResponse))]
        public IHttpActionResult Logout(string userName, Guid? sessionId, string sessionKey)
        {
            return ExecuteGet(() =>
            {
                LogoutRequest request = new LogoutRequest(userName, sessionId.Value, sessionKey);

                return _accessService.Logout(request);
            });
        }

        /// <summary>
        /// Updates the user password.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="oldPassword">The old password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns>
        /// The <see cref="UpdateUserPasswordResponse" /> based response.
        /// </returns>
        [HttpPut]
        [Route("Access/Users/{userName}/Password/{newPassword}")]
        [ResponseType(typeof(UpdateUserPasswordResponse))]
        public IHttpActionResult UpdateUserPassword(string userName, string oldPassword, string newPassword)
        {
            return ExecuteUpdate(() =>
            {
                UserPasswordDto requestDto = new UserPasswordDto(userName, oldPassword, newPassword);
                UpdateUserPasswordRequest request = new UpdateUserPasswordRequest(requestDto);

                return _accessService.UpdateUserPassword(request);
            });
        }

        /// <summary>
        /// Updates the user details.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="model">The model.</param>
        /// <returns>The <see cref="UpdateUserResponse"/> based response.</returns>
        [HttpPut]
        [Route("Access/Users/{userName}")]
        [ResponseType(typeof(UpdateUserResponse))]
        public IHttpActionResult UpdateUser(string userName, [FromBody]UserDetailsModel model)
        {
            return ExecuteUpdate(() =>
            {
                UserDetailsDto requestDto = new UserDetailsDto(userName, model.Name, model.Surname, model.Email);
                UpdateUserRequest request = new UpdateUserRequest(requestDto);

                return _userService.UpdateUser(request);
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
                    _accessService?.Dispose();
                    _userService?.Dispose();
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
