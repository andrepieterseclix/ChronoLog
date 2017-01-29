using CLog.Framework.ServiceClients;
using CLog.Services.Contracts.Timesheets;

namespace CLog.ServiceClients.Contracts.Timesheets
{
    /// <summary>
    /// Represents the Timesheet client factory contract.
    /// </summary>
    public interface ITimesheetClientFactory
    {
        /// <summary>
        /// Creates a service client instance.
        /// </summary>
        /// <returns>The service client for the Timesheet service.</returns>
        IServiceClient<ITimesheetService> Create();
    }
}
