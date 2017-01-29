using CLog.Framework.Configuration.Bootstrap;
using CLog.Infrastructure.Contracts.Security;
using CLog.Infrastructure.Security;
using Microsoft.Practices.Unity;
using System;
using System.Configuration;

namespace CLog.Host.Configuration.Installers
{
    /// <summary>
    /// Represents the infrastructure module installer.
    /// </summary>
    /// <seealso cref="CLog.Framework.Configuration.Bootstrap.IUnityDependencyInstaller" />
    public sealed class InfrastructureModuleInstaller : IUnityDependencyInstaller
    {
        /// <summary>
        /// Installs the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        public void Install(IUnityContainer container)
        {
            container
                .RegisterType<ILoginTokenHelper>(new InjectionFactory(c =>
                {
                    string securityTokenExpiryMinutes = ConfigurationManager.AppSettings["SecurityTokenExpiryMinutes"];
                    int minutes = int.Parse(securityTokenExpiryMinutes);

                    return new LoginTokenHelper(new TimeSpan(0, minutes, 0));
                }));

            container
                .RegisterType<IPasswordHelper, PasswordHelperSha256>(new HierarchicalLifetimeManager());
        }
    }
}
