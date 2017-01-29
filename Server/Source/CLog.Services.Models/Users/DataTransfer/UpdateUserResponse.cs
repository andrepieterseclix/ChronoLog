using CLog.Framework.Services.Models;
using CLog.Services.Models.Access;
using System.Runtime.Serialization;

namespace CLog.Services.Models.Users.DataTransfer
{
    /// <summary>
    /// Represents the update user response model.
    /// </summary>
    /// <seealso cref="CLog.Framework.Services.Models.ResponseBase" />
    [DataContract]
    public sealed class UpdateUserResponse : ResponseBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserResponse"/> class.
        /// </summary>
        public UpdateUserResponse()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserResponse"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public UpdateUserResponse(SessionDto session)
        {
            Session = session;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the session.
        /// </summary>
        /// <value>
        /// The session.
        /// </value>
        [DataMember]
        public SessionDto Session { get; set; }

        #endregion
    }
}
