using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Threading;

namespace CLog.Framework.Configuration.Bootstrap
{
    /// <summary>
    /// Represents the Unity Bootstrapper base class.
    /// </summary>
    public abstract class UnityBootstrapper : IBootstrapper
    {
        #region Fields

        private bool _ran;

        private ManualResetEvent _waitToClose = new ManualResetEvent(false);

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityBootstrapper"/> class.
        /// </summary>
        public UnityBootstrapper()
        {
            Container = new UnityContainer();

            // Configure the service locator pattern.  Use this pattern in rare cases only!
            UnityServiceLocator serviceLocator = new UnityServiceLocator(Container);
            ServiceLocator.SetLocatorProvider(() => serviceLocator);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        public IUnityContainer Container { get; private set; }
        
        #endregion

        #region Methods

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>The resolved instance.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public T GetInstance<T>()
        {
            if (!_ran)
                throw new InvalidOperationException($"Call the {nameof(Run)} method first!");

            return Container.Resolve<T>();
        }

        /// <summary>
        /// Runs the bootstrapping process.
        /// </summary>
        public void Run()
        {
            ConfigureContainer();
            DoRegistration();
            PostRegistration();
            _ran = true;

            // Signal caller that the bootstrapper has stopped.
            _waitToClose.Set();
        }

        /// <summary>
        /// Blocks the calling thread until the bootstrapper has stopped.
        /// </summary>
        public void Wait()
        {
            _waitToClose.WaitOne();
        }

        /// <summary>
        /// Configures the container if overridden in a derived class.
        /// </summary>
        /// <remarks>The base implementation is empty.</remarks>
        protected virtual void ConfigureContainer()
        {
        }

        /// <summary>
        /// The assembly containing the bootstrapper implementation will be scanned for non-abstract classes that implement <see cref="IUnityDependencyInstaller"/>.
        /// These classes will be instantiated and used to initialise the modules that they represent.
        /// </summary>
        protected virtual void DoRegistration()
        {
            GetType().Assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(IUnityDependencyInstaller).IsAssignableFrom(t))
                .Select(t => Activator.CreateInstance(t))
                .OfType<IUnityDependencyInstaller>()
                .ToList()
                .ForEach(i => i.Install(Container));
        }

        /// <summary>
        /// Runs after <see cref="DoRegistration"/> to finish the bootstrapping process.
        /// </summary>
        /// <remarks>The base implementation is empty.</remarks>
        protected virtual void PostRegistration()
        {
        }

        #endregion
    }
}
