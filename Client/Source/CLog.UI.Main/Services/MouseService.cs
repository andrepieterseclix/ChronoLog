using CLog.UI.Common.Helpers;
using CLog.UI.Common.Services;
using System.Windows.Input;

namespace CLog.UI.Main.Services
{
    public sealed class MouseService : IMouseService
    {
        /// <summary>
        /// Sets the override mouse cursor to the busy indicator.
        /// </summary>
        /// <param name="waiting">if set to <c>true</c> the cursor will display the busy indicator, otherwise the normal cursor will be displayed.</param>
        public void SetWait(bool waiting)
        {
            DispatcherHelper.Invoke(() =>
            {
                Mouse.OverrideCursor = waiting
                ? Cursors.Wait
                : null;
            });
        }
    }
}
