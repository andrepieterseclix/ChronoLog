using CLog.Common.Logging;
using CLog.UI.Common.Business;
using CLog.UI.Common.Messaging;
using CLog.UI.Common.Messaging.Mediator;
using CLog.UI.Common.Services;
using CLog.UI.Common.ViewModels;
using CLog.UI.Main.Managers;
using CLog.UI.Models.Access;
using System;
using System.Reflection;
using System.Windows.Input;

namespace CLog.UI.Main.ViewModels
{
    /// <summary>
    /// Represents the banner view model.
    /// </summary>
    /// <seealso cref="CLog.UI.Common.ViewModels.ViewModelBase" />
    public sealed class BannerViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAccessManager _accessManager;

        private User _user;

        private DateTime _selectedDate;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BannerViewModel" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="statusService">The status service.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="mouseService">The mouse service.</param>
        /// <param name="accessManager">The access manager.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public BannerViewModel(ILogger logger, IStatusService statusService, IDialogService dialogService, IMouseService mouseService, IAccessManager accessManager)
            : base(logger, statusService, dialogService, mouseService)
        {
            if (accessManager == null)
                throw new ArgumentNullException(nameof(accessManager));

            _accessManager = accessManager;

            LogoutCommand = CreateCommand(LogoutCommand_Execute);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the logout command.
        /// </summary>
        /// <value>
        /// The logout command.
        /// </value>
        public ICommand LogoutCommand { get; private set; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version
        {
            get { return Assembly.GetEntryAssembly().GetName().Version.ToString(); }
        }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public User User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        /// <summary>
        /// Gets or sets the selected date.
        /// </summary>
        /// <value>
        /// The selected date.
        /// </value>
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (SetProperty(ref _selectedDate, value))
                    Mediator.NotifyColleaguesAsync(MessagingConstants.DATE_CHANGED, _selectedDate);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clears the context of the view model.
        /// </summary>
        public override void ClearContext()
        {
            base.ClearContext();

            SelectedDate = DateTime.MinValue;
        }

        [MediatorMessageSink(MessagingConstants.USER_LOGGED_IN)]
        private void HandleLogin(User user)
        {
            User = user;
            SelectedDate = DateTime.Now.Date;
        }

        private void LogoutCommand_Execute(object parameter)
        {
            Execute(principal =>
            {
                UIBusinessResult result = _accessManager.Logout(principal);

                Mediator.NotifyColleaguesAsync(MessagingConstants.USER_LOGGED_OUT, principal.Identity);
                principal.Identity = null;
            });
        }

        #endregion
    }
}
