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

        private readonly IMouseService _mouseService;

        private readonly IAccessClientFactory _accessClientFactory;

        #endregion

        #region Constructors

        public LoginController(ILogger logger, IDialogService dialogService, IMouseService mouseService, IAccessClientFactory accessClientFactory)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (dialogService == null)
                throw new ArgumentNullException(nameof(dialogService));

            if (mouseService == null)
                throw new ArgumentNullException(nameof(mouseService));

            if (accessClientFactory == null)
                throw new ArgumentNullException(nameof(accessClientFactory));

            _dialogService = dialogService;
            _mouseService = mouseService;
            _logger = logger;
            _accessClientFactory = accessClientFactory;
        }

        #endregion

        #region Methods

        public bool Login()
        {
            LoginViewModel viewModel = new LoginViewModel(_logger, _mouseService, _dialogService, _accessClientFactory);
            viewModel.Initialise();

            return !_dialogService.ShowDialog<LoginWindow>(viewModel)
                ? false
                : viewModel.IsLoggedIn;
        }

        #endregion
    }
}
