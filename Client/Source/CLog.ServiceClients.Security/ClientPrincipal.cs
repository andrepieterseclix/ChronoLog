using System.Linq;
using System.Security.Principal;

namespace CLog.ServiceClients.Security
{
    /// <summary>
    /// Represents the security principal.
    /// </summary>
    /// <seealso cref="System.Security.Principal.IPrincipal" />
    public sealed class ClientPrincipal : IPrincipal
    {
        #region Fields

        private ClientIdentity _identity;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="ClientIdentity"/> based identity of the current principal.
        /// </summary>
        public ClientIdentity Identity
        {
            get { return _identity ?? new AnonymousClientIdentity(); }
            set { _identity = value; }
        }

        /// <summary>
        /// Gets the identity of the current principal.
        /// </summary>
        IIdentity IPrincipal.Identity
        {
            get { return Identity; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether the current principal belongs to the specified role.
        /// </summary>
        /// <param name="role">The name of the role for which to check membership.</param>
        /// <returns>
        /// true if the current principal is a member of the specified role; otherwise, false.
        /// </returns>
        public bool IsInRole(string role)
        {
            return _identity.Roles.Contains(role);
        }

        #endregion
    }
}
