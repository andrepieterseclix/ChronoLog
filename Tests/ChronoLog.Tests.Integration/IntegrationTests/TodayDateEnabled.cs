﻿using CLog.UI.Framework.Testing.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Globalization;

namespace ChronoLog.Tests.Integration.IntegrationTests
{
    [TestClass]
    public class TodayDateEnabled : IntegrationTestBase
    {
        [TestMethod]
        [TestCategory("End-to-end Integration Tests")]
        public void ChronoLog_Integration_TodayEnabled()
        {
            // Arrange
            Process applicationShellProcess = StartApplicationShellProcess(GetApplicationPath());
            AutomationSet set = new AutomationSet(() => applicationShellProcess.Kill());

            DateTime testDate = DateTime.Now;
            string captureDateId = string.Format(CultureInfo.CurrentCulture, CAPTURE_DATE_ID_FORMAT, testDate);
            bool? captureDateEnabled = null;

            EnqueueEnterUserName(set);
            EnqueueEnterPassword(set);
            EnqueueLogin(set);
            EnqueueChangeDate(set, testDate);

            set.Enqueue(new AutomationAction(
                () => GetAutomationElement(MAIN_WINDOW_ID, captureDateId),
                captureDate =>
                {
                    captureDateEnabled = captureDate.Current.IsEnabled;
                    return true;
                }));

            EnqueueLogout(set);
            EnqueueCloseLogoutWindow(set);

            // Act
            set.Run();

            // Assert
            Assert.IsNotNull(captureDateEnabled);
            Assert.IsTrue(captureDateEnabled.Value);
        }
    }
}
