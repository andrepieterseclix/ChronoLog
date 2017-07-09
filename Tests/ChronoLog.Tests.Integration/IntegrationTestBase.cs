using ChronoLog.Host.Configuration;
using CLog.Framework.Configuration.Bootstrap.TerminationStrategy;
using CLog.UI.Framework.Testing.Automation;
using CLog.UI.Framework.Testing.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Automation;

namespace ChronoLog.Tests.Integration
{
    public abstract class IntegrationTestBase : IDisposable
    {
        #region Fields

        private const string CLEAR_DATA_COMMAND = @"
            DELETE [dbo].[CapturedTime]
            DELETE [dbo].[DateState]
            DELETE [dbo].[Session]
            DELETE [dbo].[User]
        ";

        public const string KNOWN_STATE_1 = @"
            -- User
            INSERT [dbo].[User] ([State], [UserName], [Password], [Salt], [Name], [Surname], [Email], [ManagerId])
                VALUES (4, N'TestBot', N'rUCFPNntYl0OLM61tTLDsj8Nr2WZbk8znn+RIh8i+40=', N'G3NWV7jT', N'Tester', N'Botto', N'testbot@companyname.co.za', NULL)

            -- Date States
            INSERT [dbo].[DateState] ([Date], [IsLocked], [IsPublicHoliday])
                VALUES (CAST(N'2016-12-19 00:00:00.000' AS DateTime), 1, 0)
            INSERT [dbo].[DateState] ([Date], [IsLocked], [IsPublicHoliday])
                VALUES (CAST(N'2016-12-20 00:00:00.000' AS DateTime), 1, 0)
            INSERT [dbo].[DateState] ([Date], [IsLocked], [IsPublicHoliday])
                VALUES (CAST(N'2016-12-21 00:00:00.000' AS DateTime), 1, 0)
            INSERT [dbo].[DateState] ([Date], [IsLocked], [IsPublicHoliday])
                VALUES (CAST(N'2016-12-22 00:00:00.000' AS DateTime), 1, 0)
            INSERT [dbo].[DateState] ([Date], [IsLocked], [IsPublicHoliday])
                VALUES (CAST(N'2016-12-23 00:00:00.000' AS DateTime), 1, 0)
            INSERT [dbo].[DateState] ([Date], [IsLocked], [IsPublicHoliday])
                VALUES (CAST(N'2016-12-24 00:00:00.000' AS DateTime), 1, 0)
            INSERT [dbo].[DateState] ([Date], [IsLocked], [IsPublicHoliday])
                VALUES (CAST(N'2016-12-25 00:00:00.000' AS DateTime), 1, 0)
            
            INSERT [dbo].[DateState] ([Date], [IsLocked], [IsPublicHoliday])
                VALUES (CAST(N'2017-01-01 00:00:00.000' AS DateTime), 0, 1)
        ";

        protected DbConnection Connection { get; private set; }

        private readonly WcfConsoleBootstrapper _servicesHostBootstrapper = new WcfConsoleBootstrapper();

        private readonly ManualTerminationStrategy _servicesTerminationStrategy = new ManualTerminationStrategy();

        private string _uiProcessName;

        private readonly string EXE_FILE = "ChronoLog.exe";

        public const string MAIN_WINDOW_ID = "Shell_Window";

        public const string LOGIN_WINDOW_ID = "Login_Window";

        public const string USER_PROFILE_TAB_ID = "User_Profile";

        public const string USER_NAME_ID = "User_Name_Box";

        public const string PASSWORD_ID = "Password_Box";

        public const string LOGIN_BUTTON_ID = "Login_Button";

        public const string LOGOUT_BUTTON_ID = "Logout_Button";

        public const string CLOSE_BUTTON = "Close_Button";

        public const string SELECTED_DATE_ID = "Selected_Date";

        public const string CAPTURE_DATE_ID_FORMAT = "CaptureDate_{0:yyyyMMdd}";

        public const string SAVE_CAPTURED_TIME_BUTTON = "Save_Captured_Time_Button";

        public const string NAME_INPUT_TEXT = "Name_InputText";

        public const string SURNAME_INPUT_TEXT = "Surname_InputText";

        public const string EMAIL_INPUT_TEXT = "Email_InputText";

        public const string SAVE_PROFILE_BUTTON = "Save_Profile_Button";

        public const string USER_NAME = "TestBot";

        public const string PASSWORD = "P@ssw0rd123";

        #endregion

        #region Constructors

        public IntegrationTestBase()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
            Connection = new SqlConnection(connectionString);
        }

        #endregion

        #region Test Initialisation and Cleanup

