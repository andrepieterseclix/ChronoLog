using CLog.Models.Access;
using CLog.Services.Models.Access;
using System;

namespace CLog.Services.Access.Extensions
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
        /// <returns>The <see cref="SessionDto"/> model.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static SessionDto Map(this Session model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            SessionDto dto = new SessionDto(model.RefId, model.SessionKey);

            return dto;
        }

        /// <summary>
        /// Maps the specified model to its corresponding data transfer object.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>The <see cref="UserDto"/> based model.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static UserDto Map(this User model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            UserDto dto = new UserDto(model.UserName, model.Name, model.Surname, model.Email);

            return dto;
        }
    }
}
