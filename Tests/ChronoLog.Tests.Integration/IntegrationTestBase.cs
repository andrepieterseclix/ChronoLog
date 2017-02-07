using ChronoLog.Host.Configuration;
using CLog.Framework.Configuration.Bootstrap.TerminationStrategy;
using CLog.UI.Framework.Testing.Automation;
using CLog.UI.Framework.Testing.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Automation;

namespace ChronoLog.Tests.Integration
{
    public abstract class IntegrationTestBase
    {
        #region Fields

        private readonly string EXE_FILE = "ChronoLog.exe";

        private readonly Bootstrapper _servicesHostBootstrapper = new Bootstrapper();

        private readonly ManualTerminationStrategy _servicesTerminationStrategy = new ManualTerminationStrategy();

        private string _uiProcessName;

        public const string MAIN_WINDOW_ID = "Shell_Window";

        public const string LOGIN_WINDOW_ID = "Login_Window";

        public const string LOGIN_BUTTON_ID = "Login_Button";

        public const string LOGOUT_BUTTON_ID = "Logout_Button";

        public const string CLOSE_BUTTON = "Close_Button";

        public const string SELECTED_DATE_ID = "Selected_Date";

        public const string CAPTURE_DATE_ID_FORMAT = "CaptureDate_{0:yyyyMMdd}";

        #endregion

        #region Test Initialisation and Cleanup

        protected virtual void Init()
        {
        }

        protected virtual void Cleanup()
        {
        }

        [TestInitialize]
        public void TestInit()
        {
            // Run Services
            Thread servicesThread = new Thread(() => RunServices(_servicesTerminationStrategy));
            servicesThread.Start();

            Init();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Make sure the services are stopped
            _servicesTerminationStrategy.Terminate();
            _servicesHostBootstrapper.Wait();

            // Make sure the client process is stopped
            RetryHelper.Retry(5, 200, () =>
            {
                Process[] clientProcesses = Process.GetProcessesByName(_uiProcessName);
                if (clientProcesses != null)
                {
                    foreach (Process process in clientProcesses)
                        process.Kill();
                }
            });

            Cleanup();
        }

        #endregion

        #region Helper Methods

        protected string GetApplicationPath()
        {
            string assemblyPath = VisualStudioHelper
                .GetSolutionOutputAssemblies()
                .FirstOrDefault(x => Path.GetFileName(x) == EXE_FILE);

            if (assemblyPath == null || !File.Exists(assemblyPath))
                throw new InternalTestFailureException(string.Format("Could not find the assembly {0}", EXE_FILE));

            return assemblyPath;
        }

        protected Process StartApplicationShellProcess(string assemblyPath)
        {
            _uiProcessName = Path.GetFileNameWithoutExtension(assemblyPath);

            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "CMD.EXE",
                Arguments = string.Format("/K {0}", assemblyPath),
                CreateNoWindow = true,
                UseShellExecute = false
            };

            return Process.Start(startInfo);
        }

        private void RunServices(ITerminationStrategy terminationStrategy)
        {
            _servicesHostBootstrapper.TerminationStrategy = terminationStrategy;
            _servicesHostBootstrapper.Run();
        }

        protected AutomationElement GetAutomationElement(string windowId, string elementId)
        {
            AutomationElement mainWindow =
                AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.AutomationIdProperty, windowId));

            return mainWindow?.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, elementId));
        }

        protected void EnqueueLogin(AutomationSet set)
        {
            set.Enqueue(new AutomationAction(
                () => GetAutomationElement(LOGIN_WINDOW_ID, LOGIN_BUTTON_ID),
                loginButton =>
                {
                    if (!loginButton.Current.IsEnabled)
                        return false;

                    ((InvokePattern)loginButton.GetCurrentPattern(InvokePattern.Pattern)).Invoke();

                    return true;
                }));
        }

        protected void EnqueueChangeDate(AutomationSet set, DateTime date)
        {
            set.Enqueue(new AutomationAction(
                () => GetAutomationElement(MAIN_WINDOW_ID, SELECTED_DATE_ID),
                selectedDate =>
                {
                    if (!selectedDate.Current.IsEnabled)
                        return false;

                    ValuePattern valuePattern = (ValuePattern)selectedDate.GetCurrentPattern(ValuePattern.Pattern);
                    valuePattern.SetValue(date.ToString(CultureInfo.CurrentCulture));

                    return true;
                }));
        }

        protected void EnqueueLogout(AutomationSet set)
        {
            set.Enqueue(new AutomationAction(
                () => GetAutomationElement(MAIN_WINDOW_ID, LOGOUT_BUTTON_ID),
                logoutButton =>
                {
                    if (!logoutButton.Current.IsEnabled)
                        return false;

                    ((InvokePattern)logoutButton.GetCurrentPattern(InvokePattern.Pattern)).Invoke();

                    return true;
                }));
        }

        protected void EnqueueCloseLogoutWindow(AutomationSet set)
        {
            set.Enqueue(new AutomationAction(
                () => GetAutomationElement(LOGIN_WINDOW_ID, CLOSE_BUTTON),
                closeButton =>
                {
                    if (!closeButton.Current.IsEnabled)
                        return false;

                    ((InvokePattern)closeButton.GetCurrentPattern(InvokePattern.Pattern)).Invoke();

                    return true;
                }));
        }

        #endregion
    }
}
