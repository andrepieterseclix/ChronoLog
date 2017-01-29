using CLog.Framework.Services.Contracts;
using CLog.Services.Models.Users.DataTransfer;
using System.ServiceModel;

namespace CLog.Services.Contracts.Users
{
    /// <summary>
    /// Represents the user service contract.
    /// </summary>
    /// <seealso cref="CLog.Framework.Services.Contracts.IService" />
    [ServiceContract]
    public interface IUserService : IService
    {
        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The update user response.</returns>
        [OperationContract]
        UpdateUserResponse UpdateUser(UpdateUserRequest request);
    }
}
