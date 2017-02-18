using CLog.Common.BaseClasses;
using CLog.Common.Logging;
using CLog.Framework.Services.Models;
using CLog.UI.Common.Exceptions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CLog.UI.Common.Business
{
    /// <summary>
    /// Represents the UI Business Manager base.
    /// </summary>
    public abstract class UIBusinessManager : CommonBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UIBusinessManager"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public UIBusinessManager(ILogger logger)
            : base(logger)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes the specified action.
        /// </summary>
        /// <typeparam name="T">The result type of the business result that will be returned.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="callingMethod">The calling method.</param>
        /// <returns>
        /// The business result.
        /// </returns>
        protected UIBusinessResult<T> Execute<T>(Action<UIBusinessResult<T>> action, [CallerMemberName]string callingMethod = null)
            where T : class
        {
            UIBusinessResult<T> result = new UIBusinessResult<T>();

            Stopwatch stopwatch = new Stopwatch();

            try
            {
                stopwatch.Start();
                LoggerHelper.Debug(Logger, "Started executing '{0}'", GetQualifiedMethodName(callingMethod));

                action?.Invoke(result);

            }
            catch (Exception ex)
            {
                LoggerHelper.Fatal(Logger, ex, "Unhandled Exception occurred in business manager:  {0}", GetQualifiedMethodName(callingMethod));

                throw;
            }
            finally
            {
                stopwatch.Stop();

                LoggerHelper.Debug(Logger, "Finished executing '{0}' in {1}", GetQualifiedMethodName(callingMethod), stopwatch.Elapsed);
            }

            return result;
        }

        /// <summary>
        /// Executes the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="callingMethod">The calling method.</param>
        /// <returns>The business result.</returns>
        protected UIBusinessResult Execute(Action<UIBusinessResult> action, [CallerMemberName]string callingMethod = null)
        {
            UIBusinessResult result = new UIBusinessResult();

            Stopwatch stopwatch = new Stopwatch();

            try
            {
                stopwatch.Start();
                LoggerHelper.Debug(Logger, "Started executing '{0}'", GetQualifiedMethodName(callingMethod));

                action?.Invoke(result);

            }
            catch (Exception ex)
            {
                LoggerHelper.Fatal(Logger, ex, "Unhandled Exception occurred in business manager:  {0}", GetQualifiedMethodName(callingMethod));

                throw;
            }
            finally
            {
                stopwatch.Stop();

                LoggerHelper.Debug(Logger, "Finished executing '{0}' in {1}", GetQualifiedMethodName(callingMethod), stopwatch.Elapsed);
            }

            return result;
        }

        /// <summary>
        /// Gets the service response, and checks whether the session has expired on the server.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="serviceCall">The service call.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="CLog.UI.Common.Exceptions.SessionExpiredException"></exception>
        protected TResponse GetServiceResponse<TRequest, TResponse>(Func<TRequest, TResponse> serviceCall, TRequest request)
            where TRequest : RequestBase
            where TResponse : ResponseBase
        {
            if (serviceCall == null || request == null)
                return null;

            TResponse response = serviceCall(request);
            if (response == null)
                return null;

            if (response.SessionExpired)
                throw new SessionExpiredException(response.Errors.FirstOrDefault()?.Message);

            return response;
        }

        #endregion
    }
}
