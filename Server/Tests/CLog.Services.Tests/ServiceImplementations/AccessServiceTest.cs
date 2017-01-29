using CLog.Business.Security.Contracts.Access;
using CLog.Common.Logging;
using CLog.Framework.Business.Models.Results;
using CLog.Models.Access;
using CLog.Models.Mocks.Access;
using CLog.Services.Access;
using CLog.Services.Models.Access;
using CLog.Services.Models.Access.DataTransfer;
using CLog.Services.Security.Contracts.Access;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace CLog.Services.Tests.ServiceImplementations
{
    [TestClass]
    public class AccessServiceTest : ServiceTestBase
    {
        #region Fields
        
        private readonly Mock<IAccessManager> _accessManager = new Mock<IAccessManager>();

        private readonly IAccessService _service;

        #endregion

        #region Constructors

        public AccessServiceTest()
        {
            _service = new AccessService(_logger.Object, _accessManager.Object);
        }

        #endregion

        #region Test Methods

        [TestMethod]
        [TestCategory("Services - Implementation - Access")]
        public void AccessService_Login_Success()
        {
            // Arrange
            User user = AccessDataHelper.GetUser1();
            Session session = AccessDataHelper.GetSession1(user);

            _accessManager
                .Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new BusinessResult<Session>(session));

            LoginRequest request = new LoginRequest("User Name", "Password");

            // Act
            LoginResponse response = _service.Login(request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsLoggedIn);
            Assert.IsNotNull(response.Session);
            Assert.IsNotNull(response.User);
            Assert.IsTrue(response.Errors.Count == 0);
        }

        [TestMethod]
        [TestCategory("Services - Implementation - Access")]
        public void AccessService_Login_Failure()
        {
            // Arrange
            User user = AccessDataHelper.GetUser1();
            Session session = AccessDataHelper.GetSession1(user);

            _accessManager
                .Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("Test"));

            LoginRequest request = new LoginRequest("User Name", "Password");

            // Act
            LoginResponse response = _service.Login(request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.IsLoggedIn);
            Assert.IsTrue(response.Errors.Count > 0);
        }

        [TestMethod]
        [TestCategory("Services - Implementation - Access")]
        public void AccessService_Logout_Faliure()
        {
            // Arrange
            _accessManager
                .Setup(x => x.Logout(It.IsAny<string>()))
                .Throws(new Exception("Test"));

            LogoutRequest request = new LogoutRequest("User Name", Guid.Empty, "Session Key");

            // Act
            LogoutResponse response = _service.Logout(request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Errors.Count > 0);
        }

        [TestMethod]
        [TestCategory("Services - Implementation - Access")]
        public void AccessService_Logout_Success()
        {
            // Arrange
            BusinessResult result = new BusinessResult();

            _accessManager
                .Setup(x => x.Logout(It.IsAny<string>()))
                .Returns(result);

            LogoutRequest request = new LogoutRequest("User Name", Guid.Empty, "Session Key");

            // Act
            LogoutResponse response = _service.Logout(request);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.Errors.Count, 0);
        }

        [TestMethod]
        [TestCategory("Services - Implementation - Access")]
        public void AccessService_UpdateUserPassword_Success()
        {
            // Arrange
            BusinessResult result = new BusinessResult();

            _accessManager
                .Setup(x => x.UpdatePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(result);

            UserPasswordDto dto = new UserPasswordDto("User Name", "Old Password", "New Password");
            UpdateUserPasswordRequest request = new UpdateUserPasswordRequest(dto);

            // Act
            UpdateUserPasswordResponse response = _service.UpdateUserPassword(request);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.Errors.Count, 0);
        }

        #endregion
    }
}
