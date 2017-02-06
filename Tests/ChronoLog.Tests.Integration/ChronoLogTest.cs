using CLog.UI.Framework.Testing.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Automation;

namespace ChronoLog.Tests.Integration
{
    [TestClass]
    public class ChronoLogTest : ChronoLogTestBase
    {
        #region Initialisation and Cleanup

        protected override void Cleanup()
        {
        }

        protected override void Init()
        {
        }

        #endregion

        #region Test Methods

        [TestMethod]
        [TestCategory("ChronoLog - End-to-end - Capture Time")]
        public void ChronoLog_Integration_CaptureTime()
        {
            // Arrange
            Process applicationShellProcess = StartApplicationShellProcess(GetApplicationPath());
            AutomationSet set = new AutomationSet(() => applicationShellProcess.Kill());

            // [Log In]
            set.Enqueue(new AutomationAction(
                () =>
                {
                    AutomationElement loginWindow =
                        AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.AutomationIdProperty, "Login_Window"));
                    
                    return loginWindow?.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, "Login_Button"));
                },
                loginButton =>
                {
                    InvokePattern invokePattern = (InvokePattern)loginButton.GetCurrentPattern(InvokePattern.Pattern);
                    invokePattern.Invoke();
                }));

            // [Change Date]
            set.Enqueue(new AutomationAction(
                () =>
                {
                    AutomationElement mainWindow =
                        AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.AutomationIdProperty, "Shell_Window"));

                    return mainWindow?.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, "Selected_Date"));
                },
                selectedDate =>
                {
                    ValuePattern valuePattern = (ValuePattern)selectedDate.GetCurrentPattern(ValuePattern.Pattern);
                    string value = valuePattern.Current.Value;
                    string newValue = new DateTime(2001, 1, 1).ToString(CultureInfo.CurrentCulture);
                    valuePattern.SetValue(newValue);
                }));

            // [Log Out]
            set.Enqueue(new AutomationAction(
                () =>
                {
                    AutomationElement mainWindow =
                        AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.AutomationIdProperty, "Shell_Window"));

                    return mainWindow?.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, "Logout_Button"));
                },
                logoutButton =>
                {
                    InvokePattern invokePattern = (InvokePattern)logoutButton.GetCurrentPattern(InvokePattern.Pattern);
                    invokePattern.Invoke();
                }));

            // [Close Login Window]
            set.Enqueue(new AutomationAction(
                () =>
                {
                    AutomationElement loginWindow =
                        AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.AutomationIdProperty, "Login_Window"));
                    
                    return loginWindow?.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, "Close_Button"));
                },
                closeButton =>
                {
                    InvokePattern invokePattern = (InvokePattern)closeButton.GetCurrentPattern(InvokePattern.Pattern);
                    invokePattern.Invoke();
                }));

            // Act
            set.Run();
            //applicationShellProcess.WaitForExit(); // This should block until the automation set completes...

            // Assert
            Assert.Inconclusive("WIP");
        }

        #endregion
    }
}
