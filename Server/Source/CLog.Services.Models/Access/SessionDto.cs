using System;
using System.Runtime.Serialization;

namespace CLog.Services.Models.Access
{
    /// <summary>
    /// Represents the Session data transfer object.
    /// </summary>
    [DataContract]
    public sealed class SessionDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionDto"/> class.
        /// </summary>
        public SessionDto()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionDto"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="sessionKey">The session key.</param>
        public SessionDto(Guid id, string sessionKey)
        {
            Id = id;
            SessionKey = sessionKey;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public Guid Id { get; set; }

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
