using CLog.Common.Logging;
using CLog.Framework.Business.Messages;
using CLog.Framework.Security;
using CLog.Framework.Services.Contracts;
using CLog.Framework.Services.Extensions;
using CLog.Framework.Services.Models;
using System;
using System.ServiceModel;
using System.Threading;

namespace CLog.Services.Common
{
    /// <summary>
    /// Represents the base class for micro service implementations.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, IncludeExceptionDetailInFaults = true)]
    public abstract class ServiceBase : IService
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBase" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="accessManager">The access manager.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public ServiceBase(ILogger logger)
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

        /// <summary>
        /// Gets a value indicating whether this instance is anonymous.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is anonymous; otherwise, <c>false</c>.
        /// </value>
        protected bool IsAnonymous
        {
            get { return UserIdentity is AnonymousServerIdentity; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes the specified action.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="action">The action.</param>
        /// <returns>
        /// The response.
        /// </returns>
        public TResponse Execute<TResponse>(Action<TResponse> action)
            where TResponse : ResponseBase, new()
        {
            EnsureNotDisposed();
            TResponse response = new TResponse();

            try
            {
                ServerIdentity identity = UserIdentity;

                if (!IsAnonymous && identity.SessionExpired)
                {
                    response.SessionExpired = identity.SessionExpired;
                    response.Errors.Add(ErrorMessages.SessionExpired.Map());

                    return response;
                }

                action?.Invoke(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add(ErrorMessages.UnhandledBusinessException.Map());

                LoggerHelper.Fatal(Logger, ex, "Unhandled Exception");
            }

            return response;
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
