using CLog.DataAccess;
using CLog.DataAccess.Contracts.Repositories.Access;
using CLog.DataAccess.Repositories.Access;
using CLog.Framework.Configuration.Bootstrap;
using Microsoft.Practices.Unity;

namespace CLog.Host.Configuration.Installers
{
    class DataAccessModuleInstaller : IUnityDependencyInstaller
    {
        public void Install(IUnityContainer container)
        {
            // TODO:  Investigate how the DataContext will be instantiated and shared, and what is its life time?
            //container
            //    .RegisterType<DataContext>(new PerThreadLifetimeManager());
            container
                .RegisterType<DataContext>(new HierarchicalLifetimeManager());

            // Repositories
            container
                .RegisterType<IUserRepository, UserRepository>(new HierarchicalLifetimeManager());

            container
                .RegisterType<ISessionRepository, SessionRepository>(new HierarchicalLifetimeManager());
        }
    }
}
