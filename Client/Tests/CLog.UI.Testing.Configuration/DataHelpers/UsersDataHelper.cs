using CLog.Services.Models.Access;
using CLog.Services.Models.Users.DataTransfer;
using System;

namespace CLog.UI.Testing.Configuration.DataHelpers
{
    public static class UsersDataHelper
    {
        public static UpdateUserResponse GetUpdateUserResponse()
        {
            SessionDto session = new SessionDto(Guid.NewGuid(), "Session Key");

            return new UpdateUserResponse(session);
        }
    }
}
