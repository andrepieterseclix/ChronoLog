using System;
using System.Security.Principal;

namespace CLog.Framework.Security
{
    /// <summary>
    /// Represents the user identity that identifies the current user.
    /// </summary>
    /// <seealso cref="System.Security.Principal.IIdentity" />
    public class ServerIdentity : IIdentity
    {
        #region Fields

        private readonly Guid _scopeId;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerIdentity" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="sessionKey">The session key.</param>
        /// <param name="sessionExpired">if set to <c>true</c> [session expired].</param>
        /// <param name="roles">The roles.</param>
        public ServerIdentity(string name, long userId, Guid sessionId, string sessionKey, bool sessionExpired, string[] roles)
        {
            _scopeId = Guid.NewGuid();

            Name = name;
            UserId = userId;
            SessionId = sessionId;
            SessionKey = sessionKey;
            SessionExpired = sessionExpired;
            Roles = roles;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the scope identifier that uniquely identifies each <see cref="ServerIdentity"/> instance.  This can typically be used to identify
        /// the transaction scope, as each service call will have a <see cref="ServerIdentity"/> associated with the thread that is created for it.
        /// </summary>
        /// <value>
        /// The scope identifier.
        /// </value>
        public Guid ScopeId
        {
            get { return _scopeId; }
        }

        /// <summary>
        /// Gets the type of authentication used.
        /// </summary>
        public string AuthenticationType
        {
            get { return "Custom Authentication"; }
        }

        /// <summary>
        /// Gets a value that indicates whether the user has been authenticated.
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return
                    !string.IsNullOrWhiteSpace(Name) &&
                    UserId != default(long) &&
                    !string.IsNullOrWhiteSpace(SessionKey) &&
                    SessionId != Guid.Empty;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the session has expired.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the session has expired; otherwise, <c>false</c>.
        /// </value>
        public bool SessionExpired { get; private set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public long UserId { get; private set; }

        /// <summary>
        /// Gets the session identifier.
        /// </summary>
        /// <value>
        /// The session identifier.
        /// </value>
        public Guid SessionId { get; private set; }

        /// <summary>
        /// Gets the session key.
        /// </summary>
        /// <value>
        /// The session key.
        /// </value>
        public string SessionKey { get; private set; }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public string[] Roles { get; private set; }

        #endregion
    }
}
