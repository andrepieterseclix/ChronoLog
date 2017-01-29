using CLog.DataAccess.Contracts.Repositories.Access;
using CLog.DataAccess.Repositories.Access;
using CLog.Models.Access;
using CLog.Models.Mocks.Access;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CLog.DataAccess.Tests.Repositories
{
    [TestClass]
    public class GenericSessionRepositoryTest
    {
        #region Fields

        private readonly DataContext _dataContext;

        private readonly IUserRepository _userRepository;

        private readonly ISessionRepository _repository;

        #endregion

        #region Constructors, Initialisation and Cleanup

        public GenericSessionRepositoryTest()
        {
            _dataContext = new DataContext();
            _userRepository = new UserRepository(_dataContext);
            _repository = new SessionRepository(_dataContext);
        }

        [TestInitialize]
        public void Init()
        {
            User user = AccessDataHelper.GetUser2();
            if (_userRepository.Count(x => x.UserName == user.UserName) == 0)
                _userRepository.Add(user);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (_dataContext != null)
                _dataContext.Dispose();
        }

        #endregion

        #region Test Methods

        [TestMethod]
        [TestCategory("DataAccess - Repository (Generic) - Session")]
        public void SessionRepository_NavigationProperty_Add_DoNotSaveExistingEntity_Success()
        {
            // Arrange
            User user = _userRepository.Get(x => x.UserName == AccessDataHelper.USER_NAME2);
            Session session = AccessDataHelper.GetSession1(user);

            // Act
            _repository.Add(session);

            // Assert
            Assert.AreNotEqual(session.Id, default(long));
            Assert.IsNotNull(session.User);
            Assert.AreEqual(session.UserId, user.Id);
            Assert.AreEqual(session.UserId, session.User.Id);
        }

        [TestMethod]
        [TestCategory("DataAccess - Repository (Generic) - Session")]
        public void SessionRepository_NavigationProperty_AddMultipleChildren_Success()
        {
            // Arrange
            User user = _userRepository.Get(x => x.UserName == AccessDataHelper.USER_NAME2);
            int newSessions = 2, existingSessions = user.Sessions.Count;

            for (int i = 0; i < newSessions; i++)
            {
                Session session = AccessDataHelper.GetSession1(user, false);
                user.Sessions.Add(session);
            }

            // Act
            _userRepository.Update(user);

            // Assert
            User dbUser = _userRepository.Get(x => x.Id == user.Id);
            Assert.IsNotNull(dbUser);
            Assert.AreEqual(dbUser.Sessions.Count, existingSessions + newSessions);
        }

        [TestMethod]
        [TestCategory("DataAccess - Repository (Generic) - Session")]
        public void SessionRepository_NavigationProperty_Clear_Success()
        {
            // Arrange
            User user = _userRepository.Get(x => x.UserName == AccessDataHelper.USER_NAME2);

            if(user.Sessions.Count < 1)
            {
                user.Sessions.Add(AccessDataHelper.GetSession1(user));
                _userRepository.Update(user);
            }

            User dbUser = _userRepository.Get(x => x.Id == user.Id);

            // Act
            //dbUser.Sessions.Clear();
            //_userRepository.Update(dbUser);
            var sessions = dbUser.Sessions.ToList();
            foreach (var session in sessions)
            {
                _repository.Delete(session);
            }

            // Assert
            User dbUser2 = _userRepository.Get(x => x.Id == user.Id);
            Assert.IsNotNull(dbUser2);
            Assert.AreEqual(dbUser2.Sessions.Count, 0);
        }

        #endregion
    }
}
