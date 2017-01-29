using System;
using System.Linq;
using System.Threading;

namespace CLog.Clients.IntegrationTests
{
    class Program
    {
        private static int FailedTestsCount = 0;

        static void Main(string[] args)
        {
            try
            {
                DoIntroduction();

                var tests = typeof(Program)
                    .Assembly
                    .GetTypes()
                    .Where(t => typeof(ScriptedTest).IsAssignableFrom(t) && !t.IsAbstract)
                    .Select(t => Activator.CreateInstance(t))
                    .ToArray();

                foreach (ScriptedTest test in tests)
                {
                    test.ExceptionThrown += Test_ExceptionThrown;
                    test.RunTest();
                    test.ExceptionThrown -= Test_ExceptionThrown;
                }

                Console.WriteLine();
                if (FailedTestsCount > 0)
                    Console.WriteLine("-- Finished with errors, {0} tests failed --", FailedTestsCount);
                else
                    Console.WriteLine("-- All tests ran successfully --");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\r\n{0}", ex);
                Console.WriteLine("-- Finished with Errors --");
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void Test_ExceptionThrown(object sender, EventArgs e)
        {
            FailedTestsCount++;
        }

        static void DoIntroduction()
        {
            string header = "Services Integration Tests";
            string subheader = "Please make sure the services are running";
            int boxLength = (new[] { header.Length, subheader.Length }).Max() + 4;

            Console.WriteLine(string.Empty.PadLeft(boxLength, '*'));
            Console.WriteLine(string.Format("* {0}", header).PadRight(boxLength - 1).PadRight(boxLength, '*'));
            Console.WriteLine(string.Format("* {0}", subheader).PadRight(boxLength - 1).PadRight(boxLength, '*'));
            Console.WriteLine(string.Empty.PadLeft(boxLength, '*'));
            Console.WriteLine();

            for (int i = 10; i >= 0; i--)
            {
                Console.Write("{0}  ", i);
                Thread.Sleep(250);
            }

            Console.WriteLine("\r\nStarting Tests:");
        }
    }
}
