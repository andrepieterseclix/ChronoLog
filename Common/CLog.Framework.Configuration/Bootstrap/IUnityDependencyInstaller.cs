using Microsoft.Practices.Unity;

namespace CLog.Framework.Configuration.Bootstrap
{
    /// <summary>
    /// Represents the Unity Dependency Installer contract.
    /// </summary>
    public interface IUnityDependencyInstaller
    {
        /// <summary>
        /// Installs dependencies in the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        void Install(IUnityContainer container);
    }
}
