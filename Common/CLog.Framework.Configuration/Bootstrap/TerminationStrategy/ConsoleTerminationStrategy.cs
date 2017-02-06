using CLog.Framework.Configuration.Helpers;
using System;

namespace CLog.Framework.Configuration.Bootstrap.TerminationStrategy
{
    /// <summary>
    /// Represents the console host termination strategy.
    /// </summary>
    /// <seealso cref="ChronoLog.Host.Configuration.TerminationStrategy.ITerminationStrategy" />
    public sealed class ConsoleTerminationStrategy : ITerminationStrategy
    {
        /// <summary>
        /// Runs until the console reads the 'Q' letter.
        /// </summary>
        public void Run()
        {
            Console.WriteLine();
            ConsoleHelper.LoopUntilKeyPressed(ConsoleKey.Q);
            Console.WriteLine();
        }
    }
}
