using CLog.UI.Framework.Testing.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace ChronoLog.Tests.Integration
{
    [TestClass]
    public class TestFutureDateDisabled : IntegrationTestBase
    {
        [TestMethod]
        [TestCategory("End-to-end Integration Tests")]
        public void ChronoLog_Integration_FutureDateDisabled()
        {
            // Arrange
            Process applicationShellProcess = StartApplicationShellProcess(GetApplicationPath());
            AutomationSet set = new AutomationSet(() => applicationShellProcess.Kill());

            EnqueueLogin(set);
            EnqueueChangeDate(set, new DateTime(2200, 1, 1));

            //set.Enqueue(new AutomationAction(
            //    () =>
            //    {

            //    }))

            EnqueueLogout(set);
            EnqueueCloseLogoutWindow(set);

            // Act
            set.Run();
        }
    }
}
