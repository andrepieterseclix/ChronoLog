using CLog.Framework.ServiceClients;
using CLog.Services.Security.Contracts.Access;

namespace CLog.ServiceClients.Contracts.Access
{
    /// <summary>
    /// Represents the Access client factory contract.
    /// </summary>
    public interface IAccessClientFactory
    {
        /// <summary>
        /// Creates a service client instance.
        /// </summary>
        /// <returns>The service client for the Access service.</returns>
        IServiceClient<IAccessService> Create();
    }
}
