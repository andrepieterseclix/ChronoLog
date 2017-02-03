using CLog.UI.Common.Commands;
using CLog.UI.Common.Helpers;
using CLog.UI.Common.Models;
using System;
using System.Diagnostics;
using System.Windows.Input;

namespace CLog.UI.Common.ViewModels
{
    /// <summary>
    /// Represents the basic view model base without providing additional common view model functionality.
    /// </summary>
    /// <seealso cref="CLog.UI.Common.Models.BindableBase" />
    [DebuggerNonUserCode]
    public abstract class BasicViewModelBase : BindableBase
    {
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

        #endregion
    }
}
