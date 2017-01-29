using CLog.Framework.Configuration.Bootstrap;
using CLog.UI.CaptureTime;
using CLog.UI.Common.Modules;
using CLog.UI.Main;
using CLog.UI.UserProfile;
using Microsoft.Practices.Unity;

namespace ChronoLog.Configuration.Installers
{
    /// <summary>
    /// Represents the UI module installer.
    /// </summary>
    /// <seealso cref="CLog.Framework.Configuration.Bootstrap.IUnityDependencyInstaller" />
    public sealed class ModuleInstaller : IUnityDependencyInstaller
    {
        /// <summary>
        /// Installs the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        public void Install(IUnityContainer container)
        {
            container
                .RegisterType<IModuleInitialiser, CaptureTimeModuleInitialiser>(typeof(CaptureTimeModuleInitialiser).FullName);

            container
                .RegisterType<IModuleInitialiser, UserProfileModuleInitialiser>(typeof(UserProfileModuleInitialiser).FullName);

            // Composite module will 'suck up' all other named modules
            container
                .RegisterType<IModuleInitialiser, CompositeModule>();
        }
    }
}
