using CLog.Framework.Models;
using CLog.Models.Access;
using CLog.Models.Users;

namespace CLog.Models.Mocks.Access
{
    public static class AccessDataHelper
    {
        #region Fields

        public const string USER_NAME1 = "userName1";

        public const string USER_NAME2 = "userName2";

        public const string USER_NAME3 = "userName3";
        #endregion

        #region Methods

        public static User GetUser1()
        {
            User user = User.New(DataState.Active, USER_NAME1, "Password", "Salt", "Name", "Surname", "Email", null);

            return user;
        }

        public static User GetUser2()
        {
            User user = User.New(DataState.Active, USER_NAME2, "Password", "Salt", "Name", "Surname", "Email", null);

            return user;
        }
        
        public static Session GetSession1(User user, bool addSessionToUser = true)
        {
            Session session = Session.New("Session Key", user);
            if (addSessionToUser)
                user.Sessions.Add(session);

            return session;
        }

        public static UserPassword GetUserPassword1()
        {
            return new UserPassword(AccessDataHelper.USER_NAME1, "Old Password", "New Password");
        }

        #endregion
    }
}
