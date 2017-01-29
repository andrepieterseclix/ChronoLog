using CLog.DataAccess.Contracts.Repositories.Access;
using CLog.Models.Access;

namespace CLog.DataAccess.Repositories.Access
{
    /// <summary>
    /// Represents the User repository.
    /// </summary>
    /// <seealso cref="CLog.DataAccess.Repositories.GenericRepository{CLog.Models.Access.User}" />
    /// <seealso cref="CLog.DataAccess.Contracts.Repositories.Access.IUserRepository" />
    public sealed class UserRepository : GenericRepository<User>, IUserRepository
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        public UserRepository()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        public UserRepository(DataContext dataContext)
            : base(dataContext)
        {
        }

        #endregion
    }
}
