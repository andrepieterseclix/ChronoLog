using CLog.Common.Log4NetLogger;
using CLog.Common.Logging;
using CLog.UI.Common.Modules;
using CLog.UI.Common.ViewModels;
using CLog.UI.Framework.Testing.Helpers;
using CLog.UI.Framework.Testing.ViewModels;
using CLog.UI.Framework.Testing.Views;
using Microsoft.Practices.Unity;
using Moq;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace CLog.UI.Framework.Testing
{
    public class Bootstrapper : IDependencyContainer
    {
        #region Fields

        private readonly ILogger _logger;

        private readonly IUnityContainer _container;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public Bootstrapper(ILogger logger)
        {
            _logger = logger;
            _container = new UnityContainer();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Runs the test by scanning the solution for <see cref="IModuleInitialiser"/> implementations.
        /// </summary>
        public void Run()
        {
            ILogger logger = new Log4NetLogger();

            // Scan solution to get IModuleInitialiser
            string[] assemblyPaths = VisualStudioHelper.GetSolutionOutputAssemblies();
            Type[] types = ReflectionHelper.GetTypesAssignableFrom(typeof(IModuleInitialiser), assemblyPaths);
            Type moduleInitType = types.FirstOrDefault();

            if (types.Length != 1)
            {
                SelectModuleViewModel selectModuleViewModel = new SelectModuleViewModel(logger, types);
                SelectModuleWindow selectModuleWindow = new SelectModuleWindow() { DataContext = selectModuleViewModel };

                if (!selectModuleWindow.ShowDialog().Value)
                    return;

                moduleInitType = selectModuleViewModel.SelectedType;
            }

            // Configure the container, and initialise the module
            IModuleInitialiser initialiser = (IModuleInitialiser)Activator.CreateInstance(moduleInitType);
            ConfigureContainer();
            Common.Modules.Module module = initialiser.Initialise(this);
            
            Run(module.Control, module.ViewModel);
        }

        /// <summary>
        /// Runs the test using the specified user control and view model.
        /// </summary>
        /// <param name="viewElement">The user control.</param>
        /// <param name="viewModel">The view model.</param>
        public void Run(FrameworkElement viewElement, ViewModelBase viewModel)
        {
            // Setup Window
            TestWindow window = new TestWindow();
            window.ContentHost.Content = viewElement;
            viewElement.DataContext = viewModel;

            // Create Wpf Application
            Application app = new Application();
            app.DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            app.Run(window);
        }

        public void Register<T>()
        {
            // Check if the dependencies are registered, otherwise register mocks
            Type type = typeof(T);

            ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

            var dependencies = constructors
                .SelectMany(c => c.GetParameters())
                .Select(p => p.ParameterType)
                .Distinct();

            foreach (Type dependencyType in dependencies)
            {
                if (!_container.IsRegistered(dependencyType))
                {
                    Type mockType = typeof(Mock<>).MakeGenericType(dependencyType);
                    object mock = Activator.CreateInstance(mockType);
                    dynamic mockDynamic = mock as dynamic;
                    MethodInfo registerMethod = typeof(IUnityContainer).GetMethod("RegisterInstance", new[] { typeof(Type), typeof(string), typeof(object), typeof(LifetimeManager) });

                    registerMethod.Invoke(_container, new object[] { dependencyType, null, mockDynamic.Object, new ContainerControlledLifetimeManager() });
                }
            }

            // Register the type
            _container.RegisterType<T>();
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        #endregion

        #region Helper Methods

        private void ConfigureContainer()
        {
            _container.RegisterInstance(_logger);
        }

        #endregion

        #region Event Handlers

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            LoggerHelper.Exception(_logger, e.Exception, "Unhandled Exception");
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            LoggerHelper.Exception(_logger, e.Exception, "Unhandled Exception");
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            if (ex != null)
                LoggerHelper.Exception(_logger, ex, "Unhandled Exception");
            else
                LoggerHelper.Error(_logger, "Unhandled Exception");
        }

        #endregion
    }
}