        protected virtual void Init()
        {
            ClearData();
            CreateKnownStateOfData();
            Connection.Close();
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

        protected DbCommand NewCommand(string commandText)
        {
            DbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = commandText;

            return cmd;
        }

        protected DbDataAdapter NewDataAdapter()
        {
            return new SqlDataAdapter();
        }

        private void ClearData()
        {
            using (DbCommand cmd = NewCommand(CLEAR_DATA_COMMAND))
            {
                cmd.Connection = Connection;
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                cmd.ExecuteNonQuery();
            }
        }

        private void CreateKnownStateOfData()
        {
            using (DbCommand cmd = NewCommand(KNOWN_STATE_1))
            {
                cmd.Connection = Connection;
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                cmd.ExecuteNonQuery();
            }
        }

        protected DataTable GetData(string commandText)
        {
            DataTable table = new DataTable();

            using (DbCommand cmd = NewCommand(commandText))
            {
                cmd.Connection = Connection;
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                using (DbDataAdapter adapter = NewDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(table);
                }
            }

            return table;
        }

        protected DataTable GetCapturedTimeData()
        {
            string query = string.Format(CultureInfo.CurrentCulture, @"
                SELECT
	                u.UserName,
	                ct.*
                FROM
	                [dbo].[CapturedTime] ct
	                join [dbo].[User] u on
		                u.Id = ct.UserId
                WHERE
	                u.UserName = '{0}'", USER_NAME);

            return GetData(query);
        }

        protected AutomationElement GetAutomationElement(string windowId, string elementId)
        {
            AutomationElement mainWindow =
                AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.AutomationIdProperty, windowId));

            AutomationElement element =
                mainWindow?.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, elementId));

            return element;
        }

        // UI

        protected void EnqueueEnterUserName(AutomationSet set)
        {
            set.Enqueue(new AutomationAction(
                () => GetAutomationElement(LOGIN_WINDOW_ID, USER_NAME_ID),
                userNameText =>
                {
                    if (!userNameText.Current.IsEnabled)
                        return false;

                    ValuePattern pattern = ((ValuePattern)userNameText.GetCurrentPattern(ValuePattern.Pattern));
                    if (pattern == null)
                        return false;

                    pattern.SetValue(USER_NAME);

                    return true;
                }));
        }

        protected void EnqueueEnterPassword(AutomationSet set)
        {
            set.Enqueue(new AutomationAction(
                () => GetAutomationElement(LOGIN_WINDOW_ID, PASSWORD_ID),
                userNameText =>
                {
                    if (!userNameText.Current.IsEnabled)
                        return false;

                    ValuePattern pattern = ((ValuePattern)userNameText.GetCurrentPattern(ValuePattern.Pattern));
                    if (pattern == null)
                        return false;

                    pattern.SetValue(PASSWORD);

                    return true;
                }));
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

        protected void EnqueueChangeTab(AutomationSet set, string tabId)
        {
            set.Enqueue(new AutomationAction(
                () => GetAutomationElement(MAIN_WINDOW_ID, tabId),
                tab =>
                {
                    if (!tab.Current.IsEnabled)
                        return false;

                    ((SelectionItemPattern)tab.GetCurrentPattern(SelectionItemPattern.Pattern)).Select();

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

        protected void EnqueueCaptureDate(AutomationSet set, byte hoursWorked, string captureDateId)
        {
            set.Enqueue(new AutomationAction(
                () => GetAutomationElement(MAIN_WINDOW_ID, captureDateId),
                captureDate =>
                {
                    if (!captureDate.Current.IsEnabled)
                        return false;

                    ValuePattern pattern = (ValuePattern)captureDate.GetCurrentPattern(ValuePattern.Pattern);
                    if (pattern == null)
                        return false;

                    pattern.SetValue(hoursWorked.ToString(CultureInfo.CurrentCulture));

                    return true;
                }));
        }

        protected void EnqueueSaveCapturedTime(AutomationSet set)
        {
            set.Enqueue(new AutomationAction(
                () => GetAutomationElement(MAIN_WINDOW_ID, SAVE_CAPTURED_TIME_BUTTON),
                saveButton =>
                {
                    if (!saveButton.Current.IsEnabled)
                        return false;

                    ((InvokePattern)saveButton.GetCurrentPattern(InvokePattern.Pattern)).Invoke();

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

        // Non-UI

        protected void EnqueueLockDatesServerSide(AutomationSet set, params DateTime[] dates)
        {
            if (dates.Length < 1)
                return;

            set.Enqueue(new NonUIAction(() =>
            {
                string[] dateStrings = dates.Select(x => string.Format("('{0:yyyy-MM-dd}')", x)).ToArray();

                string commandText = string.Format(@"
                    DECLARE @Source table ([Date] DateTime)

                    INSERT @Source VALUES
                    {0}

                    MERGE dbo.[DateState] ds
                    USING @Source s ON
	                    ds.[Date] = s.[Date]
                    WHEN MATCHED THEN
	                    UPDATE SET ds.[IsLocked] = 1
                    WHEN NOT MATCHED BY TARGET THEN
	                    INSERT ([Date], [IsLocked], [IsPublicHoliday])
	                    VALUES (s.[Date], 1, 0);", string.Join(",\r\n                    ", dateStrings));

                using (DbCommand cmd = NewCommand(commandText))
                {
                    cmd.Connection = Connection;
                    if (Connection.State != ConnectionState.Open)
                        Connection.Open();

                    cmd.ExecuteNonQuery();
                }
            }));
        }

        #endregion

        #region IDisposable Implementation

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Release managed resources, and set to null
                if (Connection != null)
                {
                    Connection.Dispose();
                }
            }

            // Release native resources
            // NOTE:  call Dispose(false); in finalizer if this class contains unmanaged resources.

        }

        #endregion
    }
}
