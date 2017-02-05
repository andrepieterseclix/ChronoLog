using CLog.Services.Models.Timesheets;
using CLog.UI.CaptureTime.ViewModels;
using CLog.UI.Common.Services;
using CLog.UI.Models.Timesheets;
using System.Collections.Generic;
using System.Linq;

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
        /// <returns>The <see cref="CaptureTimeDay"/> model.</returns>
        public static CaptureTimeDay Map(this CapturedTimeDto dto)
        {
            if (dto == null)
                return null;

            CaptureTimeDay model = new CaptureTimeDay(
                dto.Date,
                dto.HoursWorked,
                dto.IsLocked,
                dto.IsEnabled,
                dto.IsPublicHoliday);

            return model;
        }

        /// <summary>
        /// Maps the specified dtos to their corresponding models.
        /// </summary>
        /// <param name="dtos">The dtos.</param>
        /// <returns>The <see cref="CaptureTimeDay"/> models.</returns>
        public static CaptureTimeDay[] Map(this IEnumerable<CapturedTimeDto> dtos)
        {
            if (dtos == null)
                return null;

            return dtos
                .Select(Map)
                .ToArray();
        }

        /// <summary>
        /// Maps the specified user name.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns>The <see cref="CapturedTimeDetailDto"/> data transfer object.</returns>
        public static CapturedTimeDetailDto Map(this CaptureTimeDay model, string userName)
        {
            if (model == null)
                return null;

            return new CapturedTimeDetailDto(
                userName,
                model.Date,
                model.Hours);
        }

        /// <summary>
        /// Maps the specified models.
        /// </summary>
        /// <param name="models">The models.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns>
        /// The <see cref="CapturedTimeDetailDto" /> data transfer objects.
        /// </returns>
        public static CapturedTimeDetailDto[] Map(this IEnumerable<CaptureTimeDay> models, string userName)
        {
            if (models == null)
                return null;

            return models
                .Select(x => x.Map(userName))
                .ToArray();
        }
    }
}
