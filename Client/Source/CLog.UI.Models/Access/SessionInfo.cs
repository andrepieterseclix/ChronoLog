using System;

namespace CLog.UI.Models.Access
{
    /// <summary>
    /// Represents the Session Information model.
    /// </summary>
    public sealed class SessionInfo
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionInfo"/> class.
        /// </summary>
        /// <param name="refId">The reference identifier.</param>
        /// <param name="sessionKey">The session key.</param>
        /// <exception cref="System.ArgumentException">
        /// The reference id can not be empty!
        /// or
        /// The session key can not be empty!
        /// </exception>
        public SessionInfo(Guid refId, string sessionKey)
        {
            if (refId == Guid.Empty)
                throw new ArgumentException("The reference id can not be empty!", nameof(refId));

            if (string.IsNullOrWhiteSpace(sessionKey))
                throw new ArgumentException("The session key can not be empty!", nameof(sessionKey));

            RefId = refId;
            SessionKey = sessionKey;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the reference identifier.
        /// </summary>
        /// <value>
        /// The reference identifier.
        /// </value>
        public Guid RefId { get; set; }

        /// <summary>
        /// Gets or sets the session key.
        /// </summary>
        /// <value>
        /// The session key.
        /// </value>
        public string SessionKey { get; set; }

        #endregion
    }
}
