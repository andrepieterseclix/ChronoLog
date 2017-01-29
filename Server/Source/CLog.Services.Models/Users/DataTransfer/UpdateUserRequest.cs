using CLog.Framework.Services.Models;
using CLog.Services.Models.Access;
using System;
using System.Runtime.Serialization;

namespace CLog.Services.Models.Users.DataTransfer
{
    /// <summary>
    /// Represents the update user request model.
    /// </summary>
    /// <seealso cref="CLog.Framework.Services.Models.RequestBase" />
    [DataContract]
    public sealed class UpdateUserRequest : RequestBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserRequest" /> class.
        /// </summary>
        /// <param name="userDetails">The user.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public UpdateUserRequest(UserDetailsDto userDetails)
        {
            if (userDetails == null)
                throw new ArgumentNullException(nameof(userDetails));

            UserDetails = userDetails;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the user details.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        [DataMember(IsRequired = true)]
        public UserDetailsDto UserDetails { get; set; }

        #endregion
    }
}
