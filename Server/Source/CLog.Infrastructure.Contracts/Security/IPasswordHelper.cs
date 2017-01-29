namespace CLog.Infrastructure.Contracts.Security
{
    /// <summary>
    /// Represents the contract for the password helper.
    /// </summary>
    public interface IPasswordHelper
    {
        /// <summary>
        /// Gets the random salt.
        /// </summary>
        /// <returns>The salt based on a crypto-random number.</returns>
        string GetRandomSalt();

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        /// <returns>The salted hash.</returns>
        string ComputeHash(string password, string salt);
    }
}
