using CLog.Common.Logging;
using CLog.Framework.Services.WebApi.Controllers;
using CLog.Services.Models.Access.DataTransfer;
using CLog.Services.Security.Contracts.Access;
using System;
using System.Web.Http;

namespace ChronoLog.Host.WebApi.Controllers
{
    /// <summary>
    /// Represents the Access REST service.
    /// </summary>
    /// <seealso cref="CLog.Framework.Services.WebApi.Controllers.ApiControllerBase" />
    public class AccessController : ApiControllerBase
    {
        #region Fields

        private IAccessService _service;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiControllerBase" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="service">The service.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public AccessController(ILogger logger, IAccessService service)
            : base(logger)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            _service = service;
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
        public IHttpActionResult Login(string userName, string password)
        {
            return Execute(() =>
            {
                LoginRequest request = new LoginRequest(userName, password);
                LoginResponse response = _service.Login(request);

                return Ok(response);
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
        public IHttpActionResult Logout(string userName, Guid? sessionId, string sessionKey)
        {
            return Execute(() =>
            {
                LogoutRequest request = new LogoutRequest(userName, sessionId.Value, sessionKey);
                LogoutResponse response = _service.Logout(request);

                return Ok(response);
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
                    if (_service != null)
                        _service.Dispose();
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
