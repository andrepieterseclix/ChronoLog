using CLog.Business.Contracts.Users;
using CLog.Framework.Business.Models.Results;
using CLog.Models.Access;
using CLog.Models.Mocks.Access;
using CLog.Models.Users;
using CLog.Services.Contracts.Users;
using CLog.Services.Models.Users;
using CLog.Services.Models.Users.DataTransfer;
using CLog.Services.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace CLog.Services.Tests.ServiceImplementations
{
    [TestClass]
    public class UserServiceTest : ServiceTestBase
    {
        #region Fields

        private Mock<IUserManager> _userManager = new Mock<IUserManager>();

        private readonly IUserService _service;

        #endregion

        #region Constructors

        public UserServiceTest()
        {
            _service = new UserService(_logger.Object, _userManager.Object);
        }

        #endregion

        #region Test Methods

        [TestMethod]
        [TestCategory("Services - Implementation - Users")]
        public void UserService_UpdateUser_Failure()
        {
            // Arrange
            UserDetailsDto dto = new UserDetailsDto("User Name", "Name", "Surname", "Email");
            UpdateUserRequest request = new UpdateUserRequest(dto);

            _userManager
                .Setup(x => x.UpdateUser(It.IsAny<UserDetails>()))
                .Throws(new Exception("Test"));

            // Act
            UpdateUserResponse response = _service.UpdateUser(request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsNull(response.Session);
            Assert.IsFalse(response.SessionExpired);
            Assert.IsTrue(response.Errors.Count > 0);
        }

        [TestMethod]
        [TestCategory("Services - Implementation - Users")]
        public void UserService_UpdateUser_Success()
        {
            // Arrange
            UserDetailsDto dto = new UserDetailsDto("User Name", "Name", "Surname", "Email");
            UpdateUserRequest request = new UpdateUserRequest(dto);

            User user = AccessDataHelper.GetUser1();
            Session session = AccessDataHelper.GetSession1(user);
            BusinessResult<Session> result = new BusinessResult<Session>(session);

            _userManager
                .Setup(x => x.UpdateUser(It.IsAny<UserDetails>()))
                .Returns(result);

            // Act
            UpdateUserResponse response = _service.UpdateUser(request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Session);
            Assert.AreEqual(response.Session.Id, session.RefId);
            Assert.AreEqual(response.Session.SessionKey, session.SessionKey);
            Assert.IsFalse(response.SessionExpired);
            Assert.IsTrue(response.Errors.Count < 1);
        }

        #endregion
    }
}
