namespace CLog.Models.Users
{
    /// <summary>
    /// Represents the User Details transient class.
    /// </summary>
    public class UserDetails
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDetails" /> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="name">The name.</param>
        /// <param name="surname">The surname.</param>
        /// <param name="email">The email.</param>
        public UserDetails(string userName, string name, string surname, string email)
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
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>
        /// The surname.
        /// </value>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }

        #endregion
    }
}
