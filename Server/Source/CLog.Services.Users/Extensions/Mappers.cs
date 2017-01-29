using CLog.Models.Access;
using CLog.Models.Users;
using CLog.Services.Models.Access;
using CLog.Services.Models.Users;

namespace CLog.Services.Users.Extensions
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
        /// <returns>The <see cref="SessionDto"/> model.</returns>
        public static SessionDto Map(this Session model)
        {
            if (model == null)
                return null;

            SessionDto dto = new SessionDto(model.RefId, model.SessionKey);

            return dto;
        }

        /// <summary>
        /// Maps the specified dto.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>The corresponding domain model.</returns>
        public static UserDetails Map(this UserDetailsDto dto)
        {
            if (dto == null)
                return null;

            return new UserDetails(
                dto.UserName,
                dto.Name,
                dto.Surname,
                dto.Email);
        }

        /// <summary>
        /// Maps the specified dto.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>The corresponding domain model.</returns>
        public static UserPassword Map(this UserPasswordDto dto)
        {
            if (dto == null)
                return null;

            return new UserPassword(dto.UserName, dto.OldPassword, dto.NewPassword);
        }
    }
}
