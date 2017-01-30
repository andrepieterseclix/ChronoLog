using CLog.Framework.Configuration.Bootstrap;
using CLog.UI.Common.Services;
using CLog.UI.Framework.Testing.ViewModels;
using CLog.UI.Main.Services;
using Microsoft.Practices.Unity;

namespace CLog.UI.Testing.Configuration.Installers
{
    public class MainModuleInstaller : IUnityDependencyInstaller
    {
        public void Install(IUnityContainer container)
        {
            // Services
            container
                .RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());

            container
                .RegisterType<IMouseService, MouseService>(new ContainerControlledLifetimeManager());

            container
                .RegisterType<IStatusService, StatusViewModel>(new ContainerControlledLifetimeManager());
        }
    }
}
