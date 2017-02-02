using CLog.Services.Models.Timesheets;
using CLog.Services.Models.Timesheets.DataTransfer;
using System;

namespace CLog.UI.Testing.Configuration.DataHelpers
{
    public static class TimesheetsDataHelper
    {
        public static GetCapturedTimeResponse GetGetCapturedTimeResponse()
        {
            DateTime fromDate = new DateTime(2017, 1, 2);

            CapturedTimeDto[] items = new[]
            {
                new CapturedTimeDto("UserName", (fromDate = fromDate.AddDays(1)), 8, true, false, false),
                new CapturedTimeDto("UserName", (fromDate = fromDate.AddDays(1)), 7, false, true, true),
                new CapturedTimeDto("UserName", (fromDate = fromDate.AddDays(1)), 6, false, false, false),
                new CapturedTimeDto("UserName", (fromDate = fromDate.AddDays(1)), 5, false, false, true),
                new CapturedTimeDto("UserName", (fromDate = fromDate.AddDays(1)), 4, false, false, true),
                new CapturedTimeDto("UserName", (fromDate = fromDate.AddDays(1)), 3, false, false, true),
                new CapturedTimeDto("UserName", (fromDate = fromDate.AddDays(1)), 0, false, false, true),
            };

            return new GetCapturedTimeResponse(items);
        }

        public static SaveCapturedTimeResponse GetSaveCapturedTimeResponse()
        {
            return new SaveCapturedTimeResponse();
        }
    }
}
