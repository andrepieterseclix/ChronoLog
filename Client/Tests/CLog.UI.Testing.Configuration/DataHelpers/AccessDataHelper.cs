using CLog.Services.Models.Access;
using CLog.Services.Models.Access.DataTransfer;
using System;

namespace CLog.UI.Testing.Configuration.DataHelpers
{
    public static class AccessDataHelper
    {
        public static LoginResponse GetLoginResponse()
        {
            return new LoginResponse(
                true,
                new SessionDto(Guid.NewGuid(), "Session Key"),
                new UserDto("User Name", "Name", "Surname", "Email"));
        }

        public static LogoutResponse GetLogoutResponse()
        {
            return new LogoutResponse();
        }

        public static UpdateUserPasswordResponse GetUpdateUserPasswordResponse()
        {
            return new UpdateUserPasswordResponse();
        }
    }
}
