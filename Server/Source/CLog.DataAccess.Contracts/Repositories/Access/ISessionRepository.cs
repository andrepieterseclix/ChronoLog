using CLog.Models.Access;

namespace CLog.DataAccess.Contracts.Repositories.Access
{
    /// <summary>
    /// Represents the Session repository contract.
    /// </summary>
    /// <seealso cref="CLog.DataAccess.Contracts.Repositories.IRepository{CLog.Models.Access.Session}" />
    public interface ISessionRepository : IRepository<Session>
    {
    }
}
