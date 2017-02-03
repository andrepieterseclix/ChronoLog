using CLog.Common.Logging;
using CLog.Framework.ServiceClients;
using CLog.ServiceClients.Contracts.Access;
using CLog.ServiceClients.Security;
using CLog.Services.Models.Access.DataTransfer;
using CLog.Services.Security.Contracts.Access;
using CLog.UI.Common.Messaging;
using CLog.UI.Common.Messaging.Mediator;
using CLog.UI.Common.Services;
using CLog.UI.Common.ViewModels;
using CLog.UI.Models.Access;
using System;
using System.Reflection;
using System.Threading;
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

        private readonly IAccessClientFactory _accessClientFactory;

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
        /// <param name="accessClientFactory">The access client factory.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public BannerViewModel(ILogger logger, IStatusService statusService, IDialogService dialogService, IMouseService mouseService, IAccessClientFactory accessClientFactory)
            : base(logger, statusService, dialogService, mouseService)
        {
            if (accessClientFactory == null)
                throw new ArgumentNullException(nameof(accessClientFactory));

            _accessClientFactory = accessClientFactory;

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
        /// Initialises the view model.
        /// </summary>
        public override void Initialise()
        {
            SelectedDate = DateTime.Now.Date;
        }

        [MediatorMessageSink(MessagingConstants.USER_LOGGED_IN)]
        private void HandleLogin(User user)
        {
            User = user;
        }

        private void LogoutCommand_Execute(object parameter)
        {
            Execute(principal =>
            {
                using (IServiceClient<IAccessService> client = _accessClientFactory.Create())
                {
                    LogoutRequest request = new LogoutRequest(principal.Identity.UserName, principal.Identity.SessionId, principal.Identity.SessionKey);
                    client.Proxy.Logout(request);

                    Mediator.NotifyColleaguesAsync(MessagingConstants.USER_LOGGED_OUT, principal.Identity);
                    principal.Identity = null;
                }
            });
        }

        #endregion
    }
}
