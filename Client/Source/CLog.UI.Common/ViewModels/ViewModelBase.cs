﻿using CLog.Common.Logging;
using CLog.UI.Common.Commands;
using CLog.UI.Common.Helpers;
using CLog.UI.Common.Messaging.Mediator;
using System;
using System.Windows.Input;

namespace CLog.UI.Common.ViewModels
{
    /// <summary>
    /// Represents the base class for view models.
    /// </summary>
    /// <seealso cref="CLog.UI.Common.ViewModels.BindableBase" />
    /// <seealso cref="System.IDisposable" />
    public abstract class ViewModelBase : BindableBase, IDisposable
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public ViewModelBase(ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            Logger = logger;

            // Register all decorated methods to the Mediator
            Mediator.Instance.Register(this);
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
        /// Gets the mediator that implements the messaging pattern.
        /// </summary>
        /// <value>
        /// The mediator.
        /// </value>
        public static Mediator Mediator
        {
            get { return Mediator.Instance; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialises the view model.
        /// </summary>
        public virtual void Initialise()
        {
        }

        /// <summary>
        /// Clears the context of the view model.
        /// </summary>
        public virtual void ClearContext()
        {
        }

        /// <summary>
        /// Invokes the specified callback on the UI thread.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public static void Invoke(Action callback)
        {
            DispatcherHelper.Invoke(callback);
        }

        /// <summary>
        /// Creates an <see cref="ICommand"/> object with the specified delegates.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        /// <returns>A command object.</returns>
        public static ICommand CreateCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            return new DelegateCommand(execute, canExecute);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return GetType().Name;
        }

        #endregion

        #region IDisposable Implementation

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
            if (disposing)
            {
                // Release managed resources

                // Unregister all decorated methods to the Mediator
                Mediator.Instance.Unregister(this);
            }

            // Release native resources
            // NOTE:  call Dispose(false); in finalizer if this class contains unmanaged resources.
        }

        #endregion
    }
}
