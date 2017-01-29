namespace CLog.Models.Access
{
    /// <summary>
    /// Represents the Session State business domain model.
    /// </summary>
    public class SessionState
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is expired.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is expired; otherwise, <c>false</c>.
        /// </value>
        public bool IsExpired { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public long UserId { get; set; }

        #endregion
    }
}
