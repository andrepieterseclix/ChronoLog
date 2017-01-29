using CLog.ServiceClients.Contracts.Users;
using CLog.Services.Contracts.Users;

namespace CLog.ServiceClients.Clients.Users
{
    /// <summary>
    /// Represents the service client factory for the User service.
    /// </summary>
    /// <seealso cref="CLog.ServiceClients.ServiceClientFactory{CLog.Services.Contracts.Users.IUserService}" />
    /// <seealso cref="CLog.ServiceClients.Contracts.Users.IUserClientFactory" />
    public sealed class UserClientFactory : ServiceClientFactory<IUserService>, IUserClientFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserClientFactory"/> class.
        /// </summary>
        public UserClientFactory()
            : base("BasicHttpUserService")
        {
        }
    }
}
