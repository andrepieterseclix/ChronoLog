using CLog.Models.Access;
using CLog.Models.Timesheets;
using CLog.Services.Models.Timesheets;
using System;
using System.Collections.Generic;

namespace CLog.Models.Mocks.Timesheets
{
    public static class TimesheetsDataHelper
    {
        #region Methods

        public static IEnumerable<CapturedTime> GetCapturedTimeItemsConsecutive(User user, DateTime start, int days)
        {
            for (int i = 0; i < days; i++)
            {
                yield return new CapturedTime((i + 1), start.AddDays(i), 8, false, false, true)
                {
                    User = user,
                    UserId = user?.Id ?? 0
                };
            }
        }

        public static IEnumerable<CapturedTimeDetail> GetCapturedTimeDetailItemsConsecutive(User user, DateTime start, int days)
        {
            for (int i = 0; i < days; i++)
            {
                yield return new CapturedTimeDetail(user?.UserName, start.AddDays(i), 8);
            }
        }

        public static IEnumerable<CapturedTimeDetailDto> GetCapturedTimeDetailDtoItemsConsecutive(User user, DateTime start, int days)
        {
            for (int i = 0; i < days; i++)
            {
                yield return new CapturedTimeDetailDto(user.UserName, start.AddDays(i), 8);
            }
        }

        #endregion
    }
}
