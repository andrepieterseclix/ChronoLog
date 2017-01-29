namespace CLog.UI.Common.Modules
{
    /// <summary>
    /// Represents a basic dependency injection container.
    /// </summary>
    public interface IDependencyContainer
    {
        /// <summary>
        /// Registers the type with the container.
        /// </summary>
        /// <typeparam name="T">The type to register</typeparam>
        void Register<T>();

        /// <summary>
        /// Resolves the type from the container.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>The resolved instance.</returns>
        T Resolve<T>();
    }
}
