using CLog.Common.Logging;
using CLog.Framework.Configuration.Bootstrap;
using CLog.Framework.ServiceClients;
using CLog.ServiceClients.Contracts.Access;
using CLog.ServiceClients.Contracts.Timesheets;
using CLog.ServiceClients.Contracts.Users;
using CLog.Services.Contracts.Timesheets;
using CLog.Services.Contracts.Users;
using CLog.Services.Models.Access.DataTransfer;
using CLog.Services.Models.Timesheets.DataTransfer;
using CLog.Services.Models.Users.DataTransfer;
using CLog.Services.Security.Contracts.Access;
using CLog.UI.Framework.Testing.Models;
using CLog.UI.Testing.Configuration.DataHelpers;
using Microsoft.Practices.Unity;
using Moq;
using System;
using System.Linq;
using System.Threading;

namespace CLog.UI.Testing.Configuration.Installers
{
    public class ServicesIntegrationModuleInstaller : IUnityDependencyInstaller
    {
        public void Install(IUnityContainer container)
        {
            TestModel testModel = container.Resolve<TestModel>();
            ServicesMockSettingsModel servicesMockSettingsModel =
                (ServicesMockSettingsModel)testModel.Environment.Children.First(x => x.GetType() == typeof(ServicesMockSettingsModel));

            if (testModel == null)
                throw new ArgumentNullException(nameof(testModel));

            MockModel mockModel = null;

            // Mocks - Access Service
            Mock<IAccessClientFactory> accessClientFactory = new Mock<IAccessClientFactory>();
            Mock<IServiceClient<IAccessService>> accessClient = new Mock<IServiceClient<IAccessService>>();
            Mock<IAccessService> accessService = new Mock<IAccessService>();

            accessClientFactory
                .Setup(x => x.Create())
                .Returns(accessClient.Object)
                .Callback(() => container.Resolve<ILogger>().Info($"{nameof(IAccessClientFactory)}.{nameof(IAccessClientFactory.Create)}"));

            accessClient
                .SetupGet(x => x.Proxy)
                .Returns(accessService.Object)
                .Callback(() => container.Resolve<ILogger>().Info($"{nameof(IServiceClient<IAccessService>)}.{nameof(IServiceClient<IAccessService>.Proxy)}"));

            mockModel = new MockModel(nameof(IAccessService), accessService);
            testModel.Mocks.Children.Add(mockModel);

            LoginResponse loginResponse = AccessDataHelper.GetLoginResponse();
            mockModel.Children.Add(new MethodModel(nameof(IAccessService.Login), AccessDataHelper.GetLoginResponse()));
            accessService
                .Setup(x => x.Login(It.IsAny<LoginRequest>()))
                .Returns(loginResponse)
                .Callback(() =>
                {
                    container.Resolve<ILogger>().Info($"{nameof(IAccessService)}.{nameof(IAccessService.Login)}");
                    Thread.Sleep(servicesMockSettingsModel.SimulateLatencyMilliseconds);
                });

            LogoutResponse logoutResponse = AccessDataHelper.GetLogoutResponse();
            mockModel.Children.Add(new MethodModel(nameof(IAccessService.Logout), logoutResponse));
            accessService
                .Setup(x => x.Logout(It.IsAny<LogoutRequest>()))
                .Returns(logoutResponse)
                .Callback(() =>
                {
                    container.Resolve<ILogger>().Info($"{nameof(IAccessService)}.{nameof(IAccessService.Logout)}");
                    Thread.Sleep(servicesMockSettingsModel.SimulateLatencyMilliseconds);
                });

            UpdateUserPasswordResponse updateUserPasswordResponse = AccessDataHelper.GetUpdateUserPasswordResponse();
            mockModel.Children.Add(new MethodModel(nameof(IAccessService.UpdateUserPassword), updateUserPasswordResponse));
            accessService
                .Setup(x => x.UpdateUserPassword(It.IsAny<UpdateUserPasswordRequest>()))
                .Returns(updateUserPasswordResponse)
                .Callback(() =>
                {
                    container.Resolve<ILogger>().Info($"{nameof(IAccessService)}.{nameof(IAccessService.UpdateUserPassword)}");
                    Thread.Sleep(servicesMockSettingsModel.SimulateLatencyMilliseconds);
                });

            // Mocks - Timesheet Service
            Mock<ITimesheetClientFactory> timesheetClientFactory = new Mock<ITimesheetClientFactory>();
            Mock<IServiceClient<ITimesheetService>> timesheetClient = new Mock<IServiceClient<ITimesheetService>>();
            Mock<ITimesheetService> timesheetService = new Mock<ITimesheetService>();

            timesheetClientFactory
                .Setup(x => x.Create())
                .Returns(timesheetClient.Object)
                .Callback(() => container.Resolve<ILogger>().Info($"{nameof(ITimesheetClientFactory)}.{nameof(ITimesheetClientFactory.Create)}"));

            timesheetClient
                .SetupGet(x => x.Proxy)
                .Returns(timesheetService.Object)
                .Callback(() => container.Resolve<ILogger>().Info($"{nameof(IServiceClient<ITimesheetService>)}.{nameof(IServiceClient<ITimesheetService>.Proxy)}"));

            mockModel = new MockModel(nameof(ITimesheetService), timesheetService);
            testModel.Mocks.Children.Add(mockModel);

            GetCapturedTimeResponse getCapturedTimeResponse = TimesheetsDataHelper.GetGetCapturedTimeResponse();
            mockModel.Children.Add(new MethodModel(nameof(ITimesheetService.GetCapturedTime), getCapturedTimeResponse));
            timesheetService
                .Setup(x => x.GetCapturedTime(It.IsAny<GetCapturedTimeRequest>()))
                .Returns(getCapturedTimeResponse)
                .Callback(() =>
                {
                    container.Resolve<ILogger>().Info($"{nameof(ITimesheetService)}.{nameof(ITimesheetService.GetCapturedTime)}");
                    Thread.Sleep(servicesMockSettingsModel.SimulateLatencyMilliseconds);
                });

            SaveCapturedTimeResponse saveCapturedTimeResponse = TimesheetsDataHelper.GetSaveCapturedTimeResponse();
            mockModel.Children.Add(new MethodModel(nameof(ITimesheetService.SaveCapturedTime), saveCapturedTimeResponse));
            timesheetService
                .Setup(x => x.SaveCapturedTime(It.IsAny<SaveCapturedTimeRequest>()))
                .Returns(saveCapturedTimeResponse)
                .Callback(() =>
                {
                    container.Resolve<ILogger>().Info($"{nameof(ITimesheetService)}.{nameof(ITimesheetService.SaveCapturedTime)}");
                    Thread.Sleep(servicesMockSettingsModel.SimulateLatencyMilliseconds);
                });

            // Mocks - User Service
            Mock<IUserClientFactory> userClientFactory = new Mock<IUserClientFactory>();
            Mock<IServiceClient<IUserService>> userClient = new Mock<IServiceClient<IUserService>>();
            Mock<IUserService> userService = new Mock<IUserService>();

            userClientFactory
                .Setup(x => x.Create())
                .Returns(userClient.Object)
                .Callback(() => container.Resolve<ILogger>().Info($"{nameof(IUserClientFactory)}.{nameof(IUserClientFactory.Create)}"));

            userClient
                .SetupGet(x => x.Proxy)
                .Returns(userService.Object)
                .Callback(() => container.Resolve<ILogger>().Info($"{nameof(IServiceClient<IUserService>)}.{nameof(IServiceClient<IUserService>.Proxy)}"));

            mockModel = new MockModel(nameof(IUserService), userService);
            testModel.Mocks.Children.Add(mockModel);

            UpdateUserResponse updateUserResponse = UsersDataHelper.GetUpdateUserResponse();
            mockModel.Children.Add(new MethodModel(nameof(IUserService.UpdateUser), updateUserResponse));
            userService
                .Setup(x => x.UpdateUser(It.IsAny<UpdateUserRequest>()))
                .Returns(updateUserResponse)
                .Callback(() =>
                {
                    container.Resolve<ILogger>().Info($"{nameof(IUserService)}.{nameof(IUserService.UpdateUser)}");
                    Thread.Sleep(servicesMockSettingsModel.SimulateLatencyMilliseconds);
                });

            // Register
            container
                .RegisterInstance(accessClientFactory.Object);

            container
                .RegisterInstance(timesheetClientFactory.Object);

            container
                .RegisterInstance(userClientFactory.Object);
        }
    }
}
