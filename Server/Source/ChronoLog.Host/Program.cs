using ChronoLog.Host.Configuration;
using CLog.Framework.Configuration.Helpers;

namespace ChronoLog.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleHelper.PrintIntroductionBox("ChronoLog", "Services Host");

            WcfConsoleBootstrapper bootstrapper = new WcfConsoleBootstrapper();
            bootstrapper.Run();
        }
    }
}
