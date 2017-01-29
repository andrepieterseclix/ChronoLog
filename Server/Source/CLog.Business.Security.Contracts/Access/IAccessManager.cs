using CLog.Framework.Business.Contracts;
using CLog.Framework.Business.Models.Results;
using CLog.Models.Access;
using System;

namespace CLog.Business.Security.Contracts.Access
{
    /// <summary>
    /// Represents the Access business manager contract.
    /// </summary>
    /// <seealso cref="CLog.Framework.Business.Contracts.IBusinessManager" />
    public interface IAccessManager : IBusinessManager
    {
        #region Methods

        /// <summary>
        /// Logs in the specified user if the correct password was specified.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns>The business result with a <see cref="Session"/> if the login was successful.</returns>
        BusinessResult<Session> Login(string userName, string password);

        /// <summary>
        /// Logout the user associated with the session specified.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>
        /// The business result.
        /// </returns>
        BusinessResult Logout(string userName);

        /// <summary>
        /// Validates the session.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="sessionKey">The session key.</param>
        /// <returns>The business result.</returns>
        BusinessResult<SessionState> ValidateSession(string userName, Guid sessionId, string sessionKey);

        /// <summary>
        /// Updates the password.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="oldPassword">The old password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns></returns>
        BusinessResult UpdatePassword(string userName, string oldPassword, string newPassword);

        #endregion
    }
}
