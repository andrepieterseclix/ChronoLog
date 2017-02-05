using CLog.UI.Common.Messaging;
using CLog.UI.Common.Messaging.Mediator;
using CLog.UI.Common.Services;
using CLog.UI.Common.ViewModels;
using CLog.UI.Models.Timesheets;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace CLog.UI.CaptureTime.ViewModels
{
    /// <summary>
    /// Represents the Captured Time day view model.
    /// </summary>
    /// <seealso cref="CLog.UI.Common.ViewModels.ViewModelBase" />
    public sealed class CaptureTimeDayViewModel : BasicViewModelBase
    {
        #region Fields

        private bool _isModelValid;

        private IStatusService _statusService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CaptureTimeDayViewModel" /> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="statusService">The status service.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public CaptureTimeDayViewModel(CaptureTimeDay model, IStatusService statusService)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (statusService == null)
                throw new ArgumentNullException(nameof(statusService));

            Model = model;
            ValidationErrors = new ObservableCollection<string>();
            ValidationErrors.CollectionChanged += ValidationErrors_CollectionChanged;
            IsModelValid = ValidationErrors.Count == 0;
            _statusService = statusService;

            Model.PropertyChanged += Model_PropertyChanged;
        }

        private void ValidationErrors_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IsModelValid = ValidationErrors.Count == 0;

            if (!IsModelValid)
            {
                string message = string.Join("  ", ValidationErrors);
                _statusService.SetStatus(StatusMessageType.Error, message);
            }
            else
            {
                // Ensure the hours are updated!
                Mediator.Instance.NotifyColleagues(MessagingConstants.HOURS_CHANGED, this);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public CaptureTimeDay Model { get; private set; }

        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        /// <value>
        /// The validation errors.
        /// </value>
        public ObservableCollection<string> ValidationErrors { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this model is valid.
        /// </summary>
        /// <value>
        /// <c>true</c> if this model is valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsModelValid
        {
            get { return _isModelValid; }
            set { SetProperty(ref _isModelValid, value); }
        }

        #endregion

        #region Event Handlers

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CaptureTimeDay.Hours))
            {
                Mediator.Instance.NotifyColleagues(MessagingConstants.HOURS_CHANGED, this);
            }
        }

        #endregion
    }
}
