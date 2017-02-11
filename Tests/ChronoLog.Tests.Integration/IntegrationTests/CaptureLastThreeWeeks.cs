using CLog.UI.Framework.Testing.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;

namespace ChronoLog.Tests.Integration.IntegrationTests
{
    [TestClass]
    public class CaptureLastThreeWeeks : IntegrationTestBase
    {
        [TestMethod]
        [TestCategory("End-to-end Integration Tests")]
        public void ChronoLog_Integration_CaptureLastThreeWeeks()
        {
            // Arrange
            Process applicationShellProcess = StartApplicationShellProcess(GetApplicationPath());
            AutomationSet set = new AutomationSet(() => applicationShellProcess.Kill());

            byte hoursWorked = 8;
            int weeks = 3;
            DateTime testDate = DateTime.Now.Date.AddDays(-7 * weeks);

            EnqueueEnterUserName(set);
            EnqueueEnterPassword(set);
            EnqueueLogin(set);

            EnqueueChangeDate(set, testDate);

            while (testDate != DateTime.Now.Date.AddDays(1))
            {
                string captureDateId = string.Format(CultureInfo.CurrentCulture, CAPTURE_DATE_ID_FORMAT, testDate);

                EnqueueCaptureDate(set, hoursWorked, captureDateId);

                // Save if end of week
                if (testDate.DayOfWeek == DayOfWeek.Sunday || testDate == DateTime.Now.Date)
                    EnqueueSaveCapturedTime(set);

                testDate = testDate.AddDays(1);

                if (testDate.DayOfWeek == DayOfWeek.Monday)
                    EnqueueChangeDate(set, testDate);
            }

            EnqueueLogout(set);
            EnqueueCloseLogoutWindow(set);

            // Act
            set.Run();

            // Assert
            DataTable capturedTimeTable = GetCapturedTimeData();
            Assert.IsNotNull(capturedTimeTable);
            Assert.AreEqual(capturedTimeTable.Rows.Count, (7 * weeks) + 1);
        }
    }
}
