using CLog.Framework.Services.Wcf.Hosting;
using CLog.Framework.Configuration.Bootstrap;
using CLog.Services.Access;
using CLog.Services.Timesheets;
using CLog.Services.Users;
using System;
using System.Linq;
using Unity.Wcf;
using CLog.Framework.Configuration.Helpers;

namespace ChronoLog.Host.Configuration
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
                typeof(AccessService),
                typeof(TimesheetService),
                typeof(UserService)
            };

            // Host the services
            WcfHostBase hostApp = new WcfHostBase(serviceTypes.Select(t => new UnityServiceHost(Container, t)));
            hostApp.Start();

            Console.WriteLine();
            ConsoleHelper.LoopUntilKeyPressed(ConsoleKey.Q);
            Console.WriteLine();

            hostApp.Stop();
        }
    }
}
