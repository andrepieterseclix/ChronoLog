using CLog.DataAccess.Contracts.Repositories.Access;
using CLog.DataAccess.Contracts.Repositories.Timesheets;
using CLog.DataAccess.Repositories.Access;
using CLog.DataAccess.Repositories.Timesheets;
using CLog.Models.Access;
using CLog.Models.Mocks.Access;
using CLog.Models.Mocks.Timesheets;
using CLog.Models.Timesheets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CLog.DataAccess.Tests.Repositories
{
    [TestClass]
    public class GenericCapturedTimeRepositoryTest
    {
        #region Fields

        private readonly ICapturedTimeRepository _repository;

        private readonly IUserRepository _userRepository;

        private readonly string _connectionString;

        #endregion

        #region Constructors, Initialisation and Cleanup

        public GenericCapturedTimeRepositoryTest()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"]?.ConnectionString;

            _repository = new CapturedTimeRepository();
            _userRepository = new UserRepository();
        }

        [TestInitialize]
        public void Init()
        {
            SqlConnection conn = new SqlConnection(_connectionString);
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
            if (_userRepository != null)
                _userRepository.Dispose();
        }

        #endregion

        #region Test Methods

        [TestMethod]
        [TestCategory("DataAccess - Repository (Generic) - Captured Time")]
        public void GenericRepository_ManualCommit_Uncommitted_Success()
        {
            // Arrange
            int days = 3;
            _repository.ManualCommit = false;
            _userRepository.ManualCommit = false;

            User user = AccessDataHelper.GetUser1();
            DateTime from = new DateTime(2017, 1, 2);

            List<CapturedTime> items1 = TimesheetsDataHelper
                .GetCapturedTimeItemsConsecutive(user, from, days)
                .ToList();

            items1.ForEach(x => user.CapturedTimeItems.Add(x));
            _userRepository.Add(user);

            // Act
            _repository.ManualCommit = true;
            CapturedTime[] items = user.CapturedTimeItems.ToArray();

            // update
            CapturedTime updateItem = items.Last();
            byte originalHoursWorked = updateItem.HoursWorked;
            updateItem.HoursWorked = 3;
            _repository.Save(updateItem);

            // insert
            CapturedTime addItem = CapturedTime.New(user, updateItem.Date.AddDays(1), 6);
            _repository.Save(addItem);

            DataTable itemsTable = GetCapturedTime(user.Id);

            // Assert
            Assert.AreEqual(itemsTable.Rows.Count, days);
            Assert.AreEqual(itemsTable.DefaultView[itemsTable.Rows.Count - 1]["HoursWorked"], originalHoursWorked);
        }

        [TestMethod]
        [TestCategory("DataAccess - Repository (Generic) - Captured Time")]
        public void GenericRepository_ManualCommit_Uncommitted_EvenAfterDisposedDataContext_Success()
        {
            // Arrange
            int days = 3;
            _repository.ManualCommit = false;
            _userRepository.ManualCommit = false;

            User user = AccessDataHelper.GetUser1();
            DateTime from = new DateTime(2017, 1, 2);

            List<CapturedTime> items1 = TimesheetsDataHelper
                .GetCapturedTimeItemsConsecutive(user, from, days)
                .ToList();

            items1.ForEach(x => user.CapturedTimeItems.Add(x));
            _userRepository.Add(user);

            // Act
            _repository.ManualCommit = true;
            CapturedTime[] items = user.CapturedTimeItems.ToArray();

            // update
            CapturedTime updateItem = items.Last();
            byte originalHoursWorked = updateItem.HoursWorked;
            updateItem.HoursWorked = 3;
            _repository.Save(updateItem);

            // insert
            CapturedTime addItem = CapturedTime.New(user, updateItem.Date.AddDays(1), 6);
            _repository.Save(addItem);

            _repository.Dispose();

            DataTable itemsTable = GetCapturedTime(user.Id);

            // Assert
            Assert.AreEqual(itemsTable.Rows.Count, days);
            Assert.AreEqual(itemsTable.DefaultView[itemsTable.Rows.Count - 1]["HoursWorked"], originalHoursWorked);
        }

        [TestMethod]
        [TestCategory("DataAccess - Repository (Generic) - Captured Time")]
        public void GenericRepository_ManualCommit_Committed_Success()
        {
            // Arrange
            int days = 3;
            _repository.ManualCommit = false;
            _userRepository.ManualCommit = false;

            User user = AccessDataHelper.GetUser1();
            DateTime from = new DateTime(2017, 1, 2);

            List<CapturedTime> items1 = TimesheetsDataHelper
                .GetCapturedTimeItemsConsecutive(user, from, days)
                .ToList();

            items1.ForEach(x => user.CapturedTimeItems.Add(x));
            _userRepository.Add(user);

            // Act
            _repository.ManualCommit = true;
            CapturedTime[] items = user.CapturedTimeItems.ToArray();

            // update
            CapturedTime updateItem = items.Last();
            byte originalHoursWorked = updateItem.HoursWorked;
            byte updateHoursWorked = 3;
            updateItem.HoursWorked = updateHoursWorked;
            _repository.Save(updateItem);

            // insert
            byte addHoursWorked = 6;
            CapturedTime addItem = CapturedTime.New(user, updateItem.Date.AddDays(1), addHoursWorked);
            _repository.Save(addItem);

            _repository.SaveChanges();

            DataTable itemsTable = GetCapturedTime(user.Id);

            // Assert
            Assert.AreEqual(itemsTable.Rows.Count, days + 1);
            Assert.AreEqual(itemsTable.DefaultView[itemsTable.Rows.Count - 2]["HoursWorked"], updateHoursWorked);
            Assert.AreEqual(itemsTable.DefaultView[itemsTable.Rows.Count - 1]["HoursWorked"], addHoursWorked);
        }

        #endregion

        #region Helper Methods

        private DataTable GetCapturedTime(long userId)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM [CapturedTime] WHERE UserId = @userName", conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            cmd.Parameters.AddWithValue("@userName", userId);

            DataTable tbl = new DataTable();
            adapter.Fill(tbl);

            return tbl;
        }

        #endregion
    }
}
