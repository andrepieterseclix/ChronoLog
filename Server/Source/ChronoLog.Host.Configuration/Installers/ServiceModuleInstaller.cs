using CLog.Framework.Configuration.Bootstrap;
using CLog.Services.Access;
using CLog.Services.Contracts.Timesheets;
using CLog.Services.Contracts.Users;
using CLog.Services.Security.Contracts.Access;
using CLog.Services.Timesheets;
using CLog.Services.Users;
using Microsoft.Practices.Unity;

namespace CLog.Host.Configuration.Installers
{
    /// <summary>
    /// Represents the service module installer.
    /// </summary>
    /// <seealso cref="CLog.Framework.Configuration.Bootstrap.IUnityDependencyInstaller" />
    public sealed class ServiceModuleInstaller : IUnityDependencyInstaller
    {
        /// <summary>
        /// Installs the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        public void Install(IUnityContainer container)
        {
            container
                .RegisterType<IAccessService, AccessService>();

            container
                .RegisterType<ITimesheetService, TimesheetService>();

            container
                .RegisterType<IUserService, UserService>();
        }
    }
}
