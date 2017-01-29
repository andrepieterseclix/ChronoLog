using CLog.DataAccess.Contracts.Repositories.Timesheets;
using CLog.Models.Timesheets;

namespace CLog.DataAccess.Repositories.Timesheets
{
    /// <summary>
    /// Represents the Date State repository.
    /// </summary>
    /// <seealso cref="CLog.DataAccess.Repositories.GenericRepository{CLog.Models.Timesheets.DateState}" />
    /// <seealso cref="CLog.DataAccess.Contracts.Repositories.Timesheets.IDateStateRepository" />
    public sealed class DateStateRepository : GenericRepository<DateState>, IDateStateRepository
    {
    }
}
