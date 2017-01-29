using CLog.UI.CaptureTime.ViewModels;
using CLog.UI.CaptureTime.Views;
using CLog.UI.Common.Modules;

namespace CLog.UI.CaptureTime
{
    /// <summary>
    /// Represents the capture time module initialiser.
    /// </summary>
    /// <seealso cref="CLog.UI.Common.Modules.IModuleInitialiser" />
    [ModuleInitialiserOrder(-1)]
    public class CaptureTimeModuleInitialiser : IModuleInitialiser
    {
        #region Methods

        /// <summary>
        /// Initialises and returns a module.
        /// </summary>
        /// <param name="container"></param>
        /// <returns>
        /// The initialised module.
        /// </returns>
        public Module Initialise(IDependencyContainer container)
        {
            container.Register<CaptureTimeViewModel>();
            CaptureTimeViewModel viewModel = container.Resolve<CaptureTimeViewModel>();

            return new Module("Capture Time", new CaptureTimeView(), viewModel);
        }

        #endregion
    }
}
