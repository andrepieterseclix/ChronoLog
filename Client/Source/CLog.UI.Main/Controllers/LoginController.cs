using CLog.Common.Logging;
using CLog.ServiceClients.Contracts.Access;
using CLog.UI.Common.Services;
using CLog.UI.Main.ViewModels;
using CLog.UI.Main.Views;
using System;

namespace CLog.UI.Main.Controllers
{
    public sealed class LoginController : ILoginController
    {
        #region Fields

        private readonly ILogger _logger;

        private readonly IDialogService _dialogService;

        private readonly IStatusService _statusService;

        private readonly IMouseService _mouseService;

        private readonly IAccessClientFactory _accessClientFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="statusService">The status service.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="mouseService">The mouse service.</param>
        /// <param name="accessClientFactory">The access client factory.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public LoginController(ILogger logger, IStatusService statusService, IDialogService dialogService, IMouseService mouseService, IAccessClientFactory accessClientFactory)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (statusService == null)
                throw new ArgumentNullException(nameof(statusService));

            if (dialogService == null)
                throw new ArgumentNullException(nameof(dialogService));

            if (mouseService == null)
                throw new ArgumentNullException(nameof(mouseService));

            if (accessClientFactory == null)
                throw new ArgumentNullException(nameof(accessClientFactory));

            _dialogService = dialogService;
            _statusService = statusService;
            _mouseService = mouseService;
            _logger = logger;
            _accessClientFactory = accessClientFactory;
        }

        #endregion

        #region Methods

        public bool Login()
        {
            LoginViewModel viewModel = new LoginViewModel(_logger, _statusService, _dialogService, _mouseService, _accessClientFactory);
            viewModel.Initialise();

            return !_dialogService.ShowDialog<LoginWindow>(viewModel)
                ? false
                : viewModel.IsLoggedIn;
        }

        #endregion
    }
}
