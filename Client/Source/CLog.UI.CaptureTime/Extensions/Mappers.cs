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
        /// <summary>
        /// Maps the specified dto to its corresponding model.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns></returns>
        public static CaptureTimeDayViewModel Map(this CapturedTimeDto dto)
        {
            if (dto == null)
                return null;

            CaptureTimeDay model = new CaptureTimeDay(
                dto.Date,
                dto.HoursWorked,
                dto.IsLocked,
                dto.IsEnabled,
                dto.IsPublicHoliday);

            return new CaptureTimeDayViewModel(model);
        }
    }
}
