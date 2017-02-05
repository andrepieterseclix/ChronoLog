using CLog.Business.Access.Managers;
using CLog.Business.Access.Messages;
using CLog.Business.Security.Contracts.Access;
using CLog.DataAccess.Contracts.Repositories.Access;
using CLog.Framework.Business.Models.Results;
using CLog.Infrastructure.Contracts.Security;
using CLog.Models.Access;
using CLog.Models.Mocks.Access;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CLog.Business.Tests.Managers
{
    [TestClass]
    public class AccessManagerTest : ManagerTestBase
    {
        #region Fields

        private readonly Mock<IPasswordHelper> _passwordHelper = new Mock<IPasswordHelper>();

        private readonly Mock<ILoginTokenHelper> _loginTokenHelper = new Mock<ILoginTokenHelper>();

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();


        private readonly Mock<ISessionRepository> _sessionRepository = new Mock<ISessionRepository>();
        
        private readonly IAccessManager _manager;

        #endregion

        #region Constructors and Initialisation

        public AccessManagerTest()
        {
            _manager = new AccessManager(_logger.Object, _passwordHelper.Object, _loginTokenHelper.Object, _userRepository.Object, _sessionRepository.Object);
        }

        #endregion

        #region Test Methods

        [TestMethod]
        [TestCategory("Business - Managers - Access")]
        public void AccessManager_Login_Success()
        {
            // Arrange
            User user1 = AccessDataHelper.GetUser1();

            _passwordHelper
                .Setup(x => x.ComputeHash(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(user1.Password);

            _userRepository
                .Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(user1)
                .Verifiable();

            // Act
            BusinessResult<Session> result = _manager.Login("user name", user1.Password);

            // Assert
            _userRepository.VerifyAll();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.HasResult);
            Assert.IsFalse(result.HasErrors);
        }

        [TestMethod]
        [TestCategory("Business - Managers - Access")]
        public void AccessManager_Login_UserNotExist_Success()
        {
            // Arrange
            _userRepository
                .Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Verifiable();

            // Act
            BusinessResult<Session> result = _manager.Login("user name", "password");

            // Assert
            _userRepository.VerifyAll();
            Assert.IsNotNull(result);
            Assert.IsFalse(result.HasResult);
            Assert.IsTrue(result.HasErrors);
        }


        [TestMethod]
        [TestCategory("Business - Managers - Access")]
        public void AccessManager_Login_InvalidPassword_Success()
        {
            // Arrange
            User user1 = AccessDataHelper.GetUser1();

            _userRepository
                .Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(user1)
                .Verifiable();

            // Act
            BusinessResult<Session> result = _manager.Login("user name", "wrong password");

            // Assert
            _userRepository.VerifyAll();
            Assert.IsNotNull(result);
            Assert.IsFalse(result.HasResult);
            Assert.IsTrue(result.HasErrors);
        }

        [TestMethod]
        [TestCategory("Business - Managers - Access")]
        public void AccessManager_UpdatePassword_UserPassword_UserNotFound_Failure()
        {
            // Arrange
            UserPassword userPassword = AccessDataHelper.GetUserPassword1();
            userPassword.UserName = null;

            _userRepository
                .Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns<User>(null)
                .Verifiable();

            // Act
            BusinessResult result = _manager.UpdatePassword(
                userPassword.UserName,
                userPassword.OldPassword,
                userPassword.NewPassword);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.HasErrors);
            Assert.IsTrue(result.Errors.Any(x => x.Message == ErrorMessages.UserNotFound().Message));
            _userRepository.Verify();
        }

        [TestMethod]
        [TestCategory("Business - Managers - Access")]
        public void AccessManager_UpdatePassword_UserPassword_InvalidOldPassword_Failure()
        {
            // Arrange
            UserPassword userPassword = AccessDataHelper.GetUserPassword1();
            userPassword.OldPassword = "Incorrect password";
            User user1 = AccessDataHelper.GetUser1();

            _userRepository
                .Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(user1)
                .Verifiable();

            _passwordHelper
                .Setup(x => x.ComputeHash(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("Different password hash")
                .Verifiable();

            // Act
            BusinessResult result = _manager.UpdatePassword(
                userPassword.UserName,
                userPassword.OldPassword,
                userPassword.NewPassword);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.HasErrors);
            Assert.IsTrue(result.Errors.Any(x => x.Message == ErrorMessages.IncorrectOldPassword().Message));
            _userRepository.Verify();
            _passwordHelper.Verify();
        }

        [TestMethod]
        [TestCategory("Business - Managers - Access")]
        public void AccessManager_UpdatePassword_UserPassword_NewPasswordInvalid1_Failure()
        {
            // Arrange
            UserPassword userPassword = AccessDataHelper.GetUserPassword1();
            userPassword.NewPassword = "";
            User user1 = AccessDataHelper.GetUser1();

            _userRepository
                .Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(user1)
                .Verifiable();

            _passwordHelper
                .Setup(x => x.ComputeHash(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(user1.Password)
                .Verifiable();

            // Act
            BusinessResult result = _manager.UpdatePassword(
                userPassword.UserName,
                userPassword.OldPassword,
                userPassword.NewPassword);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.HasErrors);
            Assert.IsTrue(result.Errors.Any(x => x.Message == ErrorMessages.InvalidPassword().Message));
            _userRepository.Verify();
            _passwordHelper.Verify();
        }

        [TestMethod]
        [TestCategory("Business - Managers - Access")]
        public void AccessManager_UpdatePassword_UserPassword_NewPasswordInvalid2_Failure()
        {
            // Arrange
            UserPassword userPassword = AccessDataHelper.GetUserPassword1();
            userPassword.NewPassword = "Need some numbers";
            User user1 = AccessDataHelper.GetUser1();

            _userRepository
                .Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(user1)
                .Verifiable();

            _passwordHelper
                .Setup(x => x.ComputeHash(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(user1.Password)
                .Verifiable();

            // Act
            BusinessResult result = _manager.UpdatePassword(
                userPassword.UserName,
                userPassword.OldPassword,
                userPassword.NewPassword);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.HasErrors);
            Assert.IsTrue(result.Errors.Any(x => x.Message == ErrorMessages.InvalidPassword().Message));
            _userRepository.Verify();
            _passwordHelper.Verify();
        }

        [TestMethod]
        [TestCategory("Business - Managers - Access")]
        public void AccessManager_UpdatePassword_UserPassword_Success()
        {
            // Arrange
            string salt = "random salt";
            UserPassword userPassword = AccessDataHelper.GetUserPassword1();
            userPassword.NewPassword = "P@ssword123";
            User user1 = AccessDataHelper.GetUser1();

            _userRepository
                .Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(user1)
                .Verifiable();

            _passwordHelper
                .Setup(x => x.ComputeHash(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(user1.Password)
                .Verifiable();

            _passwordHelper
                .Setup(x => x.GetRandomSalt())
                .Returns(salt);
            
            // Act
            BusinessResult result = _manager.UpdatePassword(
                userPassword.UserName,
                userPassword.OldPassword,
                userPassword.NewPassword);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.HasErrors);
            Assert.AreEqual(user1.Salt, salt);
            _userRepository.Verify();
            _passwordHelper.Verify();
        }

        #endregion
    }
}
