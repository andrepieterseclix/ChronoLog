using CLog.Common.BaseClasses;
using CLog.Common.Logging;
using CLog.Framework.Business.Contracts;
using CLog.Framework.Business.Models.Results;
using CLog.Framework.Security;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace CLog.Framework.Business.Managers
{
    /// <summary>
    /// Represents the base class for business managers.
    /// </summary>
    /// <seealso cref="CLog.Common.BaseClasses.CommonBase" />
    /// <seealso cref="CLog.Framework.Business.Contracts.IBusinessManager" />
    [DebuggerNonUserCode]
    public abstract class BusinessManager : CommonBase, IBusinessManager
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessManager"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public BusinessManager(ILogger logger)
            : base(logger)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the identity for the current user.
        /// </summary>
        /// <value>
        /// The user identity.
        /// </value>
        protected ServerIdentity UserIdentity
        {
            get
            {
                ServerPrincipal principal = Thread.CurrentPrincipal as ServerPrincipal;
                return principal?.Identity;
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Executes the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="callingMethod">The calling method.</param>
        protected void Execute(Action action, [CallerMemberName]string callingMethod = null)
        {
            EnsureNotDisposed();
            Stopwatch stopwatch = new Stopwatch();

            try
            {
                stopwatch.Start();
                LoggerHelper.Debug(Logger, "Started executing '{0}'", GetQualifiedMethodName(callingMethod));

                action?.Invoke();
            }
            catch (Exception)
            {
                LoggerHelper.Error(Logger, "Unhandled Exception occurred in business manager:  {0}", GetQualifiedMethodName(callingMethod));

                // Let unhandled exceptions bubble up
                throw;
            }
            finally
            {
                stopwatch.Stop();

                LoggerHelper.Debug(Logger, "Finished executing '{0}' in {1}", GetQualifiedMethodName(callingMethod), stopwatch.Elapsed);
            }
        }

        /// <summary>
        /// Validates the text.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="regex">The regex.</param>
        /// <param name="result">The result.</param>
        /// <param name="error">The error.</param>
        /// <returns><c>true</c> when the input text is valid according to the specified regex pattern, otherwise <c>false</c>.</returns>
        protected bool ValidateText(string input, string regex, BusinessResult result, ErrorMessage error)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                result.Errors.Add(error);
                return false;
            }
            if (!Regex.IsMatch(input, regex))
            {
                result.Errors.Add(error);
                return false;
            }

            return true;
        }

        #endregion
    }
}
