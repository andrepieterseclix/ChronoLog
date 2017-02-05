namespace CLog.UI.Models.Access
{
    /// <summary>
    /// Represents the login result.
    /// </summary>
    public sealed class LoginResult
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginResult" /> class.
        /// </summary>
        /// <param name="isLoggedIn">if set to <c>true</c> [is logged in].</param>
        public LoginResult(bool isLoggedIn)
        {
            IsLoggedIn = isLoggedIn;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this instance is logged in.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is logged in; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoggedIn { get; private set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public User User { get; set; }

        #endregion
    }
}
