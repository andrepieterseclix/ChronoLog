using CLog.Framework.Configuration.Bootstrap;
using CLog.Framework.Configuration.Bootstrap.TerminationStrategy;
using CLog.Framework.Services.Wcf.Hosting;
using CLog.Services.Access;
using CLog.Services.Timesheets;
using CLog.Services.Users;
using System;
using System.Linq;
using Unity.Wcf;

namespace ChronoLog.Host.Configuration
{
    /// <summary>
    /// Represents the bootstrapper for the Service Host.
    /// </summary>
    /// <seealso cref="CLog.Framework.Configuration.Bootstrap.UnityBootstrapper" />
    public sealed class Bootstrapper : UnityBootstrapper
    {
        #region Fields

        private ITerminationStrategy _terminationStrategy;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
        /// </summary>
        public Bootstrapper()
        {
            TerminationStrategy = new ConsoleTerminationStrategy();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the termination strategy.
        /// </summary>
        /// <value>
        /// The termination strategy.
        /// </value>
        /// <exception cref="System.ArgumentNullException"></exception>
        public ITerminationStrategy TerminationStrategy
        {
            get { return _terminationStrategy; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _terminationStrategy = value;
            }
        }

        #endregion

        #region Methods

        protected override void PostRegistration()
        {
            // ADD SERVICES TO HOST HERE
            Type[] serviceTypes = new[]
            {
                typeof(AccessService),
                typeof(TimesheetService),
                typeof(UserService)
            };

            // Host the services
            WcfHostBase hostApp = new WcfHostBase(serviceTypes.Select(t => new UnityServiceHost(Container, t)));
            hostApp.Start();
            TerminationStrategy.Run();
            hostApp.Stop();
        }

        #endregion
    }
}
