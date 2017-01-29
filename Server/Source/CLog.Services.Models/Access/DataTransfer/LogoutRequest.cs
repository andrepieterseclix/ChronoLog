using CLog.Framework.Services.Models;
using System;
using System.Runtime.Serialization;

namespace CLog.Services.Models.Access.DataTransfer
{
    /// <summary>
    /// Represents the Logout request model.
    /// </summary>
    /// <seealso cref="CLog.Framework.Services.Models.RequestBase" />
    [DataContract]
    public sealed class LogoutRequest : RequestBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogoutRequest" /> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="sessionKey">The session key.</param>
        public LogoutRequest(string userName, Guid sessionId, string sessionKey)
        {
            UserName = userName;
            SessionId = sessionId;
            SessionKey = sessionKey;
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
        /// Gets or sets the session identifier.
        /// </summary>
        /// <value>
        /// The session identifier.
        /// </value>
        [DataMember]
        public Guid SessionId { get; set; }

        /// <summary>
        /// Gets or sets the session key.
        /// </summary>
        /// <value>
        /// The session key.
        /// </value>
        [DataMember]
        public string SessionKey { get; set; }

        #endregion
    }
}
