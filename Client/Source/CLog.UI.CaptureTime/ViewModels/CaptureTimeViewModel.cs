using CLog.Common.Logging;
using CLog.Framework.ServiceClients;
using CLog.ServiceClients.Contracts.Timesheets;
using CLog.Services.Contracts.Timesheets;
using CLog.Services.Models.Timesheets;
using CLog.Services.Models.Timesheets.DataTransfer;
using CLog.UI.CaptureTime.Extensions;
using CLog.UI.Common.Messaging;
using CLog.UI.Common.Messaging.Mediator;
using CLog.UI.Common.Services;
using CLog.UI.Common.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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

        private readonly IStatusService _statusService;

        private readonly ITimesheetClientFactory _timesheetClientFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CaptureTimeViewModel" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="timesheetClientFactory">The timesheet client factory.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public CaptureTimeViewModel(ILogger logger, IStatusService statusService, ITimesheetClientFactory timesheetClientFactory)
            : base(logger)
        {
            if (statusService == null)
                throw new ArgumentNullException(nameof(statusService));

            if (timesheetClientFactory == null)
                throw new ArgumentNullException(nameof(timesheetClientFactory));

            _statusService = statusService;
            _timesheetClientFactory = timesheetClientFactory;

            Days = new ObservableCollection<CaptureTimeDayViewModel>();

            SaveCommand = CreateCommand(SaveCommand_Execute);
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

        [MediatorMessageSink(MessagingConstants.DATE_CHANGED)]
        private void SelectedDateChanged(DateTime selectedDate)
        {
            bool sameWeekSelected = false;

            Invoke(() =>
            {
                SelectedDate = selectedDate;
                sameWeekSelected = (SelectedDate >= FromDate && SelectedDate <= ToDate);
            });

            if (sameWeekSelected && Days.Count == DAYS_IN_WEEK)
                return;

            // Get data from server
            Task.Factory.StartNew(() =>
            {
                try
                {
                    Invoke(Days.Clear);

                    DateTime fromDate = selectedDate;

                    while (fromDate.DayOfWeek != WEEK_START_DAY)
                        fromDate = fromDate.AddDays(-1d);

                    DateTime toDate = fromDate.AddDays(DAYS_IN_WEEK - 1);

                    Invoke(() =>
                    {
                        FromDate = fromDate;
                        ToDate = toDate;
                    });

                    GetCapturedTimeResponse response = null;

                    using (IServiceClient<ITimesheetService> client = _timesheetClientFactory.Create())
                    {
                        GetCapturedTimeRequest request = new GetCapturedTimeRequest(fromDate, toDate);
                        response = client.Proxy.GetCapturedTime(request);
                    }

                    // Map
                    foreach (CapturedTimeDto item in response.CapturedTimeItems)
                    {
                        CaptureTimeDayViewModel itemViewModel = item.Map(Logger);
                        if (itemViewModel != null)
                            Invoke(() => Days.Add(itemViewModel));
                    }
                }
                catch (Exception ex)
                {
                    // TODO:  improve exception handling
                    _statusService.SetStatus(StatusMessageType.Error, "An unexpected error occurred, please check the logs.");
                    LoggerHelper.Exception(Logger, ex, nameof(SelectedDateChanged));
                }
            });
        }

        private void SaveCommand_Execute(object parameter)
        {
            _statusService.SetStatus(StatusMessageType.Warning, "Save has not yet been implemented!");
            throw new NotImplementedException();
        }

        #endregion
    }
}
