using CLog.Framework.Services.Contracts;
using CLog.Services.Models.Access.DataTransfer;
using System.ServiceModel;

namespace CLog.Services.Security.Contracts.Access
{
    /// <summary>
    /// Represents the access service contract.
    /// </summary>
    [ServiceContract]
    public interface IAccessService : IService
    {
        /// <summary>
        /// Performs the login request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The login response.</returns>
        [OperationContract]
        LoginResponse Login(LoginRequest request);

        /// <summary>
        /// Performs the Logout request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The logout response.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        [OperationContract]
        LogoutResponse Logout(LogoutRequest request);

        /// <summary>
        /// Updates the user password.
        /// </summary>
        /// <param name="UpdateUserPasswordRequest">The update user password request.</param>
        /// <returns>The update user password response.</returns>
        [OperationContract]
        UpdateUserPasswordResponse UpdateUserPassword(UpdateUserPasswordRequest request);
    }
}
