using CLog.Infrastructure.Contracts.Security;
using CLog.Models.Access;
using System;
using System.Web.Security;

namespace CLog.Infrastructure.Security
{
    /// <summary>
    /// Represents the login token helper
    /// </summary>
    /// <seealso cref="CLog.Infrastructure.Contracts.Security.ILoginTokenHelper" />
    public class LoginTokenHelper : ILoginTokenHelper
    {
        #region Fields

        private const string DATA_SEPARATOR = "<!>";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginTokenHelper"/> class.
        /// </summary>
        public LoginTokenHelper()
            : this(new TimeSpan(0, 10, 0))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginTokenHelper"/> class.
        /// </summary>
        /// <param name="tokenValidFor">The token valid for.</param>
        public LoginTokenHelper(TimeSpan tokenValidFor)
        {
            ValidFor = tokenValidFor;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the time that the token is valid for.
        /// </summary>
        /// <value>
        /// The amount of time before the token expires.
        /// </value>
        public TimeSpan ValidFor { get; private set; }

        #endregion

        /// <summary>
        /// Generates the security token.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>
        /// The security token.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public string GenerateSecurityToken(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            string[] userData = new[]
            {
                user.Id.ToString(),
                user.UserName,
                user.Name,
                user.Surname,
                user.Email
            };

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1,
                user.UserName,
                DateTime.Now,
                DateTime.Now.Add(ValidFor),
                true,
                string.Join(DATA_SEPARATOR, userData));

            return FormsAuthentication.Encrypt(ticket);
        }

        /// <summary>
        /// Verifies the security token.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="sessionExpired"></param>
        /// <returns>
        /// returns <c>true</c> if the security token associated with the session is valid.
        /// </returns>
        public bool VerifySecurityToken(Session session, out bool sessionExpired)
        {
            sessionExpired = false;
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(session.SessionKey);

            if (ticket == null)
                return false;

            if (ticket.Expired)
            {
                sessionExpired = true;
                return false;
            }

            if (ticket.Name != session.User.UserName)
                return false;

            string[] userData = ticket.UserData.Split(new string[] { DATA_SEPARATOR }, StringSplitOptions.None);

            return
                userData.Length == 5 &&
                userData[0] == session.User.Id.ToString() &&
                userData[1] == session.User.UserName &&
                userData[2] == session.User.Name &&
                userData[3] == session.User.Surname &&
                userData[4] == session.User.Email;
        }
    }
}
