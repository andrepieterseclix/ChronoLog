using CLog.UI.Common.Modules;
using CLog.UI.UserProfile.Managers;
using CLog.UI.UserProfile.ViewModels;
using CLog.UI.UserProfile.Views;

namespace CLog.UI.UserProfile
{
    /// <summary>
    /// Represents the user profile module initialiser.
    /// </summary>
    /// <seealso cref="CLog.UI.Common.Modules.IModuleInitialiser" />
    [ModuleInitialiserOrder(100)]
    public sealed class UserProfileModuleInitialiser : IModuleInitialiser
    {
        /// <summary>
        /// Initialises and returns a module.
        /// </summary>
        /// <param name="container"></param>
        /// <returns>
        /// The initialised module.
        /// </returns>
        public Module Initialise(IDependencyContainer container)
        {
            container.Register<IUserManager, UserManager>();
            container.Register<UserProfileViewModel>();

            UserProfileViewModel viewModel = container.Resolve<UserProfileViewModel>();

            return new Module("User Profile", new UserProfileView(), viewModel);
        }
    }
}
