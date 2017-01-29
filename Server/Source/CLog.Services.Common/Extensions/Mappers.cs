using CLog.Framework.Business.Models.Results;
using CLog.Framework.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace CLog.Framework.Services.Extensions
{
    /// <summary>
    /// Represents mapping extension methods.
    /// </summary>
    public static class Mappers
    {
        /// <summary>
        /// Maps the specified model to its corresponding data transfer object.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>The <see cref="ErrorDto"/> based model.</returns>
        public static ErrorDto Map(this ErrorMessage model)
        {
            if (model == null)
                return null;

            return new ErrorDto(
                model.Code,
                model.Message,
                model.AdditionalInfo);
        }

        /// <summary>
        /// Maps the specified models.
        /// </summary>
        /// <param name="models">The models.</param>
        /// <returns>The array of <see cref="ErrorDto"/> based models.</returns>
        public static ErrorDto[] Map(this IEnumerable<ErrorMessage> models)
        {
            if (models == null)
                return new ErrorDto[0];

            return models
                .Select(Map)
                .Where(x => x != null)
                .ToArray();
        }

        /// <summary>
        /// Adds the specified error messages mapped to corresponding data transfer objects.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="result">The error messages.</param>
        public static void AddMessages(this ResponseBase response, BusinessResult result)
        {
            if (response?.Errors == null || result == null)
                return;

            foreach (ErrorMessage message in result.Errors)
                response.Errors.Add(message.Map());
        }
    }
}
