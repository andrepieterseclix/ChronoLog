using CLog.Framework.Services.Contracts;
using CLog.Services.Models.Timesheets.DataTransfer;
using System.ServiceModel;

namespace CLog.Services.Contracts.Timesheets
{
    /// <summary>
    /// Represents the time sheets service contract.
    /// </summary>
    [ServiceContract]
    public interface ITimesheetService : IService
    {
        /// <summary>
        /// Gets the captured time.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The captured time response.</returns>
        [OperationContract]
        GetCapturedTimeResponse GetCapturedTime(GetCapturedTimeRequest request);

        /// <summary>
        /// Saves the captured time.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The save capture time response.</returns>
        [OperationContract]
        SaveCapturedTimeResponse SaveCapturedTime(SaveCapturedTimeRequest request);
    }
}
