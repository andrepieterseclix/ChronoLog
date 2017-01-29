using CLog.Framework.Services.Models;
using System.Runtime.Serialization;

namespace CLog.Services.Models.Access.DataTransfer
{
    /// <summary>
    /// Represents the Login request model.
    /// </summary>
    /// <seealso cref="CLog.Framework.Services.Models.RequestBase" />
    [DataContract]
    public sealed class LoginRequest : RequestBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginRequest"/> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        public LoginRequest(string userName, string password)
        {
            UserName = userName;
            Password = password;
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
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [DataMember(IsRequired = true)]
        public string Password { get; set; }

        #endregion
    }
}
