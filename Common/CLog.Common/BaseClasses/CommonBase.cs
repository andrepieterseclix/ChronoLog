using CLog.Common.Logging;
using System;
using System.Globalization;

namespace CLog.Common.BaseClasses
{
    /// <summary>
    /// Represents the common base class that exposes cross cutting concerns to their derived classes.
    /// </summary>
    public abstract class CommonBase : IDisposable
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonBase"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public CommonBase(ILogger logger)
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

        #endregion

        #region Methods

        /// <summary>
        /// Gets the qualified name of the method.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <returns>The qualified method name.</returns>
        protected string GetQualifiedMethodName(string methodName)
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}.{1}", GetType().Name, methodName);
        }

        #endregion

        #region IDisposable Implementation

        /// <summary>
        /// Keep this private, and create and maintain one for every derived class.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Ensures that this object has not been disposed.
        /// </summary>
        /// <remarks>Every derived class that declares its own <see cref="_disposed"/> field should hide this method and provide its own.</remarks>
        /// <exception cref="System.ObjectDisposedException"></exception>
        protected void EnsureNotDisposed()
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
