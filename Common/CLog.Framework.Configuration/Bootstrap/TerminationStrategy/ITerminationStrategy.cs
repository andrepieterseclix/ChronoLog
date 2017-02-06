namespace CLog.Framework.Configuration.Bootstrap.TerminationStrategy
{
    /// <summary>
    /// Represents the termination strategy contract.
    /// </summary>
    public interface ITerminationStrategy
    {
        /// <summary>
        /// Runs until the strategy decides to terminate the process.
        /// </summary>
        void Run();
    }
}
