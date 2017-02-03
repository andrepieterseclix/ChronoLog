using CLog.Common.Logging;
using CLog.Framework.Configuration.Bootstrap;
using CLog.ServiceClients.Security;
using CLog.Services.Models;
using CLog.UI.Common.Modules;
using CLog.UI.Common.Services;
using CLog.UI.Framework.Testing.Helpers;
using CLog.UI.Framework.Testing.Models;
using CLog.UI.Framework.Testing.ViewModels;
using CLog.UI.Framework.Testing.Views;
using CLog.UI.Main;
using CLog.UI.Models;
using Microsoft.Practices.Unity;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace CLog.UI.Testing.Configuration
{
    public class Bootstrapper : UnityBootstrapper, IDependencyContainer
    {
        #region Fields

        private string _testModuleAssemblyPath;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
        /// </summary>
        /// <remarks>
        /// When the test is ran from within Visual Studio, it will scan the solution for modules automatically.
        /// An assembly is considered a module if it contains a class that implements <see cref="IModuleInitialiser"/>.
        /// </remarks>
        public Bootstrapper()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
        /// </summary>
        /// <param name="testModuleAssemblyPath">The test module assembly path.</param>
        public Bootstrapper(string testModuleAssemblyPath)
        {
            _testModuleAssemblyPath = testModuleAssemblyPath;
        }

        #endregion

        #region Properties

        public TestModel Model { get; } = new TestModel();

        #endregion

        #region Methods

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            AppDomain.CurrentDomain.SetThreadPrincipal(new ClientPrincipal());
        }

        protected override void DoRegistration()
        {
            // Share the test model for installers to use
            Container.RegisterInstance(Model);

            base.DoRegistration();
        }

        protected override void PostRegistration()
        {
            base.PostRegistration();

            ModuleAssemblyModel testModuleAssembly = ResolveTestModule();

            if (testModuleAssembly == null)
                return;

            Console.WriteLine("Resolved module initialiser '{0}' in assembly '{1}'...", testModuleAssembly.ModuleClassName, testModuleAssembly.AssemblyPath);
            Type moduleInitType = ReflectionHelper.LoadTypeExternal(testModuleAssembly);

            ComponentModelHelper.MakeObjectsExpandable(moduleInitType.Assembly);
            ComponentModelHelper.MakeObjectsExpandable(typeof(DtoBase).Assembly);
            ComponentModelHelper.MakeObjectsExpandable(typeof(ModelBase).Assembly);

            // Configure the container, and initialise the module
            IModuleInitialiser initialiser = (IModuleInitialiser)Activator.CreateInstance(moduleInitType);
            Module module = initialiser.Initialise(this);

            // Setup Window
            TestViewModel testViewModel = new TestViewModel(
                Container.Resolve<ILogger>(),
                Container.Resolve<IStatusService>(),
                Container.Resolve<IDialogService>(),
                Container.Resolve<IMouseService>(),
                (StatusViewModel)Container.Resolve<IStatusService>());
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

        #region Helper Methods

        private ModuleAssemblyModel ResolveTestModule()
        {
            ModuleAssemblyModel[] modules = null;

            if (!string.IsNullOrWhiteSpace(_testModuleAssemblyPath))
            {
                modules =
                    ReflectionHelper.GetTypesAssignableFrom(typeof(IModuleInitialiser), t => t.Name != typeof(CompositeModule).Name, _testModuleAssemblyPath);

                if (modules.Length == 0)
                    throw new ArgumentException($"The specified assembly '{_testModuleAssemblyPath}' does not contain a type that implements {nameof(IModuleInitialiser)}.");
            }
            else
            {
                // Scan solution to get IModuleInitialiser
                string[] assemblyPaths = VisualStudioHelper.GetSolutionOutputAssemblies();

                modules =
                    ReflectionHelper.GetTypesAssignableFrom(typeof(IModuleInitialiser), t => t.Name != typeof(CompositeModule).Name, assemblyPaths);
            }

            ModuleAssemblyModel moduleInitType = modules.FirstOrDefault();

            if (modules.Length != 1)
            {
                SelectModuleViewModel selectModuleViewModel = new SelectModuleViewModel(
                    Container.Resolve<ILogger>(),
                    Container.Resolve<IStatusService>(),
                    Container.Resolve<IDialogService>(),
                    Container.Resolve<IMouseService>(),
                    modules);
                SelectModuleWindow selectModuleWindow = new SelectModuleWindow() { DataContext = selectModuleViewModel };

                if (!selectModuleWindow.ShowDialog().Value)
                    return null;

                moduleInitType = selectModuleViewModel.SelectedType;
            }

            return moduleInitType;
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
