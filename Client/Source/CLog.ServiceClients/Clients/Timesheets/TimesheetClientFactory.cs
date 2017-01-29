using CLog.ServiceClients.Contracts.Timesheets;
using CLog.Services.Contracts.Timesheets;

namespace CLog.ServiceClients.Clients.Timesheets
{
    /// <summary>
    /// Represents the service client factory for the Timesheet service.
    /// </summary>
    /// <seealso cref="CLog.ServiceClients.Clients.ServiceClientFactory{CLog.Services.Contracts.Timesheets.ITimesheetService}" />
    /// <seealso cref="CLog.ServiceClients.Contracts.Timesheets.ITimesheetClientFactory" />
    public sealed class TimesheetClientFactory : ServiceClientFactory<ITimesheetService>, ITimesheetClientFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimesheetClientFactory"/> class.
        /// </summary>
        public TimesheetClientFactory()
            : base("BasicHttpTimesheetService")
        {
        }
    }
}
