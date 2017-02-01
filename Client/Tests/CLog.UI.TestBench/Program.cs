using CLog.UI.Testing.Configuration;
using System;

namespace CLog.UI.TestBench
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            TestsBootstrapper bootstrapper = (args.Length > 0)
                ? new TestsBootstrapper(args[0])
                : new TestsBootstrapper();

            bootstrapper.Run();
        }
    }
}
