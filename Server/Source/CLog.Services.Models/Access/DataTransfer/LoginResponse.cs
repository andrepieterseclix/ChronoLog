using CLog.Framework.Services.Models;
using System.Runtime.Serialization;

namespace CLog.Services.Models.Access.DataTransfer
{
    /// <summary>
    /// Represents the Login response model.
    /// </summary>
    /// <seealso cref="CLog.Framework.Services.Models.ResponseBase" />
    [DataContract]
    public sealed class LoginResponse : ResponseBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginResponse"/> class.
        /// </summary>
        public LoginResponse()
            : this(false, new SessionDto(), new UserDto())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginResponse" /> class.
        /// </summary>
        /// <param name="isLoggedIn">if set to <c>true</c>, the user is logged in successfully.</param>
        /// <param name="session">The session.</param>
        /// <param name="user">The user.</param>
        public LoginResponse(bool isLoggedIn, SessionDto session, UserDto user)
        {
            Session = session;
            User = user;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the User is logged in.
        /// </summary>
        /// <value>
        /// <c>true</c> if the User is logged in successfully; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsLoggedIn { get; set; }

        /// <summary>
        /// Gets or sets the session.
        /// </summary>
        /// <value>
        /// The session.
        /// </value>
        [DataMember]
        public SessionDto Session { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        [DataMember]
        public UserDto User { get; set; }

        #endregion
    }
}
