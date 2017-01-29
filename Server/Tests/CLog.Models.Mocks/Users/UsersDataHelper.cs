using CLog.Models.Mocks.Access;
using CLog.Models.Users;

namespace CLog.Models.Mocks.Users
{
    public static class UsersDataHelper
    {
        #region Methods

        public static UserDetails GetUserDetails1()
        {
            return new UserDetails(AccessDataHelper.USER_NAME1, "Name", "Surname", "Email");
        }

        #endregion
    }
}
