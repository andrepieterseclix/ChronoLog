using CLog.DataAccess.Contracts.Repositories.Access;
using CLog.Models.Access;

namespace CLog.DataAccess.Repositories.Access
{
    /// <summary>
    /// Represents the Session repository.
    /// </summary>
    /// <seealso cref="CLog.DataAccess.Repositories.GenericRepository{CLog.Models.Access.Session}" />
    /// <seealso cref="CLog.DataAccess.Contracts.Repositories.Access.ISessionRepository" />
    public sealed class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionRepository"/> class.
        /// </summary>
        public SessionRepository()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionRepository"/> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        public SessionRepository(DataContext dataContext)
            : base(dataContext)
        {
        }

        #endregion
    }
}
