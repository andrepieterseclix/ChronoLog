using CLog.Host.Configuration;
using System;

namespace CLog.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            DoIntroduction();

            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }

        static void DoIntroduction()
        {
            string header = "Services Host";
            int boxLength = header.Length + 4;

            Console.WriteLine(string.Empty.PadLeft(boxLength, '*'));
            Console.WriteLine("*".PadRight(boxLength - 1).PadRight(boxLength, '*'));
            Console.WriteLine(string.Format("* {0}", header).PadRight(boxLength - 1).PadRight(boxLength, '*'));
            Console.WriteLine("*".PadRight(boxLength - 1).PadRight(boxLength, '*'));
            Console.WriteLine(string.Empty.PadLeft(boxLength, '*'));
            Console.WriteLine();
        }
    }
}
