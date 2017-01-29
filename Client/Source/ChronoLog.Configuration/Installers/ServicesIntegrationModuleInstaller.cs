using CLog.Framework.Configuration.Bootstrap;
using CLog.ServiceClients.Clients.Access;
using CLog.ServiceClients.Clients.Timesheets;
using CLog.ServiceClients.Clients.Users;
using CLog.ServiceClients.Contracts.Access;
using CLog.ServiceClients.Contracts.Timesheets;
using CLog.ServiceClients.Contracts.Users;
using Microsoft.Practices.Unity;

namespace ChronoLog.Configuration.Installers
{
    /// <summary>
    /// Represents the service integration module installer.
    /// </summary>
    /// <seealso cref="CLog.Framework.Configuration.Bootstrap.IUnityDependencyInstaller" />
    class ServicesIntegrationModuleInstaller : IUnityDependencyInstaller
    {
        /// <summary>
        /// Installs the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        public void Install(IUnityContainer container)
        {
            container
                .RegisterType<IAccessClientFactory, AccessClientFactory>(new HierarchicalLifetimeManager());

            container
                .RegisterType<ITimesheetClientFactory, TimesheetClientFactory>(new HierarchicalLifetimeManager());

            container
                .RegisterType<IUserClientFactory, UserClientFactory>(new HierarchicalLifetimeManager());
        }
    }
}
