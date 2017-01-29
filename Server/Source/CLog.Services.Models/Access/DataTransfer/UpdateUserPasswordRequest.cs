using CLog.Framework.Services.Models;
using System;
using System.Runtime.Serialization;

namespace CLog.Services.Models.Access.DataTransfer
{
    /// <summary>
    /// Represents the update user password request model.
    /// </summary>
    [DataContract]
    public sealed class UpdateUserPasswordRequest : RequestBase
    {
        #region Constructors
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserRequest" /> class.
        /// </summary>
        /// <param name="userPassword">The user password.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public UpdateUserPasswordRequest(UserPasswordDto userPassword)
        {
            if (userPassword == null)
                throw new ArgumentNullException(nameof(userPassword));

            UserPassword = userPassword;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the user password (optional).
        /// </summary>
        /// <value>
        /// The user password.
        /// </value>
        [DataMember(IsRequired = true)]
        public UserPasswordDto UserPassword { get; set; }

        #endregion
    }
}
