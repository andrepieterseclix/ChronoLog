using CLog.Common.Logging;
using CLog.Framework.Configuration.Bootstrap;
using CLog.UI.Common.Modules;
using CLog.UI.Common.Services;
using CLog.UI.Framework.Testing.Helpers;
using CLog.UI.Framework.Testing.Models;
using CLog.UI.Framework.Testing.ViewModels;
using CLog.UI.Framework.Testing.Views;
using CLog.UI.Main;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace CLog.UI.Testing.Configuration
{
    public class TestsBootstrapper : UnityBootstrapper, IDependencyContainer
    {
        #region Constructors

        public TestsBootstrapper()
        {

        }

        #endregion

        #region Properties

        public TestModel Model { get; } = new TestModel();

        #endregion

        #region Methods

        protected override void DoRegistration()
        {
            // Share the test model for installers to use
            Container.RegisterInstance(Model);

            base.DoRegistration();
        }

        protected override void PostRegistration()
        {
            base.PostRegistration();

            // Scan solution to get IModuleInitialiser
            string[] assemblyPaths = VisualStudioHelper.GetSolutionOutputAssemblies();
            Type[] types = ReflectionHelper.GetTypesAssignableFrom(typeof(IModuleInitialiser), assemblyPaths)
                .Where(t => t != typeof(CompositeModule))
                .ToArray();

            Type moduleInitType = types.FirstOrDefault();

            if (types.Length != 1)
            {
                SelectModuleViewModel selectModuleViewModel = new SelectModuleViewModel(Container.Resolve<ILogger>(), types);
                SelectModuleWindow selectModuleWindow = new SelectModuleWindow() { DataContext = selectModuleViewModel };

                if (!selectModuleWindow.ShowDialog().Value)
                    return;

                moduleInitType = selectModuleViewModel.SelectedType;
            }

            // Configure the container, and initialise the module
            IModuleInitialiser initialiser = (IModuleInitialiser)Activator.CreateInstance(moduleInitType);
            Module module = initialiser.Initialise(this);

            // Setup Window
            TestViewModel testViewModel = new TestViewModel(Container.Resolve<ILogger>(), (StatusViewModel)Container.Resolve<IStatusService>());
            testViewModel.TestParameterModels.Add(Model.ViewModels);
            testViewModel.TestParameterModels.Add(Model.Mocks);
            testViewModel.TestParameterModels.Add(Model.Environment);
            TestWindow window = new TestWindow() { DataContext = testViewModel };
            window.ContentHost.Content = module.Control;
            module.Control.DataContext = module.ViewModel;
            Model.ViewModels.Children.Add(new ViewModelModel(module.ViewModel.GetType().Name, module.ViewModel));

            // Create Wpf Application
            Application app = new Application();
            app.DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            app.Run(window);
        }

        public void Register<T>()
        {
            Container.RegisterType<T>();
        }

        public T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        #endregion

        #region Event Handlers

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            LoggerHelper.Exception(Container.Resolve<ILogger>(), e.Exception, "Unhandled Exception");
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            LoggerHelper.Exception(Container.Resolve<ILogger>(), e.Exception, "Unhandled Exception");
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            if (ex != null)
                LoggerHelper.Exception(Container.Resolve<ILogger>(), ex, "Unhandled Exception");
            else
                LoggerHelper.Error(Container.Resolve<ILogger>(), "Unhandled Exception");
        }

        #endregion
    }
}
