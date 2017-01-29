using CLog.Common.Logging;
using CLog.UI.Common.ViewModels;
using CLog.UI.Framework.Testing.Views;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace CLog.UI.Framework.Testing
{
    public class Bootstrapper
    {
        #region Fields

        private readonly ILogger _logger;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public Bootstrapper(ILogger logger)
        {
            _logger = logger;
        }

        #endregion

        #region Methods

        public void Run(UserControl userControl, ViewModelBase viewModel)
        {
            // Setup Window
            TestWindow window = new TestWindow();
            window.ContentHost.Content = userControl;
            userControl.DataContext = viewModel;

            // Create Wpf Application
            Application app = new Application();
            app.DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            app.Run(window);
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
