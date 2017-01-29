using CLog.Framework.Business.Contracts;
using CLog.Framework.Business.Models.Results;
using CLog.Models.Access;
using CLog.Models.Users;

namespace CLog.Business.Contracts.Users
{
    /// <summary>
    /// Represents the Users business manager contract.
    /// </summary>
    /// <seealso cref="CLog.Framework.Business.Contracts.IBusinessManager" />
    public interface IUserManager : IBusinessManager
    {
        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="userDetails">The user.</param>
        /// <returns>
        /// The business result.
        /// </returns>
        BusinessResult<Session> UpdateUser(UserDetails userDetails);
    }
}
