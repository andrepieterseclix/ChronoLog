using CLog.ServiceClients.Security;
using CLog.UI.Common.Business;
using CLog.UI.Models.Access;

namespace CLog.UI.Main.Managers
{
    /// <summary>
    /// Represents the Access UI Business Manager contract.
    /// </summary>
    public interface IAccessManager
    {
        /// <summary>
        /// Logins the specified user name.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// The business result.
        /// </returns>
        BusinessResult<LoginResult> Login(string userName, string password);

        /// <summary>
        /// Logs out the specified principal.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>The business result.</returns>
        BusinessResult Logout(ClientPrincipal principal);
    }
}