using CLog.Common.Logging;
using CLog.Services.Models.Timesheets;
using CLog.UI.CaptureTime.ViewModels;
using CLog.UI.Models.Timesheets;

namespace CLog.UI.CaptureTime.Extensions
{
    /// <summary>
    /// Represents the Data Transfer Objects to Domain Model and View Model mapper.
    /// </summary>
    public static class Mappers
    {
        public static CaptureTimeDayViewModel Map(this CapturedTimeDto dto, ILogger logger)
        {
            if (dto == null)
                return null;

            CaptureTimeDay model = new CaptureTimeDay(
                dto.Date,
                dto.HoursWorked,
                dto.IsLocked,
                dto.IsEnabled,
                dto.IsPublicHoliday);

            return new CaptureTimeDayViewModel(logger, model);
        }
    }
}
