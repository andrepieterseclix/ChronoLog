using CLog.Common.Logging;
using CLog.Framework.ServiceClients;
using CLog.ServiceClients.Contracts.Access;
using CLog.ServiceClients.Security;
using CLog.Services.Models.Access.DataTransfer;
using CLog.Services.Security.Contracts.Access;
using CLog.UI.Common.Business;
using CLog.UI.Common.Helpers;
using CLog.UI.Models.Access;
using System;
using System.Globalization;
using System.Threading;

namespace CLog.UI.Main.Managers
{
    /// <summary>
    /// Represents the Access UI Business Manager.
    /// </summary>
    /// <seealso cref="CLog.UI.Common.Business.UIBusinessManager" />
    /// <seealso cref="CLog.UI.Main.Managers.IAccessManager" />
    public sealed class AccessManager : UIBusinessManager, IAccessManager
    {
        #region Fields

        private readonly IAccessClientFactory _accessClientFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessManager"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="accessClientFactory">The access client factory.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public AccessManager(ILogger logger, IAccessClientFactory accessClientFactory)
            : base(logger)
        {
            if (accessClientFactory == null)
                throw new ArgumentNullException(nameof(accessClientFactory));

            _accessClientFactory = accessClientFactory;
        }

        public BusinessResult<LoginResult> Login(string userName, string password)
        {
            return Execute<LoginResult>(result =>
            {
                LoginRequest request = new LoginRequest(userName, password);
                LoginResponse response = null;

                using (IServiceClient<IAccessService> client = _accessClientFactory.Create())
                {
                    response = client.Proxy.Login(request);
                }

                result.Result = new LoginResult(response.IsLoggedIn);

                if (response.IsLoggedIn)
                {
                    User user = new User(response.User.UserName, response.User.Name, response.User.Surname, response.User.Email);
                    result.Result.User = user;

                    // Ensure the principal is read from the UI thread
                    DispatcherHelper.Invoke(() =>
                    {
                        ClientPrincipal principal = Thread.CurrentPrincipal as ClientPrincipal;
                        if (principal == null)
                            throw new ApplicationException("The application's thread principal has not been configured correctly.");

                        principal.Identity = new ClientIdentity(
                            user.UserName,
                            string.Format(CultureInfo.CurrentCulture, "{0} {1}", user.Name, user.Surname),
                            response.Session.Id,
                            response.Session.SessionKey,
                            new string[0]);
                    });
                }

                // Map Errors
                result.AddMessages(response);
            });
        }

        public BusinessResult Logout(ClientPrincipal principal)
        {
            return Execute(result =>
            {
                LogoutResponse response = null;

                using (IServiceClient<IAccessService> client = _accessClientFactory.Create())
                {
                    LogoutRequest request = new LogoutRequest(
                        principal.Identity.UserName,
                        principal.Identity.SessionId,
                        principal.Identity.SessionKey);

                    response = client.Proxy.Logout(request);

                    // Map Errors
                    result.AddMessages(response);
                }
            });
        }

        #endregion

        #region Methods
        
        #endregion
    }
}
