using CLog.Framework.Configuration.Bootstrap;
using CLog.UI.Common.Modules;
using CLog.UI.Main.Managers;
using CLog.UI.Main.ViewModels;
using CLog.UI.Main.Views;
using Microsoft.Practices.Unity;

namespace ChronoLog.Configuration
{
    /// <summary>
    /// Represents the bootstrapper for the WPF front-end application.
    /// </summary>
    /// <seealso cref="ChronoLog.Framework.Configuration.Bootstrap.UnityBootstrapper" />
    public sealed class Bootstrapper : UnityBootstrapper, IDependencyContainer
    {
        /// <summary>
        /// Registers the type with the container.
        /// </summary>
        /// <typeparam name="T">The type to register</typeparam>
        public void Register<T>()
        {
            Container.RegisterType<T>();
        }

        /// <summary>
        /// Registers this instance.
        /// </summary>
        /// <typeparam name="T">The contract.</typeparam>
        /// <typeparam name="U">The implementation.</typeparam>
        public void Register<T, U>()
            where U : T
        {
            Container.RegisterType<T, U>();
        }

        /// <summary>
        /// Resolves the type from the container.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>
        /// The resolved instance.
        /// </returns>
        public T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        /// <summary>
        /// The assembly containing the bootstrapper implementation will be scanned for non-abstract classes that implement <see cref="T:CLog.Framework.Configuration.Bootstrap.IUnityDependencyInstaller" />.
        /// These classes will be instantiated and used to initialise the modules that they represent.
        /// </summary>
        protected override void DoRegistration()
        {
            base.DoRegistration();

            // Views
            Container.RegisterType<ShellWindow>();

            // Business
            Container.RegisterType<IAccessManager, AccessManager>();

            // View Models
            Container.RegisterType<BannerViewModel>();
            Container.RegisterType<ShellViewModel>();
        }
    }
}
