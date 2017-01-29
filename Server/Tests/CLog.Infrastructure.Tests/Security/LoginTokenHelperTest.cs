using CLog.Infrastructure.Contracts.Security;
using CLog.Infrastructure.Security;
using CLog.Models.Access;
using CLog.Models.Mocks.Access;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace CLog.Infrastructure.Tests.Security
{
    [TestClass]
    public class LoginTokenHelperTest
    {
        #region Fields

        private const int EXPIRY = 1000;

        private readonly ILoginTokenHelper _loginTokenHelper;

        #endregion

        #region Constructors

        public LoginTokenHelperTest()
        {
            _loginTokenHelper = new LoginTokenHelper(TimeSpan.FromMilliseconds(EXPIRY));
        }

        #endregion

        #region Test Methods

        [TestMethod]
        [TestCategory("Infrastructure - Security")]
        public void LoginTokenHelper_GenerateSecurityToken_Success()
        {
            // Arrange
            User user = AccessDataHelper.GetUser1();

            // Act
            string token = _loginTokenHelper.GenerateSecurityToken(user);

            // Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(token));
        }

        [TestMethod]
        [TestCategory("Infrastructure - Security")]
        public void LoginTokenHelper_VerifySecurityToken_Success()
        {
            // Arrange
            User user = AccessDataHelper.GetUser1();
            Session session = AccessDataHelper.GetSession1(user);
            session.SessionKey = _loginTokenHelper.GenerateSecurityToken(user);

            // Act
            bool sessionExpired;
            bool isValid = _loginTokenHelper.VerifySecurityToken(session, out sessionExpired);

            // Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("Infrastructure - Security")]
        public void LoginTokenHelper_VerifySecurityToken_Expire_Fail()
        {
            // Arrange
            User user = AccessDataHelper.GetUser1();
            Session session = AccessDataHelper.GetSession1(user);
            session.SessionKey = _loginTokenHelper.GenerateSecurityToken(user);

            // Act
            Thread.Sleep(EXPIRY + 100);
            bool sessionExpired;
            bool isValid = _loginTokenHelper.VerifySecurityToken(session, out sessionExpired);

            // Assert
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        [TestCategory("Infrastructure - Security")]
        public void LoginTokenHelper_VerifySecurityToken_ChangeUserName_Fail()
        {
            // Arrange
            User user = AccessDataHelper.GetUser1();
            Session session = AccessDataHelper.GetSession1(user);
            session.SessionKey = _loginTokenHelper.GenerateSecurityToken(user);

            // Act
            user.UserName = "Changed UserName should invalidate the token";
            bool sessionExpired;
            bool isValid = _loginTokenHelper.VerifySecurityToken(session, out sessionExpired);

            // Assert
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        [TestCategory("Infrastructure - Security")]
        public void LoginTokenHelper_VerifySecurityToken_ChangeName_Fail()
        {
            // Arrange
            User user = AccessDataHelper.GetUser1();
            Session session = AccessDataHelper.GetSession1(user);
            session.SessionKey = _loginTokenHelper.GenerateSecurityToken(user);

            // Act
            user.Name = "Changed Name should invalidate the token";
            bool sessionExpired;
            bool isValid = _loginTokenHelper.VerifySecurityToken(session, out sessionExpired);

            // Assert
            Assert.IsFalse(isValid);
        }

        #endregion
    }
}
