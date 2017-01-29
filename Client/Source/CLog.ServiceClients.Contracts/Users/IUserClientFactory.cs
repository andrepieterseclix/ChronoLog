using CLog.Framework.ServiceClients;
using CLog.Services.Contracts.Users;

namespace CLog.ServiceClients.Contracts.Users
{
    /// <summary>
    /// Represents the User client factory contract.
    /// </summary>
    public interface IUserClientFactory
    {
        /// <summary>
        /// Creates a service client instance.
        /// </summary>
        /// <returns>The service client for the User service.</returns>
        IServiceClient<IUserService> Create();
    }
}
