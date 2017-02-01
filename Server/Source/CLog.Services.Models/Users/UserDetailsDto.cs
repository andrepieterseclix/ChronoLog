using System.Runtime.Serialization;

namespace CLog.Services.Models.Users
{
    /// <summary>
    /// Represents the User update details data transfer object.
    /// </summary>
    [DataContract]
    public class UserDetailsDto : DtoBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDto" /> class.
        /// </summary>
        public UserDetailsDto()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDto" /> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="name">The name.</param>
        /// <param name="surname">The surname.</param>
        /// <param name="email">The email.</param>
        public UserDetailsDto(string userName, string name, string surname, string email)
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
        [DataMember(IsRequired = true)]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>
        /// The surname.
        /// </value>
        [DataMember(IsRequired = true)]
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [DataMember(IsRequired = true)]
        public string Email { get; set; }

        #endregion
    }
}
