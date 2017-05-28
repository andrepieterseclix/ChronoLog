using CLog.Common.Logging;
using CLog.Framework.Models;
using CLog.Framework.Services.Models;
using System;
using System.Diagnostics;
using System.Linq;
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
        /// Wraps the execution of a custom operation.
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

        /// <summary>
        /// Executes the GET method.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="callingMethodName">Name of the calling method.</param>
        /// <returns>An <see cref="IHttpActionResult"/> based result for the GET method.</returns>
        protected IHttpActionResult ExecuteGet(Func<ResponseBase> action, [CallerMemberName]string callingMethodName = null)
        {
            Stopwatch stopwatch = new Stopwatch();

            try
            {
                LoggerHelper.Info(Logger, "Service method {0} executing...", callingMethodName);
                stopwatch.Start();

                ResponseBase response = action();

                IHttpActionResult failedResult;
                if (RequestFailed(response, out failedResult))
                    return failedResult;

                return Ok(response);
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

        /// <summary>
        /// Executes the PUT method.
        /// </summary>
        /// <typeparam name="T">The <see cref="ResponseBase" /> based response type.</typeparam>
        /// <param name="createdRouteName">Name of the created route.</param>
        /// <param name="getCreatedRouteValues">The delegate used to get the created route values.</param>
        /// <param name="action">The action.</param>
        /// <param name="callingMethodName">Name of the calling method.</param>
        /// <returns>
        /// The <see cref="IHttpActionResult" /> based result for the create method.
        /// </returns>
        protected IHttpActionResult ExecuteCreate<T>(string createdRouteName, Func<object> getCreatedRouteValues, Func<T> action, [CallerMemberName]string callingMethodName = null)
            where T : ResponseBase
        {
            Stopwatch stopwatch = new Stopwatch();

            try
            {
                LoggerHelper.Info(Logger, "Service method {0} executing...", callingMethodName);
                stopwatch.Start();

                T response = action();

                IHttpActionResult failedResult;
                if (RequestFailed(response, out failedResult))
                    return failedResult;

                object createdRouteValues = getCreatedRouteValues?.Invoke() ?? new object();

                return CreatedAtRoute(createdRouteName, createdRouteValues, response);
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

        /// <summary>
        /// Executes the POST method.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="callingMethodName">Name of the calling method.</param>
        /// <returns>An <see cref="IHttpActionResult"/> based result for the POST method.</returns>
        protected IHttpActionResult ExecuteUpdate(Func<ResponseBase> action, [CallerMemberName]string callingMethodName = null)
        {
            Stopwatch stopwatch = new Stopwatch();

            try
            {
                LoggerHelper.Info(Logger, "Service method {0} executing...", callingMethodName);
                stopwatch.Start();

                ResponseBase response = action();

                IHttpActionResult failedResult;
                if (RequestFailed(response, out failedResult))
                    return failedResult;

                return Ok(response);
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

        private bool RequestFailed(ResponseBase response, out IHttpActionResult failedResult)
        {
            failedResult = null;

            if (response == null)
            {
                failedResult = InternalServerError();
                return false;
            }

            if (response.Errors.Count > 0)
            {
                if (response.Errors.All(x => x.Category == ErrorCategory.ResourceNotFound))
                {
                    failedResult = NotFound();
                }
                else if (response.Errors.All(x => x.Category == ErrorCategory.InvalidRequest))
                {
                    string message = string.Join("\r\n", response.Errors.Select(x => x.ToString()));
                    failedResult = BadRequest(message);
                }
                else
                {
                    failedResult = InternalServerError();
                }
            }

            return failedResult != null;
        }

        #endregion
    }
}
