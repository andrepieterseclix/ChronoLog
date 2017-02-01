using CLog.Framework.Configuration.Helpers;
using CLog.UI.Testing.Configuration;
using System;

namespace CLog.UI.TestBench
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ConsoleHelper.PrintIntroductionBox("ChronoLog UI", "Test Bench");

            Bootstrapper bootstrapper = (args.Length > 0)
                ? new Bootstrapper(args[0])
                : new Bootstrapper();

            bootstrapper.Run();
        }
    }
}
