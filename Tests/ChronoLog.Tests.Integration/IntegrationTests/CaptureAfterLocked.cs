using CLog.UI.Framework.Testing.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;

namespace ChronoLog.Tests.Integration.IntegrationTests
{
    [TestClass]
    public class CaptureAfterLocked : IntegrationTestBase
    {
        [TestMethod]
        [TestCategory("End-to-end Integration Tests")]
        public void ChronoLog_Integration_CaptureAfterLocked()
        {
            // Arrange
            Process applicationShellProcess = StartApplicationShellProcess(GetApplicationPath());
            AutomationSet set = new AutomationSet(() => applicationShellProcess.Kill());

            byte hoursWorked = 8;
            DateTime testDate = new DateTime(2017, 2, 1);
            string captureDateId = string.Format(CultureInfo.CurrentCulture, CAPTURE_DATE_ID_FORMAT, testDate);

            EnqueueEnterUserName(set);
            EnqueueEnterPassword(set);
            EnqueueLogin(set);
            EnqueueChangeDate(set, testDate);
            EnqueueCaptureDate(set, hoursWorked, captureDateId);
            EnqueueLockDates(set, testDate);  // After this the UI still thinks the date is not locked, server must handle the situation and return an error
            EnqueueSaveCapturedTime(set);
            EnqueueLogout(set);
            EnqueueCloseLogoutWindow(set);

            // Act
            set.Run();

            // Assert
            DataTable capturedTimeTable = GetCapturedTimeData();
            Assert.IsNotNull(capturedTimeTable);
            Assert.AreEqual(capturedTimeTable.Rows.Count, 0);
        }
    }
}
