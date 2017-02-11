using CLog.UI.Framework.Testing.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;

namespace ChronoLog.Tests.Integration.IntegrationTests
{
    [TestClass]
    public class CapturePublicHoliday : IntegrationTestBase
    {
        [TestMethod]
        [TestCategory("End-to-end Integration Tests")]
        public void ChronoLog_Integration_CapturePublicHoliday()
        {
            // Arrange
            Process applicationShellProcess = StartApplicationShellProcess(GetApplicationPath());
            AutomationSet set = new AutomationSet(() => applicationShellProcess.Kill());

            byte hoursWorked = 8;
            DateTime testDate = new DateTime(2017, 1, 1);
            string captureDateId = string.Format(CultureInfo.CurrentCulture, CAPTURE_DATE_ID_FORMAT, testDate);

            EnqueueEnterUserName(set);
            EnqueueEnterPassword(set);
            EnqueueLogin(set);
            EnqueueChangeDate(set, testDate);
            EnqueueCaptureDate(set, hoursWorked, captureDateId);
            EnqueueSaveCapturedTime(set);
            EnqueueLogout(set);
            EnqueueCloseLogoutWindow(set);

            // Act
            set.Run();

            // Assert
            DataTable capturedTimeTable = GetCapturedTimeData();
            Assert.IsNotNull(capturedTimeTable);
            Assert.AreEqual(capturedTimeTable.Rows.Count, 1);
            Assert.AreEqual(capturedTimeTable.Rows[0]["UserName"], USER_NAME);
            Assert.AreEqual(capturedTimeTable.Rows[0]["HoursWorked"], hoursWorked);
            Assert.AreEqual(capturedTimeTable.Rows[0]["Date"], testDate);
        }
    }
}
