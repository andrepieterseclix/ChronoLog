using CLog.Common.Logging;
using CLog.Framework.Services.WebApi.Controllers;
using CLog.Services.Contracts.Timesheets;
using CLog.Services.Models.Timesheets;
using CLog.Services.Models.Timesheets.DataTransfer;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace ChronoLog.Host.WebApi.Controllers
{
    /// <summary>
    /// Represents the Timesheet REST service.
    /// </summary>
    /// <seealso cref="CLog.Framework.Services.WebApi.Controllers.ApiControllerBase" />
    public class TimesheetController : ApiControllerBase
    {
        #region Fields

        private readonly ITimesheetService _service;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimesheetController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="service">The service.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public TimesheetController(ILogger logger, ITimesheetService service)
            : base(logger)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            _service = service;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the captured time.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns>The <see cref="GetCapturedTimeResponse"/> based response.</returns>
        [HttpGet]
        [Route("Timesheet", Name = "GetCapturedTime")]
        [ResponseType(typeof(GetCapturedTimeResponse))]
        public IHttpActionResult GetCapturedTime(DateTime fromDate, DateTime toDate)
        {
            return ExecuteGet(() =>
            {
                GetCapturedTimeRequest request = new GetCapturedTimeRequest(fromDate, toDate);

                return _service.GetCapturedTime(request);
            });
        }

        /// <summary>
        /// Saves the captured time.
        /// </summary>
        /// <param name="models">The models.</param>
        /// <returns>The <see cref="SaveCapturedTimeResponse"/> based response.</returns>
        [HttpPost]
        [Route("Timesheet")]
        [ResponseType(typeof(SaveCapturedTimeResponse))]
        public IHttpActionResult SaveCapturedTime([FromBody]CapturedTimeDetailDto[] models)
        {
            return ExecuteCreate(
                "GetCapturedTime",
                () =>
                {
                    DateTime[] dates = models
                        .Select(x => x.Date)
                        .OrderBy(x => x)
                        .ToArray();

                    return new { fromDate = dates.First().ToString("yyyy-MM-dd"), toDate = dates.Last().ToString("yyyy-MM-dd") };
                },
                () =>
                {
                    SaveCapturedTimeRequest request = new SaveCapturedTimeRequest(models);

                    return _service.SaveCapturedTime(request);
                });
        }

        #endregion

        #region IDisposable Implementation

        /// <summary>
        /// Keep this private, and create and maintain one for every derived class.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _service?.Dispose();
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}