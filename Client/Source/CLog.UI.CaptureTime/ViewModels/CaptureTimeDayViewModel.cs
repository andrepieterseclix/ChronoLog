using CLog.UI.Common.ViewModels;
using CLog.UI.Models.Timesheets;
using System;

namespace CLog.UI.CaptureTime.ViewModels
{
    /// <summary>
    /// Represents the Captured Time day view model.
    /// </summary>
    /// <seealso cref="CLog.UI.Common.ViewModels.ViewModelBase" />
    public sealed class CaptureTimeDayViewModel : BasicViewModelBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CaptureTimeDayViewModel" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="statusService">The status service.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="mouseService">The mouse service.</param>
        /// <param name="model">The model.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public CaptureTimeDayViewModel(CaptureTimeDay model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            Model = model;
        }

        #endregion

        #region Properties

        public CaptureTimeDay Model { get; private set; }

        #endregion
    }
}
