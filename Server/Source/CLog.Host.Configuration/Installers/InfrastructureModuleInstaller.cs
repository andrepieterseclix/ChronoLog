using CLog.Framework.Configuration.Bootstrap;
using CLog.Infrastructure.Contracts.Security;
using CLog.Infrastructure.Security;
using Microsoft.Practices.Unity;
using System;
using System.Configuration;

namespace CLog.Host.Configuration.Installers
{
    class InfrastructureModuleInstaller : IUnityDependencyInstaller
    {
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
