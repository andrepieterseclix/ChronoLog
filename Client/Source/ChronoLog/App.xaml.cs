using ChronoLog.Configuration;
using CLog.ServiceClients.Security;
using CLog.UI.Common;
using CLog.UI.Common.Modules;
using CLog.UI.Main;
using CLog.UI.Main.ViewModels;
using CLog.UI.Main.Views;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace ChronoLog
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.SetThreadPrincipal(new ClientPrincipal());
            Thread.CurrentThread.Name = CommonConstants.UI_THREAD_NAME;

            base.OnStartup(e);

            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run();

            ShellWindow window = bootstrapper.GetInstance<ShellWindow>();
            ShellViewModel viewModel = bootstrapper.GetInstance<ShellViewModel>();
            CompositeModule compositeModule = (CompositeModule)bootstrapper.GetInstance<IModuleInitialiser>();

            if (compositeModule?.Modules != null)
            {
                foreach (IModuleInitialiser initialiser in compositeModule.Modules)
                {
                    Module module = initialiser.Initialise(bootstrapper);
                    module.ViewModel.Initialise();

                    TabItem tabItem = new TabItem()
                    {
                        Header = module.Name,
                        Content = module.Control,
                    };

                    module.Control.Padding = new Thickness(5d);
                    module.Control.DataContext = module.ViewModel;
                    window.UITabs.Items.Add(tabItem);
                    viewModel.TabViewModels.Add(module.ViewModel);
                }
            }

            MainWindow = window;
            MainWindow.DataContext = viewModel;
            viewModel.Initialise();
            MainWindow.Show();
        }
    }
}
