using CLog.Common.Logging;
using CLog.Framework.ServiceClients;
using CLog.ServiceClients.Contracts.Access;
using CLog.ServiceClients.Security;
using CLog.Services.Security.Contracts.Access;
using CLog.Services.Models.Access.DataTransfer;
using CLog.UI.Common.Messaging;
using CLog.UI.Common.Services;
using CLog.UI.Common.ViewModels;
using CLog.UI.Models.Access;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows.Input;

namespace CLog.UI.Main.ViewModels
{
    /// <summary>
    /// Represents the Login view model.
    /// </summary>
    /// <seealso cref="CLog.UI.Common.ViewModels.ViewModelBase" />
    public sealed class LoginViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAccessClientFactory _accessClientFactory;

        private string _message;

        private string _userName;

        private string _password;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="statusService">The status service.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="mouseService">The mouse service.</param>
        /// <param name="accessClientFactory">The access client factory.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public LoginViewModel(ILogger logger, IStatusService statusService, IDialogService dialogService, IMouseService mouseService, IAccessClientFactory accessClientFactory)
            : base(logger, statusService, dialogService, mouseService)
        {
            if (accessClientFactory == null)
                throw new ArgumentNullException(nameof(accessClientFactory));

            _accessClientFactory = accessClientFactory;
            LoginCommand = CreateCommand(LoginCommand_Execute, LoginCommand_CanExecute);

            PropertyChanged += LoginViewModel_PropertyChanged;

#if DEBUG
            UserName = "Tester";
            Password = "P@ssw0rd";
#endif
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the login command.
        /// </summary>
        /// <value>
        /// The login command.
        /// </value>
        public ICommand LoginCommand { get; private set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is logged in.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is logged in; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoggedIn { get; internal set; }

        #endregion

        #region Methods

        private void LoginViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UserName) || e.PropertyName == nameof(Password))
                Message = null;
        }

        private void LoginCommand_Execute(object parameter)
        {
            ExecuteAnonymous(() =>
            {
                try
                {
                    using (IServiceClient<IAccessService> client = _accessClientFactory.Create())
                    {
                        LoginRequest request = new LoginRequest(UserName, Password);
                        LoginResponse response = client.Proxy.Login(request);

                        IsLoggedIn = response.IsLoggedIn;

                        if (IsLoggedIn)
                        {
                            User user = new User(response.User.UserName, response.User.Name, response.User.Surname, response.User.Email);
                            Mediator.NotifyColleagues(MessagingConstants.USER_LOGGED_IN, user);

                            // Ensure the principal is read from the UI thread
                            Invoke(() =>
                            {
                                ClientPrincipal principal = Thread.CurrentPrincipal as ClientPrincipal;
                                if (principal == null)
                                    throw new ApplicationException("The application's thread principal has not been configured correctly.");

                                principal.Identity = new ClientIdentity(
                                    user.UserName,
                                    string.Format(CultureInfo.CurrentCulture, "{0} {1}", user.Name, user.Surname),
                                    response.Session.Id,
                                    response.Session.SessionKey,
                                    new string[0]);
                            });

                            DialogService.SetDialogResult(true);
                            return;
                        }

                        StringBuilder buffer = new StringBuilder();
                        foreach (var error in response.Errors)
                        {
                            buffer.AppendLine(error.Message);
                        }

                        Message = buffer.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Message = "An error occurred, please try again later.";
                    LoggerHelper.Exception(Logger, ex, Message);
                }
            });
        }

        private bool LoginCommand_CanExecute(object parameter)
        {
            return
                !string.IsNullOrWhiteSpace(UserName) &&
                !string.IsNullOrWhiteSpace(Password);
        }

        #endregion
    }
}
