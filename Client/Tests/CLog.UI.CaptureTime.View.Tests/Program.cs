using CLog.Common.Log4NetLogger;
using CLog.Common.Logging;
using CLog.Framework.ServiceClients;
using CLog.ServiceClients.Contracts.Timesheets;
using CLog.Services.Contracts.Timesheets;
using CLog.Services.Models.Timesheets;
using CLog.Services.Models.Timesheets.DataTransfer;
using CLog.UI.CaptureTime.ViewModels;
using CLog.UI.CaptureTime.Views;
using CLog.UI.Common.Services;
using CLog.UI.Framework.Testing;
using CLog.UI.Testing.Configuration;
using Moq;
using System;
using System.Threading;

namespace CLog.UI.CaptureTime.View.Tests
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //// Setup
            //ILogger logger = new Log4NetLogger();
            //Mock<IStatusService> statusService = new Mock<IStatusService>();
            //Mock<ITimesheetClientFactory> timesheetClientFactory = new Mock<ITimesheetClientFactory>();
            //Mock<IServiceClient<ITimesheetService>> serviceClient = new Mock<IServiceClient<ITimesheetService>>();
            //Mock<ITimesheetService> timesheetService = new Mock<ITimesheetService>();

            //// TODO:  create data helper!
            //DateTime fromDate = new DateTime(2017, 1, 1);
            //CapturedTimeDto[] items = new[]
            //{
            //    new CapturedTimeDto("UserName", (fromDate = fromDate.AddDays(1)), 8, true, false, true),
            //    new CapturedTimeDto("UserName", (fromDate = fromDate.AddDays(1)), 7, false, true, true),
            //    new CapturedTimeDto("UserName", (fromDate = fromDate.AddDays(1)), 6, false, false, false),
            //    new CapturedTimeDto("UserName", (fromDate = fromDate.AddDays(1)), 5, false, false, true),
            //    new CapturedTimeDto("UserName", (fromDate = fromDate.AddDays(1)), 4, false, false, true),
            //    new CapturedTimeDto("UserName", (fromDate = fromDate.AddDays(1)), 3, false, false, true),
            //    new CapturedTimeDto("UserName", (fromDate = fromDate.AddDays(1)), 0, false, false, true),
            //};
            //GetCapturedTimeResponse getCapturedTimeResponse = new GetCapturedTimeResponse(items);

            //timesheetClientFactory
            //    .Setup(x => x.Create())
            //    .Returns(serviceClient.Object)
            //    .Callback(() => LoggerHelper.Info(logger, $"{nameof(ITimesheetClientFactory)}.{nameof(ITimesheetClientFactory.Create)}"));

            //serviceClient
            //    .SetupGet(x => x.Proxy)
            //    .Returns(timesheetService.Object)
            //    .Callback(() => LoggerHelper.Info(logger, $"{nameof(IServiceClient<ITimesheetService>)}.{nameof(IServiceClient<ITimesheetService>.Proxy)}"));

            //timesheetService
            //    .Setup(x => x.GetCapturedTime(It.IsAny<GetCapturedTimeRequest>()))
            //    .Returns(getCapturedTimeResponse)
            //    .Callback(() => LoggerHelper.Info(logger, $"{nameof(ITimesheetService)}.{nameof(ITimesheetService.GetCapturedTime)}"));

            //CaptureTimeViewModel viewModel = new CaptureTimeViewModel(logger, statusService.Object, timesheetClientFactory.Object);
            //viewModel.Initialise();

            //// TODO:  Make this invokable from the Test Window!  Change back to private
            //new Thread(() =>
            //{
            //    Thread.Sleep(1000);
            //    viewModel.SelectedDateChanged(DateTime.Now);
            //}).Start();

            //// Execute
            //Bootstrapper bootstrapper = new Bootstrapper(logger);
            ////bootstrapper.Run(new CaptureTimeView(), viewModel);
            //bootstrapper.Run();


            TestsBootstrapper bootstrapper = new TestsBootstrapper();
            bootstrapper.Run();
        }
    }
}
