using CLog.UI.Framework.Testing.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace ChronoLog.Tests.Integration
{
    [TestClass]
    public class LogInLogOut : IntegrationTestBase
    {
        [TestMethod]
        [TestCategory("End-to-end Integration Tests")]
        public void ChronoLog_Integration_LogInLogOut()
        {
            // Arrange
            Process applicationShellProcess = StartApplicationShellProcess(GetApplicationPath());
            AutomationSet set = new AutomationSet(() => applicationShellProcess.Kill());

            EnqueueLogin(set);
            EnqueueLogout(set);
            EnqueueCloseLogoutWindow(set);

            // Act
            set.Run();
        }
    }
}
