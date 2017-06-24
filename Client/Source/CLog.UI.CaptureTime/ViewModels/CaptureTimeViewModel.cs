using CLog.Common.Logging;
using CLog.UI.CaptureTime.Managers;
using CLog.UI.Common.Business;
using CLog.UI.Common.Messaging;
using CLog.UI.Common.Messaging.Mediator;
using CLog.UI.Common.Services;
using CLog.UI.Common.ViewModels;
using CLog.UI.Models;
using CLog.UI.Models.Timesheets;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace CLog.UI.CaptureTime.ViewModels
{
    public class CaptureTimeViewModel : ViewModelBase
    {
        #region Fields

        private const DayOfWeek WEEK_START_DAY = DayOfWeek.Monday;

        private const int DAYS_IN_WEEK = 7;

        private DateTime _selectedDate;

        private DateTime _fromDate;

        private DateTime _toDate;

        private string _description;

        private int _totalHours;

        private readonly ITimesheetManager _timesheetManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CaptureTimeViewModel" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="statusService">The status service.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="mouseService">The mouse service.</param>
        /// <param name="timesheetManager">The timesheet manager.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public CaptureTimeViewModel(ILogger logger, IStatusService statusService, IDialogService dialogService, IMouseService mouseService, ITimesheetManager timesheetManager)
            : base(logger, statusService, dialogService, mouseService)
        {
            if (timesheetManager == null)
                throw new ArgumentNullException(nameof(timesheetManager));

            _timesheetManager = timesheetManager;

            Days = new ObservableCollection<CaptureTimeDayViewModel>();

            SaveCommand = CreateCommand(SaveCommand_Execute, SaveCommand_CanExecute);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the selected date.
        /// </summary>
        /// <value>
        /// The selected date.
        /// </value>
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { SetProperty(ref _selectedDate, value); }
        }

        /// <summary>
        /// Gets or sets from date.
        /// </summary>
        /// <value>
        /// From date.
        /// </value>
        public DateTime FromDate
        {
            get { return _fromDate; }
            set { SetProperty(ref _fromDate, value); }
        }

        /// <summary>
        /// Gets or sets to date.
        /// </summary>
        /// <value>
        /// To date.
        /// </value>
        public DateTime ToDate
        {
            get { return _toDate; }
            set { SetProperty(ref _toDate, value); }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        /// <summary>
        /// Gets or sets the total hours.
        /// </summary>
        /// <value>
        /// The total hours.
        /// </value>
        public int TotalHours
        {
            get { return _totalHours; }
            set { SetProperty(ref _totalHours, value); }
        }

        /// <summary>
        /// Gets the days.
        /// </summary>
        /// <value>
        /// The days.
        /// </value>
        public ObservableCollection<CaptureTimeDayViewModel> Days { get; private set; }

        /// <summary>
        /// Gets the save command.
        /// </summary>
        /// <value>
        /// The save command.
        /// </value>
        public ICommand SaveCommand { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Clears the context of the view model.
        /// </summary>
        public override void ClearContext()
        {
            base.ClearContext();

            Invoke(Days.Clear);
        }

        [MediatorMessageSink(MessagingConstants.DATE_CHANGED)]
        private void SelectedDateChanged(DateTime selectedDate)
        {
            if (selectedDate == DateTime.MinValue)
                return;

            bool sameWeekSelected = false;

            Invoke(() =>
            {
                SelectedDate = selectedDate;
                sameWeekSelected = (SelectedDate >= FromDate && SelectedDate <= ToDate);
            });

            if (sameWeekSelected && Days.Count == DAYS_IN_WEEK)
            {
                StatusService.SetStatus(StatusMessageType.Info, "Selected {0} (same week)", SelectedDate.ToString(ModelConstants.DATE_FORMAT));
                return;
            }

            ExecuteAsync(principal =>
            {
                ClearContext();

                DateTime fromDate = selectedDate;

                while (fromDate.DayOfWeek != WEEK_START_DAY)
                    fromDate = fromDate.AddDays(-1d);

                DateTime toDate = fromDate.AddDays(DAYS_IN_WEEK - 1);

                Invoke(() =>
                {
                    FromDate = fromDate;
                    ToDate = toDate;
                });

                UIBusinessResult<CaptureTimeDay[]> result =
                    _timesheetManager.GetCapturedTime(fromDate, toDate);

                Invoke(() =>
                {
                    foreach (CaptureTimeDay model in result.Result)
                        Days.Add(new CaptureTimeDayViewModel(model, StatusService));
                });

                UpdateTotalHours();

                StatusService.SetStatus(StatusMessageType.Info, "Selected {0}", SelectedDate.ToString(ModelConstants.DATE_FORMAT));
            });
        }

        [MediatorMessageSink(MessagingConstants.HOURS_CHANGED)]
        private void UpdateTotalHours(CaptureTimeDayViewModel viewModel = null)
        {
            if (viewModel != null)
                StatusService.SetStatus(StatusMessageType.Warning, "Captured {0} hours for {1}", viewModel.Model.Hours, viewModel.Model.Date.ToString(ModelConstants.DATE_FORMAT));

            int totalHours = 0;

            foreach (CaptureTimeDayViewModel captureTimeViewModel in Days)
                totalHours += captureTimeViewModel.Model.Hours;

            TotalHours = totalHours;
        }

        private void SaveCommand_Execute(object parameter)
        {
            if (!SaveCommand_CanExecute(parameter))
                return;

            ExecuteAsync(principal =>
            {
                CaptureTimeDay[] capturedTimeDays =
                    Days.Select(x => x.Model).ToArray();

                UIBusinessResult result =
                    _timesheetManager.SaveCapturedTime(capturedTimeDays, principal.Identity.UserName);

                foreach (ErrorMessage errorMessage in result.Errors)
                    LoggerHelper.Error(Logger, "{0}", errorMessage);

                if (result.HasErrors)
                {
                    if (result.Errors.Count > 1)
                        StatusService.SetStatus(StatusMessageType.Error, "{0}  See the logs for additional errors that have occurred.", result.Errors.First());
                    else
                        StatusService.SetStatus(StatusMessageType.Error, "{0}", result.Errors.First());
                }
                else
                {
                    StatusService.SetStatus(StatusMessageType.Info, "Your time has been saved.");
                }
            });
        }

        private bool SaveCommand_CanExecute(object parameter)
        {
            return Days?.All(d => d.IsModelValid) ?? false;
        }

        #endregion
    }
}
