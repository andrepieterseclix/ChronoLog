using System;
using System.Linq;

namespace CLog.Framework.Configuration.Helpers
{
    /// <summary>
    /// Represents the static class used to print an introduction box for console type applications.
    /// </summary>
    public static class ConsoleHelper
    {
        private const int SPACE = 4;

        /// <summary>
        /// Prints the introduction box.
        /// </summary>
        /// <param name="lines">The lines.</param>
        public static void PrintIntroductionBox(params string[] lines)
        {
            if (lines == null || lines.Length == 0)
                return;

            int boxLength = lines.Max(x => x.Length) + SPACE;

            Console.WriteLine(string.Empty.PadLeft(boxLength, '*'));
            Console.WriteLine("*".PadRight(boxLength - 1).PadRight(boxLength, '*'));

            foreach (string line in lines)
            {
                Console.WriteLine(string.Format("* {0}", line).PadRight(boxLength - 1).PadRight(boxLength, '*'));
            }

            Console.WriteLine("*".PadRight(boxLength - 1).PadRight(boxLength, '*'));
            Console.WriteLine(string.Empty.PadLeft(boxLength, '*'));
            Console.WriteLine();
        }

        /// <summary>
        /// Loops the until key pressed.
        /// </summary>
        /// <param name="q">The console key.</param>
        public static void LoopUntilKeyPressed(ConsoleKey key)
        {
            Console.WriteLine("Press '{0}' to quit...\r\n", key);

            ConsoleKeyInfo keyInfo;

            while ((keyInfo = Console.ReadKey(true)).Key != key)
            {
            }
        }
    }
}
