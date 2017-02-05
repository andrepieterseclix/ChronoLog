using CLog.Common.Logging;
using CLog.Framework.ServiceClients;
using CLog.ServiceClients.Contracts.Users;
using CLog.ServiceClients.Security;
using CLog.Services.Contracts.Users;
using CLog.Services.Models.Users;
using CLog.Services.Models.Users.DataTransfer;
using CLog.UI.Common.Business;
using CLog.UI.Common.Messaging;
using CLog.UI.Common.Messaging.Mediator;
using CLog.UI.Common.Services;
using CLog.UI.Common.ViewModels;
using CLog.UI.Models.Access;
using CLog.UI.UserProfile.Managers;
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

        private readonly IUserManager _userManager;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileViewModel" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="statusService">The status service.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="mouseService">The mouse service.</param>
        /// <param name="userManager">The user service client.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public UserProfileViewModel(ILogger logger, IStatusService statusService, IDialogService dialogService, IMouseService mouseService, IUserManager userManager)
            : base(logger, statusService, dialogService, mouseService)
        {
            if (userManager == null)
                throw new ArgumentNullException(nameof(userManager));

            _userManager = userManager;

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

        /// <summary>
        /// Clears the context of the view model.
        /// </summary>
        public override void ClearContext()
        {
            base.ClearContext();

            _currentUser = null;
            if (ShadowUser != null)
                ShadowUser.PropertyChanged -= ShadowUser_PropertyChanged;
            ShadowUser = null;
        }

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
            ExecuteAsync(principal =>
            {
                BusinessResult<SessionInfo> result = _userManager.UpdateUser(ShadowUser);
                
                if (result.HasErrors)
                {
                    LoggerHelper.Error(Logger, UPDATE_PROFILE_ERROR);
                    result.Errors.ForEach(x => LoggerHelper.Error(Logger, x.ToString()));
                    string combinedMessage = string.Join("\r\n", result.Errors.Select(e => e.ToString()));
                    StatusService.SetStatus(StatusMessageType.Error, UPDATE_PROFILE_ERROR);
                    DialogService.ShowError(combinedMessage, UPDATE_PROFILE_ERROR);

                    return;
                }

                _currentUser.UserName = ShadowUser.UserName;
                _currentUser.Name = ShadowUser.Name;
                _currentUser.Surname = ShadowUser.Surname;
                _currentUser.Email = ShadowUser.Email;

                HasUnsavedChanges = false;

                ClientIdentity identity = principal.Identity;
                identity.UpdateSession(result.Result.RefId, result.Result.SessionKey);

                StatusService.SetStatus(StatusMessageType.Info, UPDATE_PROFILE_SUCCESS);
                DialogService.ShowInfo(UPDATE_PROFILE_SUCCESS);
                LoggerHelper.Info(Logger, UPDATE_PROFILE_SUCCESS);
            });
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
