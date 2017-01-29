using CLog.Models.Access;

namespace CLog.DataAccess.Contracts.Repositories.Access
{
    /// <summary>
    /// Represents the User repository contract.
    /// </summary>
    /// <seealso cref="CLog.DataAccess.Contracts.Repositories.IRepository{CLog.Models.Access.User}" />
    public interface IUserRepository : IRepository<User>
    {
    }
}
