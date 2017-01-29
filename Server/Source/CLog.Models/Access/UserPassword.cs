using System;

namespace CLog.Models.Access
{
    /// <summary>
    /// Represents the user Password transient model.
    /// </summary>
    public class UserPassword
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPassword" /> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="oldPassword">The old password.</param>
        /// <param name="newPassword">The password.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public UserPassword(string userName, string oldPassword, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException(nameof(userName));

            if (string.IsNullOrWhiteSpace(oldPassword))
                throw new ArgumentNullException(nameof(oldPassword));

            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentNullException(nameof(newPassword));

            UserName = userName;
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the old password.
        /// </summary>
        /// <value>
        /// The old password.
        /// </value>
        public string OldPassword { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string NewPassword { get; set; }

        #endregion
    }
}
