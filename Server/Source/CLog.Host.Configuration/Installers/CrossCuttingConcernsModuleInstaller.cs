using CLog.Common.Log4NetLogger;
using CLog.Common.Logging;
using CLog.Framework.Configuration.Bootstrap;
using Microsoft.Practices.Unity;

namespace CLog.Host.Configuration.Installers
{
    class CrossCuttingConcernsModuleInstaller : IUnityDependencyInstaller
    {
        public void Install(IUnityContainer container)
        {
            // Singleton
            container
                .RegisterType<ILogger, Log4NetLogger>(new ContainerControlledLifetimeManager());
        }
    }
}
