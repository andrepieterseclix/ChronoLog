using CLog.Framework.Models;
using System;

namespace CLog.Models.Access
{
    /// <summary>
    /// Represents the Session business domain model.
    /// </summary>
    /// <seealso cref="CLog.Models.BusinessModel" />
    public class Session : BusinessModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Session"/> class.
        /// </summary>
        public Session()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Session" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="refId">The reference identifier.</param>
        /// <param name="sessionKey">The session key.</param>
        /// <param name="loginTimeUtc">The login time UTC.</param>
        /// <param name="isActive">if set to <c>true</c> [is active].</param>
        /// <param name="user">The user.</param>
        public Session(long id, Guid refId, string sessionKey, DateTime loginTimeUtc, bool isActive, User user)
            : base(id)
        {
            RefId = refId;
            SessionKey = sessionKey;
            LoginTimeUtc = loginTimeUtc;
            LastActiveUtc = loginTimeUtc;
            IsActive = isActive;
            User = user;
            UserId = user?.Id ?? default(long);
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

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the login time.
        /// </summary>
        /// <value>
        /// The login time.
        /// </value>
        public DateTime LoginTimeUtc { get; set; }

        /// <summary>
        /// Gets or sets the last active UTC time.
        /// </summary>
        /// <value>
        /// The last active UTC time.
        /// </value>
        public DateTime LastActiveUtc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public virtual User User { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new instance of the <see cref="Session"/> class.
        /// </summary>
        /// <param name="sessionKey">The session key.</param>
        /// <param name="user">The user.</param>
        /// <returns>A new session.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static Session New(string sessionKey, User user)
        {
            if (string.IsNullOrWhiteSpace(sessionKey))
                throw new ArgumentNullException(nameof(sessionKey));

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return new Session(0, Guid.NewGuid(), sessionKey, DateTime.UtcNow, true, user);
        }

        #endregion
    }
}
