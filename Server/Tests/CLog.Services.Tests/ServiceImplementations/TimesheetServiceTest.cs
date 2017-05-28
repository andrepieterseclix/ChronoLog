using CLog.Business.Contracts.Timesheets;
using CLog.Framework.Business.Models.Results;
using CLog.Framework.Models;
using CLog.Models.Access;
using CLog.Models.Mocks.Access;
using CLog.Models.Mocks.Timesheets;
using CLog.Models.Timesheets;
using CLog.Services.Contracts.Timesheets;
using CLog.Services.Models.Timesheets;
using CLog.Services.Models.Timesheets.DataTransfer;
using CLog.Services.Timesheets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;

namespace CLog.Services.Tests.ServiceImplementations
{
    [TestClass]
    public class TimesheetServiceTest : ServiceTestBase
    {
        #region Fields

        private readonly Mock<ITimesheetManager> _timesheetManager = new Mock<ITimesheetManager>();

        private readonly ITimesheetService _service;

        #endregion

        #region Constructors

        public TimesheetServiceTest()
        {
            _service = new TimesheetService(_logger.Object, _timesheetManager.Object);
        }

        #endregion

        #region Test Methods

        [TestMethod]
        [TestCategory("Services - Implementation - Timesheets")]
        public void TimesheetService_GetCapturedTime_Failure()
        {
            // Arrange
            GetCapturedTimeRequest request = new GetCapturedTimeRequest(DateTime.MinValue, DateTime.MinValue);
            User user = AccessDataHelper.GetUser1();

            BusinessResult<CapturedTime[]> result = new BusinessResult<CapturedTime[]>();
            result.Errors.Add(new ErrorMessage(ErrorCategory.General, "Code", "Message"));

            _timesheetManager
                .Setup(x => x.GetCapturedTime(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(result)
                .Verifiable();

            // Act
            GetCapturedTimeResponse response = _service.GetCapturedTime(request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsNull(response.CapturedTimeItems);
            Assert.AreEqual(response.Errors.Count, result.Errors.Count);
            _timesheetManager.Verify();
        }

        [TestMethod]
        [TestCategory("Services - Implementation - Timesheets")]
        public void TimesheetService_GetCapturedTime_Success()
        {
            // Arrange
            GetCapturedTimeRequest request = new GetCapturedTimeRequest(DateTime.MinValue, DateTime.MinValue);
            User user = AccessDataHelper.GetUser1();
            DateTime fromDate = new DateTime(2017, 1, 2);
            int days = 5;

            CapturedTime[] items = TimesheetsDataHelper.GetCapturedTimeItemsConsecutive(user, fromDate, days).ToArray();
            BusinessResult<CapturedTime[]> result = new BusinessResult<CapturedTime[]>(items);

            _timesheetManager
                .Setup(x => x.GetCapturedTime(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(result)
                .Verifiable();

            // Act
            GetCapturedTimeResponse response = _service.GetCapturedTime(request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.CapturedTimeItems);
            Assert.AreEqual(response.CapturedTimeItems.Length, items.Length);
            Assert.AreEqual(response.Errors.Count, 0);
            _timesheetManager.Verify();
        }

        [TestMethod]
        [TestCategory("Services - Implementation - Timesheets")]
        public void TimesheetService_SaveCapturedTime_Failure()
        {
            // Arrange
            User user = AccessDataHelper.GetUser1();
            DateTime fromDate = new DateTime(2017, 1, 2);
            int days = 5;

            CapturedTimeDetailDto[] items = TimesheetsDataHelper.GetCapturedTimeDetailDtoItemsConsecutive(user, fromDate, days).ToArray();
            SaveCapturedTimeRequest request = new SaveCapturedTimeRequest(items);

            BusinessResult result = new BusinessResult();
            result.Errors.Add(new ErrorMessage(ErrorCategory.General, "Code", "Message"));

            _timesheetManager
                .Setup(x => x.SaveCapturedTime(It.IsAny<CapturedTimeDetail[]>()))
                .Returns(result)
                .Verifiable();

            // Act
            SaveCapturedTimeResponse response = _service.SaveCapturedTime(request);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.Errors.Count, result.Errors.Count);
            _timesheetManager.Verify();
        }

        [TestMethod]
        [TestCategory("Services - Implementation - Timesheets")]
        public void TimesheetService_SaveCapturedTime_Success()
        {
            // Arrange
            User user = AccessDataHelper.GetUser1();
            DateTime fromDate = new DateTime(2017, 1, 2);
            int days = 5;

            CapturedTimeDetailDto[] items = TimesheetsDataHelper.GetCapturedTimeDetailDtoItemsConsecutive(user, fromDate, days).ToArray();
            SaveCapturedTimeRequest request = new SaveCapturedTimeRequest(items);

            BusinessResult result = new BusinessResult();

            _timesheetManager
                .Setup(x => x.SaveCapturedTime(It.IsAny<CapturedTimeDetail[]>()))
                .Returns(result)
                .Verifiable();

            // Act
            SaveCapturedTimeResponse response = _service.SaveCapturedTime(request);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.Errors.Count, 0);
            _timesheetManager.Verify();
        }

        #endregion
    }
}
