using CLog.Common.Logging;
using CLog.UI.Common.ViewModels;
using CLog.UI.Models.Timesheets;
using System;

namespace CLog.UI.CaptureTime.ViewModels
{
    /// <summary>
    /// Represents the Captured Time day view model.
    /// </summary>
    /// <seealso cref="CLog.UI.Common.ViewModels.ViewModelBase" />
    public sealed class CaptureTimeDayViewModel : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CaptureTimeDayViewModel" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="model">The model.</param>
        public CaptureTimeDayViewModel(ILogger logger, CaptureTimeDay model)
            : base(logger)
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
