using CLog.Common.Logging;
using CLog.ServiceClients.Security;
using CLog.UI.Common.Messaging;
using CLog.UI.Common.Messaging.Mediator;
using CLog.UI.Common.Messaging.Models;
using CLog.UI.Common.Services;
using CLog.UI.Common.ViewModels;
using CLog.UI.Main.Controllers;
using System;
using System.Collections.Generic;
using System.Windows;

namespace CLog.UI.Main.ViewModels
{
    /// <summary>
    /// Represents the view model for the shell (main window) of the application.
    /// </summary>
    /// <seealso cref="CLog.UI.Common.ViewModels.ViewModelBase" />
    public sealed class ShellViewModel : ViewModelBase
    {
        #region Fields

        private string _status;

        private StatusMessageType _statusType;

        private readonly ILoginController _loginController;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="statusService">The status service.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="mouseService">The mouse service.</param>
        /// <param name="loginController">The login controller.</param>
        /// <param name="bannerViewModel">The banner view model.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public ShellViewModel(ILogger logger, IStatusService statusService, IDialogService dialogService, IMouseService mouseService, ILoginController loginController, BannerViewModel bannerViewModel)
            : base(logger, statusService, dialogService, mouseService)
        {
            if (loginController == null)
                throw new ArgumentNullException(nameof(loginController));

            _loginController = loginController;

            Banner = bannerViewModel;
            TabViewModels = new List<ViewModelBase>();

            SetStatusMessage("Shell Created...", StatusMessageType.Warning);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        /// <summary>
        /// Gets or sets the type of the status.
        /// </summary>
        /// <value>
        /// The type of the status.
        /// </value>
        public StatusMessageType StatusType
        {
            get { return _statusType; }
            set { SetProperty(ref _statusType, value); }
        }

        /// <summary>
        /// Gets the banner view model.
        /// </summary>
        /// <value>
        /// The banner.
        /// </value>
        public BannerViewModel Banner { get; private set; }

        /// <summary>
        /// Gets the tab view models.
        /// </summary>
        /// <value>
        /// The tab view models.
        /// </value>
        public List<ViewModelBase> TabViewModels { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initialises the view model.
        /// </summary>
        public override void Initialise()
        {
            Invoke(Login);

            Banner.Initialise();
        }

        [MediatorMessageSink(MessagingConstants.USER_LOGGED_OUT)]
        public void HandleLogout(ClientIdentity user)
        {
            foreach (ViewModelBase viewModel in TabViewModels)
                viewModel.ClearContext();

            Invoke(() =>
            {
                DialogService.SetMainWindowVisible(false);
                Login();
                DialogService.SetMainWindowVisible(true);
            });
        }

        /// <summary>
        /// Updates the status bar with the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        [MediatorMessageSink(MessagingConstants.STATUS_UPDATE)]
        public void SetStatusMessage(StatusMessage message)
        {
            if (message == null)
                return;

            SetStatusMessage(message.Message, message.MessageType);
        }

        private void SetStatusMessage(string message, StatusMessageType messageType)
        {
            Status = message;
            StatusType = messageType;
        }

        private void Login()
        {
            bool loggedIn = _loginController.Login();
            if (!loggedIn)
                Application.Current.Shutdown();

            SetStatusMessage("Logged in", StatusMessageType.Info);
        }

        #endregion
    }
}
