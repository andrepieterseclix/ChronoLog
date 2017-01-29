using CLog.DataAccess;
using CLog.DataAccess.Contracts.Repositories.Access;
using CLog.DataAccess.Contracts.Repositories.Timesheets;
using CLog.DataAccess.Repositories.Access;
using CLog.DataAccess.Repositories.Timesheets;
using CLog.Framework.Configuration.Bootstrap;
using Microsoft.Practices.Unity;

namespace CLog.Host.Configuration.Installers
{
    /// <summary>
    /// Represents the data access module installer.
    /// </summary>
    /// <seealso cref="CLog.Framework.Configuration.Bootstrap.IUnityDependencyInstaller" />
    public sealed class DataAccessModuleInstaller : IUnityDependencyInstaller
    {
        /// <summary>
        /// Installs the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        public void Install(IUnityContainer container)
        {
            // Data Context
            container
                .RegisterType<DataContext>(new HierarchicalLifetimeManager());

            // Repositories
            container
                .RegisterType<ISessionRepository, SessionRepository>(new HierarchicalLifetimeManager());

            container
                .RegisterType<ICapturedTimeRepository, CapturedTimeRepository>(new HierarchicalLifetimeManager());

            container
                .RegisterType<IUserRepository, UserRepository>(new HierarchicalLifetimeManager());
        }
    }
}
