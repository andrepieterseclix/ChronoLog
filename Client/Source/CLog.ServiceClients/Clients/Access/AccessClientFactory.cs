using CLog.ServiceClients.Contracts.Access;
using CLog.Services.Security.Contracts.Access;

namespace CLog.ServiceClients.Clients.Access
{
    /// <summary>
    /// Represents the service client factory for the Access service.
    /// </summary>
    /// <seealso cref="CLog.ServiceClients.ServiceClientFactory{CLog.Services.Security.Contracts.Access.IAccessService}" />
    /// <seealso cref="CLog.ServiceClients.Contracts.Access.IAccessClientFactory" />
    public sealed class AccessClientFactory : ServiceClientFactory<IAccessService>, IAccessClientFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccessClientFactory"/> class.
        /// </summary>
        public AccessClientFactory()
            : base("BasicHttpAccessService")
        {
        }
    }
}
