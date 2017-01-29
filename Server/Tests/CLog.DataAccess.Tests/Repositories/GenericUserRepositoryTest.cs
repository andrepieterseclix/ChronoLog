using CLog.DataAccess.Contracts.Repositories.Access;
using CLog.DataAccess.Repositories.Access;
using CLog.Models.Access;
using CLog.Models.Mocks.Access;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace CLog.DataAccess.Tests.Repositories
{
    [TestClass]
    public class GenericUserRepositoryTest
    {
        #region Fields

        private readonly IUserRepository _repository;

        #endregion

        #region Constructors, Initialisation and Cleanup

        public GenericUserRepositoryTest()
        {
            _repository = new UserRepository();
        }

        [TestInitialize]
        public void Init()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"]?.ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("DELETE [User] WHERE UserName = @userName", conn);
            cmd.Parameters.AddWithValue("@userName", AccessDataHelper.USER_NAME1);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (_repository != null)
                _repository.Dispose();
        }

        #endregion

        #region Test Methods

        [TestMethod]
        [TestCategory("DataAccess - Repository (Generic) - Users")]
        public void UserRepository_Add_Success()
        {
            // Arrange
            User user = AccessDataHelper.GetUser1();

            // Act
            _repository.Add(user);

            // Assert
            Assert.IsNotNull(user);
            Assert.AreNotEqual(user.Id, default(long));
        }

        [TestMethod]
        [TestCategory("DataAccess - Repository (Generic) - Users")]
        public void UserRepository_Get_Success()
        {
            // Arrange
            User newUser = AccessDataHelper.GetUser1();
            _repository.Add(newUser);

            // Act
            User user = _repository.Get(x => x.Id == newUser.Id);

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(user.Id, newUser.Id);
            Assert.AreEqual(user.UserName, newUser.UserName);
            Assert.AreEqual(user.Password, newUser.Password);
            Assert.AreEqual(user.Name, newUser.Name);
            Assert.AreEqual(user.Surname, newUser.Surname);
            Assert.AreEqual(user.Email, newUser.Email);
            Assert.AreEqual(user.ManagerId, newUser.ManagerId);
            Assert.AreEqual(user.Manager, newUser.Manager);
        }

        [TestMethod]
        [TestCategory("DataAccess - Repository (Generic) - Users")]
        public void UserRepository_GetAll_All_Success()
        {
            // Arrange
            User newUser = AccessDataHelper.GetUser1();
            _repository.Add(newUser);

            // Act
            IEnumerable<User> users = _repository.GetAll();

            // Assert
            Assert.IsNotNull(users);
            Assert.IsTrue(users.Count() >= 1);
        }

        [TestMethod]
        [TestCategory("DataAccess - Repository (Generic) - Users")]
        public void UserRepository_Count_All_Success()
        {
            // Arrange
            User newUser = AccessDataHelper.GetUser1();
            _repository.Add(newUser);

            // Act
            long count = _repository.Count();

            // Assert
            Assert.IsTrue(count >= 1);
        }

        [TestMethod]
        [TestCategory("DataAccess - Repository (Generic) - Users")]
        public void UserRepository_Count_Filtered_Success()
        {
            // Arrange
            User newUser = AccessDataHelper.GetUser1();
            _repository.Add(newUser);

            // Act
            long count = _repository.Count(x => x.UserName == AccessDataHelper.USER_NAME1);

            // Assert
            Assert.AreEqual(count, 1);
        }

        [TestMethod]
        [TestCategory("DataAccess - Repository (Generic) - Users")]
        public void UserRepository_GetAll_Filtered_Success()
        {
            // Arrange
            User newUser = AccessDataHelper.GetUser1();
            _repository.Add(newUser);

            // Act
            IEnumerable<User> users = _repository.GetAll(x => x.UserName == AccessDataHelper.USER_NAME1);

            // Assert
            Assert.IsNotNull(users);
            Assert.AreEqual(users.Count(), 1);
        }

        [TestMethod]
        [TestCategory("DataAccess - Repository (Generic) - Users")]
        public void UserRepository_Update_Success()
        {
            // Arrange
            User newUser = AccessDataHelper.GetUser1();
            _repository.Add(newUser);
            string updatedSurname = "NewSurname";

            // Act
            User user = _repository.Get(x => x.Id == newUser.Id);
            user.Surname = updatedSurname;
            _repository.Update(user);

            User updatedUser = _repository.Get(x => x.Id == user.Id);

            // Assert
            Assert.AreEqual(user.Id, newUser.Id);
            Assert.AreEqual(user.Id, updatedUser.Id);
            Assert.AreEqual(updatedUser.Surname, updatedSurname);
        }

        [TestMethod]
        [TestCategory("DataAccess - Repository (Generic) - Users")]
        public void UserRepository_Delete_Success()
        {
            // Arrange
            User user = AccessDataHelper.GetUser1();
            _repository.Add(user);

            // Act
            _repository.Delete(user);

            User deletedUser = _repository.Get(x => x.Id == user.Id);

            // Assert
            Assert.IsNull(deletedUser);
        }
        
        #endregion
    }
}
