using System.Runtime.Serialization;

namespace CLog.Services.Models.Access
{
    /// <summary>
    /// Represents the User password data transfer object.
    /// </summary>
    [DataContract]
    public sealed class UserPasswordDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPasswordDto" /> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="oldPassword">The old password.</param>
        /// <param name="newPassword">The password.</param>
        public UserPasswordDto(string userName, string oldPassword, string newPassword)
        {
            UserName = userName;
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        [DataMember(IsRequired = true)]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the old password.
        /// </summary>
        /// <value>
        /// The old password.
        /// </value>
        [DataMember(IsRequired = true)]
        public string OldPassword { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [DataMember(IsRequired = true)]
        public string NewPassword { get; set; }

        #endregion
    }
}
