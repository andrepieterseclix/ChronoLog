using CLog.Framework.Services.Models;

namespace CLog.UI.Common.Business
{
    /// <summary>
    /// Represents the UI business layer extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Maps the specified dto to its corresponding model.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>The <see cref="ErrorMessage"/> model.</returns>
        public static ErrorMessage Map(this ErrorDto dto)
        {
            if (dto == null)
                return null;

            return new ErrorMessage(dto.Code, dto.Message, dto.AdditionalInfo);
        }

        /// <summary>
        /// Adds the messages from the service response to the business result.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="serviceResponse">The service response.</param>
        public static void AddMessages(this BusinessResult result, ResponseBase serviceResponse)
        {
            if (result == null || serviceResponse == null)
                return;

            foreach (ErrorDto errorDto in serviceResponse.Errors)
                result.Errors.Add(errorDto.Map());
        }
    }
}
