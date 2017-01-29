namespace CLog.Framework.Configuration.Bootstrap
{
    /// <summary>
    /// Represents the interface for the bootstrapper.
    /// </summary>
    public interface IBootstrapper
    {
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>The instance registered for this base object or interface.</returns>
        T GetInstance<T>();
        
        /// <summary>
        /// Runs the bootstrapping process.
        /// </summary>
        void Run();
    }
}
