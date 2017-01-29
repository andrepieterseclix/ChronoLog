using System;
using System.Linq;
using System.Security.Principal;

namespace CLog.Framework.Security
{
    /// <summary>
    /// Represents the server principal.
    /// </summary>
    /// <seealso cref="System.Security.Principal.IPrincipal" />
    public sealed class ServerPrincipal : IPrincipal
    {
        #region Fields

        private ServerIdentity _identity;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerPrincipal"/> class for anonymous calls.
        /// </summary>
        public ServerPrincipal()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerPrincipal"/> class.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public ServerPrincipal(ServerIdentity identity)
        {
            if (identity == null)
                throw new ArgumentNullException(nameof(identity));

            _identity = identity;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="ServerIdentity"/> based identity of the current principal.
        /// </summary>
        public ServerIdentity Identity
        {
            get { return _identity ?? new AnonymousServerIdentity(); }
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