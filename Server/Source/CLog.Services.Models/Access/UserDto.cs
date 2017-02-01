using System.Runtime.Serialization;

namespace CLog.Services.Models.Access
{
    /// <summary>
    /// Represents the User data transfer object.
    /// </summary>
    [DataContract]
    public sealed class UserDto : DtoBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDto"/> class.
        /// </summary>
        public UserDto()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDto"/> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="name">The name.</param>
        /// <param name="surname">The surname.</param>
        /// <param name="email">The email.</param>
        public UserDto(string userName, string name, string surname, string email)
        {
            UserName = userName;
            Name = name;
            Surname = surname;
            Email = email;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>
        /// The surname.
        /// </value>
        [DataMember]
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [DataMember]
        public string Email { get; set; }

        #endregion
    }
}
