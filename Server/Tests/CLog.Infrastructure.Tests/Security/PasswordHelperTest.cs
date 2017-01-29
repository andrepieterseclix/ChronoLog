using CLog.Infrastructure.Contracts.Security;
using CLog.Infrastructure.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CLog.Business.Tests.Helpers
{
    [TestClass]
    public class PasswordHelperTest
    {
        #region Fields

        private readonly IPasswordHelper _passwordHelper;

        #endregion

        #region Constructors

        public PasswordHelperTest()
        {
            _passwordHelper = new PasswordHelperSha256();
        }

        #endregion

        #region Test Methods

        [TestMethod]
        [TestCategory("Infrastructure - Security")]
        public void PasswordHelper_GetRandomSalt_Generate_Success()
        {
            // Act
            string salt = _passwordHelper.GetRandomSalt();

            // Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(salt));
        }

        [TestMethod]
        [TestCategory("Infrastructure - Security")]
        public void PasswordHelper_GetRandomSalt_Compare_Success()
        {
            // Act
            string salt1 = _passwordHelper.GetRandomSalt();
            string salt2 = _passwordHelper.GetRandomSalt();

            // Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(salt1));
            Assert.IsFalse(string.IsNullOrWhiteSpace(salt2));
            Assert.AreNotEqual(salt1, salt2);
        }

        [TestMethod]
        [TestCategory("Infrastructure - Security")]
        public void PasswordHelper_ComputeHash_Success()
        {
            // Arrange
            string password = "P@ssw0rd123";
            string salt = "SaLt";

            // Act
            string hash = _passwordHelper.ComputeHash(password, salt);

            // Assert
            Assert.IsNotNull(hash);
        }

        [TestMethod]
        [TestCategory("Infrastructure - Security")]
        public void PasswordHelper_ComputeHash_SameHashProduced_Success()
        {
            // Arrange
            string password = "P@ssw0rd123";
            string salt = "SaLt";

            // Act
            string hash1 = _passwordHelper.ComputeHash(password, salt);
            string hash2 = _passwordHelper.ComputeHash(password, salt);

            // Assert
            Assert.IsNotNull(hash1);
            Assert.IsNotNull(hash2);
            Assert.AreEqual(hash1, hash2);
        }

        [TestMethod]
        [TestCategory("Infrastructure - Security")]
        public void PasswordHelper_ComputeHash_DifferentSaltHashCompare_Success()
        {
            // Arrange
            string password = "P@ssw0rd123";
            string salt1 = "SaLt1";
            string salt2 = "SaLt2";

            // Act
            string hash1 = _passwordHelper.ComputeHash(password, salt1);
            string hash2 = _passwordHelper.ComputeHash(password, salt2);

            // Assert
            Assert.IsNotNull(hash1);
            Assert.IsNotNull(hash2);
            Assert.AreNotEqual(hash1, hash2);
        }

        #endregion
    }
}
