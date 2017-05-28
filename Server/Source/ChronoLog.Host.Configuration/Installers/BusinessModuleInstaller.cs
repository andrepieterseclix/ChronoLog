using CLog.Business.Access.Managers;
using CLog.Business.Contracts.Timesheets;
using CLog.Business.Contracts.Users;
using CLog.Business.Security.Contracts.Access;
using CLog.Business.Timesheets.Managers;
using CLog.Business.Users.Managers;
using CLog.Common.Logging;
using CLog.DataAccess;
using CLog.DataAccess.Repositories.Access;
using CLog.Framework.Configuration.Bootstrap;
using CLog.Framework.Services.Common.RequestInterception;
using CLog.Infrastructure.Contracts.Security;
using Microsoft.Practices.Unity;

namespace CLog.Host.Configuration.Installers
{
    /// <summary>
    /// Represents the business module installer.
    /// </summary>
    /// <seealso cref="CLog.Framework.Configuration.Bootstrap.IUnityDependencyInstaller" />
    public sealed class BusinessModuleInstaller : IUnityDependencyInstaller
    {
        /// <summary>
        /// Installs the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        public void Install(IUnityContainer container)
        {
            #region Special Access Manager for WCF Message Interceptor

            /* Register a named type without specifying a lifetime manager.
             * This will allow us to get a new instance every time we resolve.
             * Use this named resolver in the interceptor for controlled life-time.
             */

            container
                .RegisterType<IAccessManager>(
                HttpMessageSessionInterceptionHandler.INTERCEPTOR_ACCESS_MANAGER_INSTANCE_NAME,
                new InjectionFactory(c =>
                {
                    DataContext dc = new DataContext();

                    return new AccessManager(
                        container.Resolve<ILogger>(),
                        container.Resolve<IPasswordHelper>(),
                        container.Resolve<ILoginTokenHelper>(),
                        new UserRepository(dc),
                        new SessionRepository(dc));
                }));

            #endregion

            #region Managers for Services

            container
                .RegisterType<IAccessManager, AccessManager>(new HierarchicalLifetimeManager());

            container
                .RegisterType<ITimesheetManager, TimesheetManager>(new HierarchicalLifetimeManager());

            container
                .RegisterType<IUserManager, UserManager>(new HierarchicalLifetimeManager());

            #endregion
        }
    }
}
