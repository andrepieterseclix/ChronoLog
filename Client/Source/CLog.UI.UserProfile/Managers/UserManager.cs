using System;
using CLog.Common.Logging;
using CLog.UI.Common.Business;
using CLog.UI.Models.Access;
using CLog.ServiceClients.Contracts.Users;
using CLog.Services.Contracts.Users;
using CLog.Framework.ServiceClients;
using CLog.ServiceClients.Security;
using CLog.Services.Models.Users;
using CLog.UI.UserProfile.Extensions;
using CLog.Services.Models.Users.DataTransfer;

namespace CLog.UI.UserProfile.Managers
{
    /// <summary>
    /// Represents the UI User Manager.
    /// </summary>
    /// <seealso cref="CLog.UI.UserProfile.Managers.IUserManager" />
    public sealed class UserManager : UIBusinessManager, IUserManager
    {
        #region Fields

        private readonly IUserClientFactory _userServiceClient;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserManager"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="userServiceClient">The user service client.</param>
        public UserManager(ILogger logger, IUserClientFactory userServiceClient)
            : base(logger)
        {
            if (userServiceClient == null)
                throw new ArgumentNullException(nameof(userServiceClient));

            _userServiceClient = userServiceClient;
        }

        #endregion

        #region Methods

        public UIBusinessResult<SessionInfo> UpdateUser(User user)
        {
            return Execute<SessionInfo>(result =>
            {
                UserDetailsDto userDetails = user.Map();
                UpdateUserRequest request = new UpdateUserRequest(userDetails);
                UpdateUserResponse response = null;

                using (IServiceClient<IUserService> client = _userServiceClient.Create())
                {
                    response = client.Proxy.UpdateUser(request);
                }

                // Map Errors
                result.AddMessages(response);

                // Map Result
                result.Result = response.Session.Map();
            });
        }

        #endregion
    }
}
