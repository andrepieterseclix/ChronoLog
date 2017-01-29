using System.Windows;

namespace CLog.UI.Common.Services
{
    /// <summary>
    /// Represents the contract for interacting with dialogs.
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// Sets the dialog result of the window with the specified name.
        /// </summary>
        /// <param name="dialogResult">The dialog result to set.</param>
        /// <param name="windowName">Name of the window.  If not specified, the active window of the application is used.</param>
        void SetDialogResult(bool dialogResult, string windowName = null);

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        /// <typeparam name="TWindow">The type of the window.</typeparam>
        /// <param name="dataContext">The data context.</param>
        /// <returns>The dialog result.</returns>
        bool ShowDialog<TWindow>(object dataContext = null)
            where TWindow : Window, new();

        /// <summary>
        /// Sets the main window's visibility.
        /// </summary>
        /// <param name="isVisible">if set to <c>true</c> the main window will be visible.</param>
        void SetMainWindowVisible(bool isVisible);

        /// <summary>
        /// Shows the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="caption">The caption.</param>
        void ShowError(string message, string caption = "Error");

        /// <summary>
        /// Shows the information.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="caption">The caption.</param>
        void ShowInfo(string message, string caption = "Info");
    }
}
