namespace CLog.UI.Common.Modules
{
    /// <summary>
    /// Represents the module initialiser contract.
    /// </summary>
    public interface IModuleInitialiser
    {
        /// <summary>
        /// Initialises and returns a module.
        /// </summary>
        /// <returns>The initialised module.</returns>
        Module Initialise(IDependencyContainer container);
    }
}
