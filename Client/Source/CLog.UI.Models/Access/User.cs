using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CLog.UI.Models.Access
{
    /// <summary>
    /// Represents the User model.
    /// </summary>
    public sealed class User : ModelBase, IDataErrorInfo
    {
        #region Fields

        public const string REGEX_EMAIL = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        private string _userName;

        private string _name;

        private string _surname;

        private string _fullName;

        private string _email;
        
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {
            PropertyChanged += User_PropertyChanged;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="name">The name.</param>
        /// <param name="surname">The surname.</param>
        /// <param name="email">The email.</param>
        public User(string userName, string name, string surname, string email)
            : this()
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
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>
        /// The surname.
        /// </value>
        public string Surname
        {
            get { return _surname; }
            set { SetProperty(ref _surname, value); }
        }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public string FullName
        {
            get { return _fullName; }
            set { SetProperty(ref _fullName, value); }
        }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        #endregion

        #region Methods

        private void User_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(Name) || e.PropertyName == nameof(Surname))
            {
                FullName = string.Format(CultureInfo.CurrentCulture, "{0} {1}", Name, Surname);
            }
        }

        protected override string ValidateProperty(string propertyName)
        {
            string result = null;

            if (propertyName == nameof(Name))
            {
                if (string.IsNullOrWhiteSpace(Name) || Name.Length < 2)
                    result = string.Format(CultureInfo.CurrentCulture, "The name '{0}' is invalid!", Name);
            }
            if (propertyName == nameof(Surname))
            {
                if (string.IsNullOrWhiteSpace(Surname) || Surname.Length < 2)
                    result = string.Format(CultureInfo.CurrentCulture, "The surname '{0}' is invalid!", Surname);
            }
            if (propertyName == nameof(Email))
            {
                if (string.IsNullOrWhiteSpace(Email) || !Regex.IsMatch(Email, REGEX_EMAIL))
                    result = "The e-mail address is invalid.";
            }

            return result;
        }

        #endregion
    }
}
