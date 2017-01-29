using CLog.Infrastructure.Contracts.Security;
using System;
using System.Security.Cryptography;
using System.Text;

namespace CLog.Infrastructure.Security
{
    /// <summary>
    /// Provides methods for computing password hashes based on SHA256.
    /// </summary>
    public class PasswordHelperSha256 : IPasswordHelper
    {
        #region Methods

        /// <summary>
        /// Gets the random salt.
        /// </summary>
        /// <returns>The random crypto-generated salt.</returns>
        public string GetRandomSalt()
        {
            byte[] salt = new byte[6];
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        /// <returns>The salted hashed password.</returns>
        public string ComputeHash(string password, string salt)
        {
            if (string.IsNullOrWhiteSpace(password))
                return null;

            SHA256Managed csp = new SHA256Managed();
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password + salt);
            byte[] hash = csp.ComputeHash(passwordBytes);

            return Convert.ToBase64String(hash);
        }

        #endregion
    }
}
