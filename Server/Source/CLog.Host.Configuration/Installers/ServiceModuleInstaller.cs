using CLog.Framework.Configuration.Bootstrap;
using CLog.Services.Access;
using CLog.Services.Contracts.Access;
using Microsoft.Practices.Unity;

namespace CLog.Host.Configuration.Installers
{
    class ServiceModuleInstaller : IUnityDependencyInstaller
    {
        public void Install(IUnityContainer container)
        {
            container
                .RegisterType<IAccessService, AccessService>(new HierarchicalLifetimeManager());
        }
    }
}
