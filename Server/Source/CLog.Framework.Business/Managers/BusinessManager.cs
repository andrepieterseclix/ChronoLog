using CLog.Common.Logging;
using CLog.Framework.Business.Contracts;
using CLog.Framework.Business.Models.Results;
using CLog.Framework.Security;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace CLog.Framework.Business.Managers
{
    /// <summary>
    /// Represents the base class for business managers.
    /// </summary>
    /// <seealso cref="CLog.Framework.Business.Contracts.IBusinessManager" />
    public abstract class BusinessManager : IBusinessManager
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessManager"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public BusinessManager(ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            Logger = logger;
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
                LoggerHelper.Error(Logger, "Exception occurred in business manager:  {0}", GetQualifiedMethodName(callingMethod));

                // Let unhandled exceptions bubble up
                throw;
            }
            finally
            {
                stopwatch.Stop();

                LoggerHelper.Debug(Logger, "Finished executing '{0}' in {1}", GetQualifiedMethodName(callingMethod), stopwatch.Elapsed);
            }
        }

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

        private string GetQualifiedMethodName(string methodName)
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}.{1}", GetType().Name, methodName);
        }

        #endregion

        #region IDisposable Implementation

        /// <summary>
        /// Keep this private, and create and maintain one for every derived class.
        /// </summary>
        private bool _disposed;

        private void EnsureNotDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Release managed resources, and set to null

            }

            // Release native resources
            // NOTE:  call Dispose(false); in finalizer if this class contains unmanaged resources.

            _disposed = true;
        }

        #endregion
    }
}
