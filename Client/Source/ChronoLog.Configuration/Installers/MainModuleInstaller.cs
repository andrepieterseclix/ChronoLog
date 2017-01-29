using CLog.Framework.Configuration.Bootstrap;
using CLog.UI.Common.Services;
using CLog.UI.Main.Controllers;
using CLog.UI.Main.Services;
using Microsoft.Practices.Unity;

namespace ChronoLog.Configuration.Installers
{
    /// <summary>
    /// Represents the main UI module installer.
    /// </summary>
    /// <seealso cref="CLog.Framework.Configuration.Bootstrap.IUnityDependencyInstaller" />
    public sealed class MainModuleInstaller : IUnityDependencyInstaller
    {
        /// <summary>
        /// Installs the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        public void Install(IUnityContainer container)
        {
            // Controllers
            container
                .RegisterType<ILoginController, LoginController>(new HierarchicalLifetimeManager());

            // Services
            container
                .RegisterType<IDialogService, DialogService>(new HierarchicalLifetimeManager());

            container
                .RegisterType<IMouseService, MouseService>(new HierarchicalLifetimeManager());

            container
                .RegisterType<IStatusService, StatusService>(new HierarchicalLifetimeManager());
        }
    }
}
