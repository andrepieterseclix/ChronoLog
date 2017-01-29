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
    public sealed class LoginViewModel : ViewModelBase
    {
        #region Fields

        private readonly IDialogService _dialogService;

        private readonly IMouseService _mouseService;

        private readonly IAccessClientFactory _accessClientFactory;

        private string _message;

        private string _userName;

        private string _password;

        #endregion

        #region Constructors

        public LoginViewModel(ILogger logger, IMouseService mouseService, IDialogService dialogService, IAccessClientFactory accessClientFactory)
            : base(logger)
        {
            if (dialogService == null)
                throw new ArgumentNullException(nameof(dialogService));

            if (mouseService == null)
                throw new ArgumentNullException(nameof(mouseService));

            if (accessClientFactory == null)
                throw new ArgumentNullException(nameof(accessClientFactory));

            _dialogService = dialogService;
            _mouseService = mouseService;
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

        public ICommand LoginCommand { get; private set; }

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

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
            try
            {
                _mouseService.SetWait(true);

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

                        _dialogService.SetDialogResult(true);
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
            finally
            {
                _mouseService.SetWait(false);
            }
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
