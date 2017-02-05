using CLog.Services.Models.Access;
using CLog.Services.Models.Users;
using CLog.UI.Models.Access;

namespace CLog.UI.UserProfile.Extensions
{
    /// <summary>
    /// Represents the Data Transfer Objects to Domain Model and View Model mapper.
    /// </summary>
    public static class Mappers
    {
        /// <summary>
        /// Maps the specified model to its corresponding data transfer object model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>The <see cref="UserDetailsDto"/> model.</returns>
        public static UserDetailsDto Map(this User model)
        {
            if (model == null)
                return null;

            return new UserDetailsDto(
                model.UserName,
                model.Name,
                model.Surname,
                model.Email);
        }

        /// <summary>
        /// Maps the specified dto to its corresponding model.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>The <see cref="SessionInfo"/> model.</returns>
        public static SessionInfo Map(this SessionDto dto)
        {
            if (dto == null)
                return null;

            return new SessionInfo(
                dto.Id,
                dto.SessionKey);
        }
    }
}
