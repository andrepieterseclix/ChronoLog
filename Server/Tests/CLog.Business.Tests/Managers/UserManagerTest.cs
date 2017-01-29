using CLog.Business.Contracts.Users;
using CLog.Business.Users.Managers;
using CLog.Business.Users.Messages;
using CLog.DataAccess.Contracts.Repositories.Access;
using CLog.Framework.Business.Models.Results;
using CLog.Infrastructure.Contracts.Security;
using CLog.Models.Access;
using CLog.Models.Mocks.Access;
using CLog.Models.Mocks.Users;
using CLog.Models.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace CLog.Business.Tests.Managers
{
    [TestClass]
    public class UserManagerTest : ManagerTestBase
    {
        #region Fields

        private const string VALID_EMAIL = "valid@emailaddress.co.za";

        private readonly Mock<IPasswordHelper> _passwordHelper = new Mock<IPasswordHelper>();

        private readonly Mock<ILoginTokenHelper> _loginTokenHelper = new Mock<ILoginTokenHelper>();

        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();

        private readonly Mock<ISessionRepository> _sessionRepository = new Mock<ISessionRepository>();

        private readonly IUserManager _manager;

        #endregion

        #region Constructors

        public UserManagerTest()
        {
            _manager = new UserManager(_logger.Object, _passwordHelper.Object, _loginTokenHelper.Object, _userRepository.Object);
        }

        #endregion

        #region Test Methods

        [TestMethod]
        [TestCategory("Business - Managers - Users")]
        public void UserManager_UpdateUser_NullArguments_Failure()
        {
            // Act
            BusinessResult<Session> result = _manager.UpdateUser(null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.HasErrors);
            Assert.AreEqual(result.Errors.Count, 1);
            Assert.IsFalse(result.HasResult);
        }

        [TestMethod]
        [TestCategory("Business - Managers - Users")]
        public void UserManager_UpdateUser_UserDetails_UserNameNull_Failure()
        {
            // Arrange
            UserDetails userDetails = UsersDataHelper.GetUserDetails1();
            userDetails.UserName = null;

            // Act
            BusinessResult<Session> result = _manager.UpdateUser(userDetails);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.HasErrors);
            Assert.AreEqual(result.Errors.Count, 1);
            Assert.IsFalse(result.HasResult);
        }

        [TestMethod]
        [TestCategory("Business - Managers - Users")]
        public void UserManager_UpdateUser_UserDetails_UserNotExists_Failure()
        {
            // Arrange
            UserDetails userDetails = UsersDataHelper.GetUserDetails1();

            _userRepository
                .Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns<User>(null)
                .Verifiable();

            // Act
            BusinessResult<Session> result = _manager.UpdateUser(userDetails);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.HasErrors);
            Assert.IsFalse(result.HasResult);
            Assert.IsTrue(result.Errors.Any(x => x.Message == ErrorMessages.UserNotFound.Message));
            _userRepository.Verify();
        }

        [TestMethod]
        [TestCategory("Business - Managers - Users")]
        public void UserManager_UpdateUser_UserDetails_EmailInvalid_Failure()
        {
            // Arrange
            UserDetails userDetails = UsersDataHelper.GetUserDetails1();
            userDetails.Email = "This is invalid";
            User user1 = AccessDataHelper.GetUser1();

            _userRepository
                .Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(user1)
                .Verifiable();

            // Act
            BusinessResult<Session> result = _manager.UpdateUser(userDetails);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.HasErrors);
            Assert.IsFalse(result.HasResult);
            Assert.IsTrue(result.Errors.Any(x => x.Message == ErrorMessages.InvalidEmailAddress.Message));
            _userRepository.Verify();
        }

        [TestMethod]
        [TestCategory("Business - Managers - Users")]
        public void UserManager_UpdateUser_UserDetails_InvalidName_Failure()
        {
            // Arrange
            UserDetails userDetails = UsersDataHelper.GetUserDetails1();
            userDetails.Email = VALID_EMAIL;
            userDetails.Name = "This is invalid 123 &^%$";
            userDetails.Surname = "Valid";
            User user1 = AccessDataHelper.GetUser1();

            _userRepository
                .Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(user1)
                .Verifiable();

            // Act
            BusinessResult<Session> result = _manager.UpdateUser(userDetails);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.HasErrors);
            Assert.IsFalse(result.HasResult);
            Assert.IsTrue(result.Errors.Any(x => x.Message == ErrorMessages.InvalidNameOrSurname.Message));
            _userRepository.Verify();
        }

        [TestMethod]
        [TestCategory("Business - Managers - Users")]
        public void UserManager_UpdateUser_UserDetails_InvalidSurname_Failure()
        {
            // Arrange
            UserDetails userDetails = UsersDataHelper.GetUserDetails1();
            userDetails.Email = VALID_EMAIL;
            userDetails.Name = "Valid";
            userDetails.Surname = "This is invalid 123 &^%$";
            User user1 = AccessDataHelper.GetUser1();

            _userRepository
                .Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(user1)
                .Verifiable();

            // Act
            BusinessResult<Session> result = _manager.UpdateUser(userDetails);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.HasErrors);
            Assert.IsFalse(result.HasResult);
            Assert.IsTrue(result.Errors.Any(x => x.Message == ErrorMessages.InvalidNameOrSurname.Message));
            _userRepository.Verify();
        }

        [TestMethod]
        [TestCategory("Business - Managers - Users")]
        public void UserManager_UpdateUser_UserDetails_EmptyStrings_Failure()
        {
            // Arrange
            UserDetails userDetails = UsersDataHelper.GetUserDetails1();
            userDetails.Email = "";
            userDetails.Name = null;
            userDetails.Surname = " ";
            User user1 = AccessDataHelper.GetUser1();

            _userRepository
                .Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(user1)
                .Verifiable();

            // Act
            BusinessResult<Session> result = _manager.UpdateUser(userDetails);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.HasErrors);
            Assert.IsFalse(result.HasResult);
            Assert.AreEqual(result.Errors.Count, 3);
            _userRepository.Verify();
        }


        [TestMethod]
        [TestCategory("Business - Managers - Users")]
        public void UserManager_UpdateUser_UserDetails_Success()
        {
            // Arrange
            string generatedKey = "generated key";
            UserDetails userDetails = UsersDataHelper.GetUserDetails1();
            userDetails.Email = VALID_EMAIL;
            User user1 = AccessDataHelper.GetUser1();
            Session s1 = AccessDataHelper.GetSession1(user1);
            s1.IsActive = false;
            Thread.Sleep(100);
            Session s2 = AccessDataHelper.GetSession1(user1);
            Thread.Sleep(100);
            Session s3 = AccessDataHelper.GetSession1(user1);

            _userRepository
                .Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(user1)
                .Verifiable();

            _userRepository
                .Setup(x => x.Update(It.IsAny<User>()))
                .Verifiable();

            _loginTokenHelper
                .Setup(x => x.GenerateSecurityToken(It.IsAny<User>()))
                .Returns(generatedKey)
                .Verifiable();

            // Act
            BusinessResult<Session> result = _manager.UpdateUser(userDetails);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.HasResult);
            Assert.IsFalse(result.HasErrors);
            Assert.IsTrue(s3.IsActive);
            Assert.AreEqual(s3.SessionKey, generatedKey);
            _userRepository.Verify();
            _loginTokenHelper.Verify();
        }
        
        #endregion
    }
}
