using System.Threading;

namespace CLog.Framework.Configuration.Bootstrap.TerminationStrategy
{
    public sealed class ManualTerminationStrategy : ITerminationStrategy
    {
        ManualResetEvent blocker = new ManualResetEvent(false);

        /// <summary>
        /// Terminates this instance.
        /// </summary>
        public void Terminate()
        {
            blocker.Set();
        }

        /// <summary>
        /// Runs until the <see cref="Terminate"/> method is called.
        /// </summary>
        public void Run()
        {
            blocker.WaitOne();
        }
    }
}
