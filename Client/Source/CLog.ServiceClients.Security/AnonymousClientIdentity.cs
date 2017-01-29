using System;

namespace CLog.ServiceClients.Security
{
    /// <summary>
    /// Represents the anonymous user identity.
    /// </summary>
    /// <seealso cref="CLog.UI.Common.Security.UserIdentity" />
    public sealed class AnonymousClientIdentity : ClientIdentity
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AnonymousClientIdentity"/> class.
        /// </summary>
        public AnonymousClientIdentity()
            : base(string.Empty, string.Empty, Guid.Empty, null, new string[0])
        {
        }

        #endregion
    }
}
