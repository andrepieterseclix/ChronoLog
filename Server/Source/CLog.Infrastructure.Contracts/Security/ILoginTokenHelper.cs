using CLog.Models.Access;

namespace CLog.Infrastructure.Contracts.Security
{
    /// <summary>
    /// Represents the token helper contract.
    /// </summary>
    public interface ILoginTokenHelper
    {
        /// <summary>
        /// Generates the security token.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The security token.</returns>
        string GenerateSecurityToken(User user);

        /// <summary>
        /// Verifies the security token.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="sessionExpired">if set to <c>true</c> [session expired].</param>
        /// <returns>
        /// returns <c>true</c> if the security token associated with the session is valid.
        /// </returns>
        bool VerifySecurityToken(Session session, out bool sessionExpired);
    }
}
