using CLog.UI.Common.Business;
using CLog.UI.Models.Access;

namespace CLog.UI.UserProfile.Managers
{
    /// <summary>
    /// Repersents the UI User Manager contract.
    /// </summary>
    public interface IUserManager
    {
        UIBusinessResult<SessionInfo> UpdateUser(User user);
    }
}