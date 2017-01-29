using CLog.Common.Logging;
using CLog.Framework.ServiceClients;
using CLog.ServiceClients.Contracts.Users;
using CLog.ServiceClients.Security;
using CLog.Services.Contracts.Users;
using CLog.Services.Models.Users;
using CLog.Services.Models.Users.DataTransfer;
using CLog.UI.Common.Messaging;
using CLog.UI.Common.Messaging.Mediator;
using CLog.UI.Common.Services;
using CLog.UI.Common.ViewModels;
using CLog.UI.Models.Access;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Input;

namespace CLog.UI.UserProfile.ViewModels
{
    /// <summary>
    /// Represents the user profile view model.
    /// </summary>
    /// <seealso cref="CLog.UI.Common.ViewModels.ViewModelBase" />
    public class UserProfileViewModel : ViewModelBase
    {
        #region Fields

        private readonly string UPDATE_PROFILE_ERROR = "Could not save your profile.";

        private readonly string UPDATE_PROFILE_SUCCESS = "Your profile has been saved.";

        private bool _hasUnsavedChanges;

        private User _currentUser;

        private User _shadowUser;

        private readonly IUserClientFactory _userServiceClient;

        private readonly IDialogService _dialogService;

        private readonly IStatusService _statusService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileViewModel" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="statusService">The status service.</param>
        /// <param name="userServiceClient">The user service client.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public UserProfileViewModel(ILogger logger, IDialogService dialogService, IStatusService statusService, IUserClientFactory userServiceClient)
            : base(logger)
        {
            if (userServiceClient == null)
                throw new ArgumentNullException(nameof(userServiceClient));

            if (dialogService == null)
                throw new ArgumentNullException(nameof(dialogService));

            if (statusService == null)
                throw new ArgumentNullException(nameof(statusService));

            _userServiceClient = userServiceClient;
            _dialogService = dialogService;
            _statusService = statusService;

            SaveCommand = CreateCommand(SaveCommand_Execute, SaveCommand_CanExecute);
            ResetCommand = CreateCommand(ResetCommand_Execute, ResetCommand_CanExecute);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the save command.
        /// </summary>
        /// <value>
        /// The save command.
        /// </value>
        public ICommand SaveCommand { get; private set; }

        public ICommand ResetCommand { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has unsaved changes.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has unsaved changes; otherwise, <c>false</c>.
        /// </value>
        public bool HasUnsavedChanges
        {
            get { return _hasUnsavedChanges; }
            set { SetProperty(ref _hasUnsavedChanges, value); }
        }

        /// <summary>
        /// Gets or sets the shadow user.
        /// </summary>
        /// <value>
        /// The shadow user.
        /// </value>
        public User ShadowUser
        {
            get { return _shadowUser; }
            set { SetProperty(ref _shadowUser, value); }
        }

        #endregion

        #region Methods

        [MediatorMessageSink(MessagingConstants.USER_LOGGED_IN)]
        private void HandleLogin(User user)
        {
            _currentUser = user;

            if (user == null)
                return;

            if (ShadowUser != null)
                ShadowUser.PropertyChanged -= ShadowUser_PropertyChanged;
            ShadowUser = new User(user.UserName, user.Name, user.Surname, user.Email);
            ShadowUser.PropertyChanged += ShadowUser_PropertyChanged;
        }

        private void ShadowUser_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_currentUser == null)
                return;

            HasUnsavedChanges =
                ShadowUser.UserName != _currentUser.UserName ||
                ShadowUser.Name != _currentUser.Name ||
                ShadowUser.Surname != _currentUser.Surname ||
                ShadowUser.Email != _currentUser.Email;
        }

        private void SaveCommand_Execute(object parameter)
        {
            try
            {
                using (IServiceClient<IUserService> client = _userServiceClient.Create())
                {
                    // TODO:  refactor this into base class
                    ClientPrincipal principal = Thread.CurrentPrincipal as ClientPrincipal;
                    if (principal == null)
                        throw new ApplicationException("The application's thread principal has not been configured correctly.");

                    ClientIdentity identity = principal.Identity;
                    UserDetailsDto userDetails = new UserDetailsDto(ShadowUser.UserName, ShadowUser.Name, ShadowUser.Surname, ShadowUser.Email);
                    UpdateUserRequest request = new UpdateUserRequest(userDetails);
                    UpdateUserResponse response = client.Proxy.UpdateUser(request);

                    if (response.Errors != null && response.Errors.Count > 0)
                    {
                        LoggerHelper.Error(Logger, UPDATE_PROFILE_ERROR);
                        response.Errors.ForEach(e => LoggerHelper.Error(Logger, e.ToString()));
                        string combinedMessage = string.Join("\r\n", response.Errors.Select(e => string.Format(CultureInfo.CurrentCulture, "{0}:  {1}", e.Code, e.Message)));
                        _statusService.SetStatus(StatusMessageType.Error, UPDATE_PROFILE_ERROR);
                        _dialogService.ShowError(combinedMessage, UPDATE_PROFILE_ERROR);

                        return;
                    }

                    _currentUser.UserName = ShadowUser.UserName;
                    _currentUser.Name = ShadowUser.Name;
                    _currentUser.Surname = ShadowUser.Surname;
                    _currentUser.Email = ShadowUser.Email;

                    HasUnsavedChanges = false;

                    identity.UpdateSession(response.Session.Id, response.Session.SessionKey);

                    _statusService.SetStatus(StatusMessageType.Info, UPDATE_PROFILE_SUCCESS);
                    _dialogService.ShowInfo(UPDATE_PROFILE_SUCCESS);
                    LoggerHelper.Info(Logger, UPDATE_PROFILE_SUCCESS);
                }
            }
            catch (Exception ex)
            {
                // TODO:  improve exception handling
                _statusService.SetStatus(StatusMessageType.Error, "An unexpected error occurred, please check the logs.");
                LoggerHelper.Exception(Logger, ex, nameof(SaveCommand_Execute));
            }
        }

        private bool SaveCommand_CanExecute(object parameter)
        {
            return
                HasUnsavedChanges &&
                string.IsNullOrWhiteSpace(ShadowUser.Error);
        }

        private void ResetCommand_Execute(object parameter)
        {
            ShadowUser.UserName = _currentUser.UserName;
            ShadowUser.Name = _currentUser.Name;
            ShadowUser.Surname = _currentUser.Surname;
            ShadowUser.Email = _currentUser.Email;
        }

        private bool ResetCommand_CanExecute(object parameter)
        {
            return HasUnsavedChanges;
        }

        #endregion
    }
}
