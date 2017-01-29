using CLog.Business.Contracts.Timesheets;
using CLog.Business.Timesheets.Managers;
using CLog.Business.Timesheets.Messages;
using CLog.DataAccess.Contracts.Repositories.Access;
using CLog.DataAccess.Contracts.Repositories.Timesheets;
using CLog.Framework.Business.Exceptions;
using CLog.Framework.Business.Models.Results;
using CLog.Models.Access;
using CLog.Models.Mocks.Access;
using CLog.Models.Mocks.Timesheets;
using CLog.Models.Timesheets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CLog.Business.Tests.Managers
{
    [TestClass]
    public class TimesheetManagerTest : ManagerTestBase
    {
        #region Fields

        private readonly Mock<ICapturedTimeRepository> _capturedTimeRepository = new Mock<ICapturedTimeRepository>();

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();

        private readonly ITimesheetManager _manager;

        #endregion

        #region Constructor

        public TimesheetManagerTest()
        {
            _manager = new TimesheetManager(_logger.Object, _capturedTimeRepository.Object, _userRepository.Object);
        }

        #endregion

        #region Test Methods

        #region GetCapturedTime

        [TestMethod]
        [TestCategory("Business - Managers - Timesheets")]
        public void TimesheetManager_GetCapturedTime_ToGreaterThanFrom_Failure()
        {
            // Arrange
            DateTime from = new DateTime(2000, 1, 10);
            DateTime to = new DateTime(2000, 1, 2);

            // Act
            BusinessResult<CapturedTime[]> result = _manager.GetCapturedTime(from, to);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.HasResult);
            Assert.IsTrue(result.HasErrors);
            Assert.IsTrue(result.Errors.Any(e => e.Message == ErrorMessages.InvalidFromAndToDate.Message));
        }

        [TestMethod]
        [TestCategory("Business - Managers - Timesheets")]
        public void TimesheetManager_GetCapturedTime_MaxDateRangeExceeded_Failure()
        {
            // Arrange
            DateTime from = new DateTime(2000, 1, 1);
            DateTime to = new DateTime(2015, 1, 1);

            // Act
            BusinessResult<CapturedTime[]> result = _manager.GetCapturedTime(from, to);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.HasResult);
            Assert.IsTrue(result.HasErrors);
            Assert.IsTrue(result.Errors.Any(e => e.Message == ErrorMessages.QueryMaxDaySpan.Message));
        }

        [TestMethod]
        [TestCategory("Business - Managers - Timesheets")]
        [ExpectedException(typeof(BusinessException))]
        public void TimesheetManager_GetCapturedTime_UserNotFound_Failure()
        {
            // Arrange
            DateTime from = new DateTime(2000, 1, 1);
            DateTime to = new DateTime(2000, 1, 5);

            // Act
            BusinessResult<CapturedTime[]> result = _manager.GetCapturedTime(from, to);
        }

        [TestMethod]
        [TestCategory("Business - Managers - Timesheets")]
        public void TimesheetManager_GetCapturedTime_DuplicateDatesInRepo_Failure()
        {
            // Arrange
            int days = 2;
            DateTime from = new DateTime(2000, 1, 1);
            DateTime to = from.AddDays(days - 1);

            User user = AccessDataHelper.GetUser1();
            CapturedTime[] items = TimesheetsDataHelper.GetCapturedTimeItemsConsecutive(user, from, days).ToArray();
            items[1].Date = items[0].Date;
            items.ToList().ForEach(x => user.CapturedTimeItems.Add(x));

            _userRepository
                .Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(user)
                .Verifiable();

            // Act
            BusinessResult<CapturedTime[]> result = _manager.GetCapturedTime(from, to);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.HasResult);
            Assert.IsTrue(result.HasErrors);
            Assert.IsTrue(result.Errors.Any(x => x.Message == ErrorMessages.CapturedTimeDuplicateDates.Message));
            _userRepository.Verify();
        }

        [TestMethod]
        [TestCategory("Business - Managers - Timesheets")]
        public void TimesheetManager_GetCapturedTime_NoCapturedItems_Success()
        {
            // Arrange
            int days = 7;
            DateTime from = new DateTime(2000, 1, 1);
            DateTime to = from.AddDays(days - 1);

            User user = AccessDataHelper.GetUser1();

            _userRepository
                .Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(user)
                .Verifiable();

            // Act
            BusinessResult<CapturedTime[]> result = _manager.GetCapturedTime(from, to);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.HasResult);
            Assert.IsFalse(result.HasErrors);
            Assert.AreEqual(result.Result.Length, days);
            Assert.IsTrue(result.Result.All(x => x.IsEnabled));
            Assert.IsTrue(result.Result.All(x => x.User != null && x.User.UserName == user.UserName));
            _userRepository.Verify();
        }

        [TestMethod]
        [TestCategory("Business - Managers - Timesheets")]
        public void TimesheetManager_GetCapturedTime_NonConsecutiveRangeFromRepo_Success()
        {
            // Arrange
            int days = 7;
            DateTime from = new DateTime(2000, 1, 1);
            DateTime to = from.AddDays(days - 1);

            User user = AccessDataHelper.GetUser1();
            CapturedTime[] items =
                TimesheetsDataHelper.GetCapturedTimeItemsConsecutive(user, new DateTime(2000, 1, 2), 2)
                .Union(TimesheetsDataHelper.GetCapturedTimeItemsConsecutive(user, new DateTime(2000, 1, 5), 2))
                .ToArray();

            items.ToList().ForEach(x => user.CapturedTimeItems.Add(x));

            _userRepository
                .Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(user)
                .Verifiable();

            // Act
            BusinessResult<CapturedTime[]> result = _manager.GetCapturedTime(from, to);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.HasResult);
            Assert.IsFalse(result.HasErrors);
            Assert.AreEqual(result.Result.Length, days);
            Assert.IsTrue(result.Result.All(x => x.IsEnabled));
            Assert.IsTrue(result.Result.All(x => x.User != null && x.User.UserName == user.UserName));
            _userRepository.Verify();
        }

        [TestMethod]
        [TestCategory("Business - Managers - Timesheets")]
        public void TimesheetManager_GetCapturedTime_Success()
        {
            // Arrange
            int days = 7;
            DateTime from = new DateTime(2000, 1, 1);
            DateTime to = from.AddDays(days - 1);

            User user = AccessDataHelper.GetUser1();
            CapturedTime[] items = TimesheetsDataHelper.GetCapturedTimeItemsConsecutive(user, from, days).ToArray();
            items.ToList().ForEach(x => user.CapturedTimeItems.Add(x));

            _userRepository
                .Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(user)
                .Verifiable();

            // Act
            BusinessResult<CapturedTime[]> result = _manager.GetCapturedTime(from, to);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.HasResult);
            Assert.IsFalse(result.HasErrors);
            Assert.AreEqual(result.Result.Length, days);
            Assert.IsTrue(result.Result.All(x => x.IsEnabled));
            Assert.IsTrue(result.Result.All(x => x.User != null && x.User.UserName == user.UserName));
            _userRepository.Verify();
        }

        [TestMethod]
        [TestCategory("Business - Managers - Timesheets")]
        public void TimesheetManager_GetCapturedTime_EnablePastDaysAndToday_Success()
        {
            // Arrange
            int days = 7;
            DateTime today = DateTime.Now.Date;
            DateTime from = today.AddDays(-3);
            DateTime to = from.AddDays(days - 1);

            User user = AccessDataHelper.GetUser1();
            CapturedTime[] items = TimesheetsDataHelper.GetCapturedTimeItemsConsecutive(user, from, days).ToArray();
            items.ToList().ForEach(x => user.CapturedTimeItems.Add(x));

            _userRepository
                .Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(user)
                .Verifiable();

            // Act
            BusinessResult<CapturedTime[]> result = _manager.GetCapturedTime(from, to);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.HasResult);
            Assert.IsFalse(result.HasErrors);
            Assert.AreEqual(result.Result.Length, days);
            Assert.IsTrue(result.Result.Where(x => x.Date <= today).All(x => x.IsEnabled));
            Assert.IsTrue(result.Result.Where(x => x.Date > today).All(x => !x.IsEnabled));
            Assert.IsTrue(result.Result.All(x => x.User != null && x.User.UserName == user.UserName));
            _userRepository.Verify();
        }

        [TestMethod]
        [TestCategory("Business - Managers - Timesheets")]
        public void TimesheetManager_GetCapturedTime_LockAndEnable_Success()
        {
            // Arrange
            int days = 7;
            DateTime today = DateTime.Now.Date;
            DateTime from = today.AddDays(-3);
            DateTime to = from.AddDays(days - 1);

            User user = AccessDataHelper.GetUser1();
            CapturedTime[] items = TimesheetsDataHelper.GetCapturedTimeItemsConsecutive(user, from, days).ToArray();
            items[0].IsLocked = true;
            items.ToList().ForEach(x => user.CapturedTimeItems.Add(x));

            _userRepository
                .Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(user)
                .Verifiable();

            // Act
            BusinessResult<CapturedTime[]> result = _manager.GetCapturedTime(from, to);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.HasResult);
            Assert.IsFalse(result.HasErrors);
            Assert.AreEqual(result.Result.Length, days);
            Assert.IsTrue(result.Result.Where(x => x.Date <= today && !x.IsLocked).All(x => x.IsEnabled));
            Assert.IsTrue(result.Result.Where(x => x.Date <= today && x.IsLocked).All(x => !x.IsEnabled));
            Assert.IsTrue(result.Result.Where(x => x.Date > today).All(x => !x.IsEnabled));
            Assert.IsTrue(result.Result.All(x => x.User != null && x.User.UserName == user.UserName));
            _userRepository.Verify();
        }

        #endregion

        #region SaveCapturedTime

        [TestMethod]
        [TestCategory("Business - Managers - Timesheets")]
        public void TimesheetManager_SaveCapturedTime_InvalidArgs_Failure()
        {
            // Arrange
            CapturedTimeDetail[] capturedTimeItems = null;

            // Act
            BusinessResult result = _manager.SaveCapturedTime(capturedTimeItems);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.HasErrors);
            Assert.IsTrue(result.Errors.Any(x => x.Message == ErrorMessages.CapturedTimeItemsNotValid.Message));
        }

        [TestMethod]
        [TestCategory("Business - Managers - Timesheets")]
        public void TimesheetManager_SaveCapturedTime_UpdateOtherUser_Failure()
        {
            // Arrange
            int days = 7;
            User user = AccessDataHelper.GetUser1();
            DateTime fromDate = new DateTime(2000, 1, 1);
            CapturedTimeDetail[] capturedTimeItems = TimesheetsDataHelper.GetCapturedTimeDetailItemsConsecutive(user, fromDate, days).ToArray();
            capturedTimeItems[0].UserName = "AnotherUserName";

            // Act
            BusinessResult result = _manager.SaveCapturedTime(capturedTimeItems);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.HasErrors);
            Assert.IsTrue(result.Errors.Any(x => x.Message == ErrorMessages.InvalidUserRequest.Message));
        }

        [TestMethod]
        [TestCategory("Business - Managers - Timesheets")]
        public void TimesheetManager_SaveCapturedTime_SkipUpdateZeroHours_Failure()
        {
            // Arrange
            int saved = 0;
            int days = 7;
            User user = AccessDataHelper.GetUser1();
            DateTime today = DateTime.Now.Date;
            DateTime from = today.AddDays(-3);
            DateTime to = from.AddDays(days - 1);

            List<CapturedTimeDetail> capturedTimeItems = TimesheetsDataHelper
                .GetCapturedTimeDetailItemsConsecutive(user, from, days)
                .ToList();

            capturedTimeItems.ForEach(x => x.HoursWorked = 0);

            List<CapturedTime> items = TimesheetsDataHelper.GetCapturedTimeItemsConsecutive(user, from, days).ToList();
            items.ForEach(x =>
            {
                x.HoursWorked = 0;
                user.CapturedTimeItems.Add(x);
            });

            _userRepository
                .Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(user)
                .Verifiable();

            _capturedTimeRepository
                .Setup(x => x.Save(It.IsAny<CapturedTime>()))
                .Callback(() => saved++);

            // Act
            BusinessResult result = _manager.SaveCapturedTime(capturedTimeItems.ToArray());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.HasErrors);
            Assert.AreEqual(saved, 0);
            _userRepository.Verify();
            _capturedTimeRepository.Verify();
        }

        [TestMethod]
        [TestCategory("Business - Managers - Timesheets")]
        public void TimesheetManager_SaveCapturedTime_DuplicateDateState_Failure()
        {
            // Arrange
            int days = 7;
            int pastAndTodayDays = 4;
            User user = AccessDataHelper.GetUser1();
            DateTime today = DateTime.Now.Date;
            DateTime from = today.AddDays((pastAndTodayDays - 1) * -1);
            DateTime to = from.AddDays(days - 1);

            List<CapturedTimeDetail> capturedTimeItems = TimesheetsDataHelper
                .GetCapturedTimeDetailItemsConsecutive(user, from, days)
                .ToList();

            List<CapturedTime> items = TimesheetsDataHelper
                .GetCapturedTimeItemsConsecutive(user, from, days)
                .ToList();

            List<DateState> dateStates = new List<DateState>()
            {
                new DateState(from, true, false),
                new DateState(from, false, true) // Duplicate date
            };

            items.ForEach(x =>
            {
                x.HoursWorked = 0;
                user.CapturedTimeItems.Add(x);
            });

            _userRepository
                .Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(user)
                .Verifiable();

            _capturedTimeRepository
                .Setup(x => x.GetAllDateStates(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(dateStates)
                .Verifiable();
            
            // Act
            BusinessResult result = _manager.SaveCapturedTime(capturedTimeItems.ToArray());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.HasErrors);
            Assert.IsTrue(result.Errors.Any(x => x.Message == ErrorMessages.DuplicateDateState.Message));
            _userRepository.Verify();
            _capturedTimeRepository.Verify();
        }

        [TestMethod]
        [TestCategory("Business - Managers - Timesheets")]
        public void TimesheetManager_SaveCapturedTime_TryUpdateDisabledDay_Failure()
        {
            // Arrange
            int saved = 0;
            int days = 7;
            int pastAndTodayDays = 4;
            bool saveChangesCalled = false;
            User user = AccessDataHelper.GetUser1();
            DateTime today = DateTime.Now.Date;
            DateTime from = today.AddDays((pastAndTodayDays - 1) * -1);
            DateTime to = from.AddDays(days - 1);

            List<CapturedTimeDetail> capturedTimeItems = TimesheetsDataHelper
                .GetCapturedTimeDetailItemsConsecutive(user, from, days)
                .ToList();

            List<CapturedTime> items = TimesheetsDataHelper
                .GetCapturedTimeItemsConsecutive(user, from, days)
                .ToList();

            List<DateState> dateStates = new List<DateState>()
            {
                new DateState(from, true, false),
                new DateState(from.AddDays(1), false, true)
            };

            items.ForEach(x =>
            {
                x.HoursWorked = 0;
                user.CapturedTimeItems.Add(x);
            });

            _userRepository
                .Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(user)
                .Verifiable();

            _capturedTimeRepository
                .Setup(x => x.GetAllDateStates(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(dateStates)
                .Verifiable();

            _capturedTimeRepository
                .Setup(x => x.Save(It.IsAny<CapturedTime>()))
                .Callback(() => saved++);

            _capturedTimeRepository
                .Setup(x => x.SaveChanges())
                .Callback(() => saveChangesCalled = true);

            // Act
            BusinessResult result = _manager.SaveCapturedTime(capturedTimeItems.ToArray());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.HasErrors);
            Assert.IsTrue(result.Errors.Any(x => x.Message == ErrorMessages.CaptureDateNotEnabled.Message));
            Assert.AreEqual(saved, pastAndTodayDays - 1);
            Assert.IsFalse(saveChangesCalled);
            _userRepository.Verify();
            _capturedTimeRepository.Verify();
        }

        [TestMethod]
        [TestCategory("Business - Managers - Timesheets")]
        public void TimesheetManager_SaveCapturedTime_Success()
        {
            // Arrange
            int saved = 0;
            int days = 5;
            User user = AccessDataHelper.GetUser1();
            DateTime from = new DateTime(2017, 1, 2);

            List<CapturedTimeDetail> capturedTimeItems = TimesheetsDataHelper
                .GetCapturedTimeDetailItemsConsecutive(user, from, days)
                .ToList();

            List<CapturedTime> items = TimesheetsDataHelper
                .GetCapturedTimeItemsConsecutive(user, from, days)
                .ToList();

            items.ForEach(x =>
            {
                x.HoursWorked = 0;
                user.CapturedTimeItems.Add(x);
            });

            _userRepository
                .Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(user)
                .Verifiable();

            _capturedTimeRepository
                .Setup(x => x.Save(It.IsAny<CapturedTime>()))
                .Callback(() => saved++);

            _capturedTimeRepository
                .Setup(x => x.SaveChanges())
                .Verifiable();

            // Act
            BusinessResult result = _manager.SaveCapturedTime(capturedTimeItems.ToArray());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.HasErrors);
            Assert.AreEqual(saved, days);
            _userRepository.Verify();
            _capturedTimeRepository.Verify();
        }

        #endregion

        #endregion
    }
}
