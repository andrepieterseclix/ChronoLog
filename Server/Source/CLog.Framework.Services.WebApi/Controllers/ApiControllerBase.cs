using CLog.Common.Logging;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Web.Http;

namespace CLog.Framework.Services.WebApi.Controllers
{
    /// <summary>
    /// Represents the base class for Api Controllers.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public abstract class ApiControllerBase : ApiController
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiControllerBase"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        protected ApiControllerBase(ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        protected ILogger Logger { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Wraps the execution of a get operation.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="callingMethodName">Name of the calling method.</param>
        /// <returns>An <see cref="IHttpActionResult"/> based result.</returns>
        protected IHttpActionResult Execute(Func<IHttpActionResult> action, [CallerMemberName]string callingMethodName = null)
        {
            Stopwatch stopwatch = new Stopwatch();

            try
            {
                LoggerHelper.Info(Logger, "Service method {0} executing...", callingMethodName);
                stopwatch.Start();

                return action();
            }
            catch (Exception ex)
            {
                LoggerHelper.Exception(Logger, ex, "Error calling Service method {0}", callingMethodName);

                return InternalServerError(ex);
            }
            finally
            {
                stopwatch.Stop();
                LoggerHelper.Info(Logger, "Service method {0} executed in {1}", callingMethodName, stopwatch.Elapsed);
            }
        }

        #endregion
    }
}
