using CLog.Business.Access.Managers;
using CLog.Business.Contracts.Access;
using CLog.Framework.Configuration.Bootstrap;
using Microsoft.Practices.Unity;

namespace CLog.Host.Configuration.Installers
{
    class BusinessModuleInstaller : IUnityDependencyInstaller
    {
        public void Install(IUnityContainer container)
        {
            container
                .RegisterType<IAccessManager, AccessManager>(new HierarchicalLifetimeManager());
        }
    }
}
