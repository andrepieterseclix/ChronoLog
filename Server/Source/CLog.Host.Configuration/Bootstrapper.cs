using CLog.Framework.Applications.Hosting;
using CLog.Framework.Configuration.Bootstrap;
using CLog.Services.Access;
using System;
using System.Linq;
using Unity.Wcf;

namespace CLog.Host.Configuration
{
    /// <summary>
    /// Represents the bootstrapper for the Service Host.
    /// </summary>
    /// <seealso cref="CLog.Framework.Configuration.Bootstrap.UnityBootstrapper" />
    public sealed class Bootstrapper : UnityBootstrapper
    {
        protected override void PostRegistration()
        {
            // ADD SERVICES TO HOST HERE
            Type[] serviceTypes = new[]
            {
                typeof(AccessService)
            };

            // Host the services
            WcfHostBase hostApp = new WcfHostBase(serviceTypes.Select(t => new UnityServiceHost(Container, t)));
            hostApp.Start();
            Console.WriteLine("\r\nPress 'Return' to quit...\r\n");
            Console.ReadLine();
            hostApp.Stop();
        }
    }
}
