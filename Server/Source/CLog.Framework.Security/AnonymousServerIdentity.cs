using System;

namespace CLog.Framework.Security
{
    /// <summary>
    /// Represents the anonymous user identity.
    /// </summary>
    /// <seealso cref="CLog.UI.Common.Security.UserIdentity" />
    public sealed class AnonymousServerIdentity : ServerIdentity
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AnonymousServerIdentity" /> class.
        /// </summary>
        public AnonymousServerIdentity()
            : base(string.Empty, 0, Guid.Empty, null, false, new string[0])
        {
        }

        #endregion
    }
}
