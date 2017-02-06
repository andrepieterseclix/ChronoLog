using ChronoLog.Host.Configuration;
using CLog.Framework.Configuration.Bootstrap.TerminationStrategy;
using CLog.UI.Framework.Testing.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace ChronoLog.Tests.Integration
{
    public abstract class ChronoLogTestBase
    {
        #region Fields

        private readonly string EXE_FILE = "ChronoLog.exe";

        private readonly Bootstrapper _servicesHostBootstrapper = new Bootstrapper();

        private readonly ManualTerminationStrategy _servicesTerminationStrategy = new ManualTerminationStrategy();

        #endregion

        #region Test Initialisation and Cleanup

        protected abstract void Init();

        protected abstract void Cleanup();

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
            _servicesTerminationStrategy.Terminate();
            _servicesHostBootstrapper.Wait();

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

        #endregion
    }
}
