using CLog.Common.Logging;
using CLog.UI.Common.Services;
using CLog.UI.Main.Managers;
using CLog.UI.Main.ViewModels;
using CLog.UI.Main.Views;
using System;

namespace CLog.UI.Main.Controllers
{
    /// <summary>
    /// Represents the Login controller.
    /// </summary>
    /// <seealso cref="CLog.UI.Main.Controllers.ILoginController" />
    public sealed class LoginController : ILoginController
    {
        #region Fields

        private readonly ILogger _logger;

        private readonly IDialogService _dialogService;

        private readonly IStatusService _statusService;

        private readonly IMouseService _mouseService;

        private readonly IAccessManager _accessManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginController" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="statusService">The status service.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="mouseService">The mouse service.</param>
        /// <param name="accessManager">The access manager.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public LoginController(ILogger logger, IStatusService statusService, IDialogService dialogService, IMouseService mouseService, IAccessManager accessManager)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (statusService == null)
                throw new ArgumentNullException(nameof(statusService));

            if (dialogService == null)
                throw new ArgumentNullException(nameof(dialogService));

            if (mouseService == null)
                throw new ArgumentNullException(nameof(mouseService));

            if (accessManager == null)
                throw new ArgumentNullException(nameof(accessManager));

            _dialogService = dialogService;
            _statusService = statusService;
            _mouseService = mouseService;
            _logger = logger;
            _accessManager = accessManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Log in
        /// </summary>
        /// <returns><c>true</c> when the user logged in successfully, otherwise <c>false</c>.</returns>
        public bool Login()
        {
            LoginViewModel viewModel = new LoginViewModel(_logger, _statusService, _dialogService, _mouseService, _accessManager);
            viewModel.Initialise();

            return !_dialogService.ShowDialog<LoginWindow>(viewModel)
                ? false
                : viewModel.IsLoggedIn;
        }

        #endregion
    }
}
