using CLog.Common.Log4NetLogger;
using CLog.Common.Logging;
using CLog.Framework.Configuration.Bootstrap;
using Microsoft.Practices.Unity;

namespace CLog.Host.Configuration.Installers
{
    /// <summary>
    /// Represents the cross cutting concerns module installer.
    /// </summary>
    /// <seealso cref="CLog.Framework.Configuration.Bootstrap.IUnityDependencyInstaller" />
    public sealed class CrossCuttingConcernsModuleInstaller : IUnityDependencyInstaller
    {
        /// <summary>
        /// Installs the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        public void Install(IUnityContainer container)
        {
            // Singleton
            container
                .RegisterType<ILogger, Log4NetLogger>(new ContainerControlledLifetimeManager()); // Singleton
        }
    }
}
