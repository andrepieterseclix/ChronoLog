using CLog.Models.Timesheets;
using CLog.Services.Models.Timesheets;
using System.Collections.Generic;
using System.Linq;

namespace CLog.Services.Timesheets.Extensions
{
    /// <summary>
    /// Represents the mapping extension methods.
    /// </summary>
    public static class Mappers
    {
        /// <summary>
        /// Maps the specified model to its corresponding data transfer object.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// The <see cref="CapturedTimeDto" /> model.
        /// </returns>
        public static CapturedTimeDto Map(this CapturedTime model)
        {
            if (model == null)
                return null;

            CapturedTimeDto dto = new CapturedTimeDto(
                model.User.UserName,
                model.Date,
                model.HoursWorked,
                model.IsLocked,
                model.IsPublicHoliday,
                model.IsEnabled);

            return dto;
        }

        /// <summary>
        /// Maps the specified models to their corresponding data transfer objects.
        /// </summary>
        /// <param name="models">The models.</param>
        /// <returns>The <see cref="CapturedTimeDto"/> based collection of DTOs.</returns>
        public static CapturedTimeDto[] Map(this IEnumerable<CapturedTime> models)
        {
            if (models == null)
                return null;

            return models
                .Select(Map)
                .ToArray();
        }

        /// <summary>
        /// Maps the specified dto.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns></returns>
        public static CapturedTimeDetail Map(this CapturedTimeDetailDto dto)
        {
            if (dto == null)
                return null;

            return new CapturedTimeDetail(
                dto.UserName,
                dto.Date,
                dto.HoursWorked);
        }

        /// <summary>
        /// Maps the specified dtos.
        /// </summary>
        /// <param name="dtos">The dtos.</param>
        /// <returns></returns>
        public static CapturedTimeDetail[] Map(this IEnumerable<CapturedTimeDetailDto> dtos)
        {
            if (dtos == null)
                return null;

            return dtos
                .Select(Map)
                .ToArray();
        }
    }
}
