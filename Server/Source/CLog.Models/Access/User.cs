using CLog.Framework.Models;
using CLog.Models.Timesheets;
using System.Collections.ObjectModel;

namespace CLog.Models.Access
{
    /// <summary>
    /// Represents the User business domain model.
    /// </summary>
    /// <seealso cref="CLog.Models.BusinessModel" />
    public class User : BusinessModel
    {
        #region Fields

        private Collection<Session> _sessions;

        private Collection<CapturedTime> _capturedTimeItems;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
            : this(default(long), DataState.None, null, null, null, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt used for password hashing.</param>
        /// <param name="name">The name.</param>
        /// <param name="surname">The surname.</param>
        /// <param name="email">The email.</param>
        public User(long id, DataState state, string userName, string password, string salt, string name, string surname, string email)
            : base(id)
        {
            State = state;
            UserName = userName;
            Password = password;
            Salt = salt;
            Name = name;
            Surname = surname;
            Email = email;

            Sessions = new Collection<Session>();
            CapturedTimeItems = new Collection<CapturedTime>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public DataState State { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the salt for password hashing.
        /// </summary>
        /// <value>
        /// The salt.
        /// </value>
        public string Salt { get; set; }

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

        /// <summary>
        /// Gets or sets the manager identifier.
        /// </summary>
        /// <value>
        /// The manager identifier.
        /// </value>
        public long? ManagerId { get; set; }

        /// <summary>
        /// Gets or sets the manager.
        /// </summary>
        /// <value>
        /// The manager.
        /// </value>
        public virtual User Manager { get; set; }

        /// <summary>
        /// Gets or sets the sessions.
        /// </summary>
        /// <value>
        /// The sessions.
        /// </value>
        public virtual Collection<Session> Sessions
        {
            get { return _sessions; }
            set { _sessions = value; }
        }

        /// <summary>
        /// Gets or sets the captured time items.
        /// </summary>
        /// <value>
        /// The captured time items.
        /// </value>
        public virtual Collection<CapturedTime> CapturedTimeItems
        {
            get { return _capturedTimeItems; }
            set { _capturedTimeItems = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new instance of the <see cref="User" /> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="name">The name.</param>
        /// <param name="surname">The surname.</param>
        /// <param name="email">The email.</param>
        /// <param name="manager">The manager.</param>
        /// <returns>A new user.</returns>
        public static User New(DataState state, string userName, string password, string salt, string name, string surname, string email, User manager)
        {
            User model = new User(0, state, userName, password, salt, name, surname, email);

            if (manager != null)
            {
                model.ManagerId = manager.Id;
                model.Manager = manager;
            }

            return model;
        }

        #endregion
    }
}
