using System;
using System.Windows;
using System.Windows.Threading;

namespace CLog.UI.Common.Helpers
{
    /// <summary>
    /// Represents the dispatcher helper static class.
    /// </summary>
    public static class DispatcherHelper
    {
        /// <summary>
        /// Invokes the specified action through the dispatcher when the calling thread is not the UI thread.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void Invoke(Action callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            if (Application.Current == null)
                return;

            Dispatcher dispatcher = Application.Current.Dispatcher;

            if (dispatcher == null || dispatcher.CheckAccess())
                callback();
            else
                dispatcher.Invoke(callback);
        }
    }
}
