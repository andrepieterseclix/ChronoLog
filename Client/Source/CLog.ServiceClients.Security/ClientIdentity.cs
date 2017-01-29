using System;
using System.Security.Principal;

namespace CLog.ServiceClients.Security
{
    /// <summary>
    /// Represents the user identity that identifies the current user.
    /// </summary>
    /// <seealso cref="System.Security.Principal.IIdentity" />
    public class ClientIdentity : IIdentity
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientIdentity" /> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="name">The name.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="sessionKey">The session key.</param>
        /// <param name="roles">The roles.</param>
        public ClientIdentity(string userName, string name, Guid sessionId, string sessionKey, string[] roles)
        {
            UserName = userName;
            Name = name;
            SessionId = sessionId;
            SessionKey = sessionKey;
            Roles = roles;
        }

        #endregion

        #region Properties

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
                    !string.IsNullOrWhiteSpace(UserName) &&
                    !string.IsNullOrWhiteSpace(SessionKey) &&
                    SessionId != Guid.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; private set; }

        /// <summary>
        /// Gets or sets the full name of the user.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public string Name { get; private set; }

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

        #region Methods

        /// <summary>
        /// Updates the session.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="sessionKey">The session key.</param>
        public void UpdateSession(Guid sessionId, string sessionKey)
        {
            SessionId = sessionId;
            SessionKey = sessionKey;
        }

        #endregion
    }
}
