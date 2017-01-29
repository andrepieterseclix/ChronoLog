using CLog.Framework.Services.Wcf.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace CLog.Framework.Services.Wcf.Hosting
{
    /// <summary>
    /// Represents the WCF Host base class.
    /// </summary>
    public class WcfHostBase
    {
        #region Fields

        private readonly List<ServiceHost> _hosts;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfHostBase" /> class.
        /// </summary>
        /// <param name="hosts">The hosts.</param>
        /// <exception cref="System.ArgumentException">No service hosts was specified!</exception>
        public WcfHostBase(IEnumerable<ServiceHost> hosts)
        {
            _hosts = hosts?.ToList() ?? new List<ServiceHost>();

            if (_hosts.Count < 1)
                throw new ArgumentException("No service hosts was specified!", nameof(hosts));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            foreach (ServiceHost host in _hosts)
            {
                host.Description.Behaviors.Add(new ServerSecurityInterceptorBehavior());
                host.Open();
                Console.WriteLine("Started '{0}'...", host.Description.ServiceType.Name);
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            foreach (ServiceHost host in _hosts)
            {
                using (host)
                {
                    if (host.State == CommunicationState.Opened)
                    {
                        host.Close();
                        Console.WriteLine("Stopped '{0}'...", host.Description.ServiceType.Name);
                    }
                }
            }
        }

        #endregion
    }
}
