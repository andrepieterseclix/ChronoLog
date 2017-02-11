using CLog.UI.Framework.Testing.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Globalization;

namespace ChronoLog.Tests.Integration.IntegrationTests
{
    [TestClass]
    public class LockedDatesDisabled : IntegrationTestBase
    {
        [TestMethod]
        [TestCategory("End-to-end Integration Tests")]
        public void ChronoLog_Integration_LockedDateDisabled()
        {
            // Arrange
            Process applicationShellProcess = StartApplicationShellProcess(GetApplicationPath());
            AutomationSet set = new AutomationSet(() => applicationShellProcess.Kill());
            
            DateTime testDate = new DateTime(2016, 12, 19); ;
            bool? captureDateEnabled = null;

            EnqueueEnterUserName(set);
            EnqueueEnterPassword(set);
            EnqueueLogin(set);

            // Check that the whole week is locked/disabled
            for (int i = 0; i < 7; i++)
            {
                string captureDateId = string.Format(CultureInfo.CurrentCulture, CAPTURE_DATE_ID_FORMAT, testDate);

                EnqueueChangeDate(set, testDate);

                set.Enqueue(new AutomationAction(
                    () => GetAutomationElement(MAIN_WINDOW_ID, captureDateId),
                    captureDate =>
                    {
                        captureDateEnabled = captureDateEnabled.GetValueOrDefault() || captureDate.Current.IsEnabled;
                        return true;
                    }));

                testDate = testDate.AddDays(1);
            }

            EnqueueLogout(set);
            EnqueueCloseLogoutWindow(set);

            // Act
            set.Run();

            // Assert
            Assert.IsNotNull(captureDateEnabled);
            Assert.IsFalse(captureDateEnabled.Value);
        }
    }
}
