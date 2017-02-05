using CLog.UI.Common.Helpers;
using CLog.UI.Common.Services;
using CLog.UI.Main.Views;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CLog.UI.Main.Services
{
    /// <summary>
    /// Represents the implementation of the dialog service.
    /// </summary>
    /// <seealso cref="CLog.UI.Common.Services.IDialogService" />
    public class DialogService : IDialogService
    {
        #region Fields

        private readonly Rectangle _overlayRectangle = new Rectangle()
        {
            Fill = Brushes.Gray,
            Opacity = 0.4d
        };

        #endregion

        /// <summary>
        /// Applies the main window overlay.
        /// </summary>
        /// <param name="showOverlay">if set to <c>true</c>, show overlay.</param>
        public void ApplyMainWindowOverlay(bool showOverlay)
        {
            DispatcherHelper.Invoke(() =>
            {
                ShellWindow window = Application.Current.MainWindow as ShellWindow;
                if (window == null)
                    return;

                window.LayoutRoot.IsEnabled = !showOverlay;

                if (showOverlay && !window.LayoutRoot.Children.Contains(_overlayRectangle))
                    window.LayoutRoot.Children.Add(_overlayRectangle);
                else if (!showOverlay && window.LayoutRoot.Children.Contains(_overlayRectangle))
                    window.LayoutRoot.Children.Remove(_overlayRectangle);
            });
        }

        /// <summary>
        /// Sets the dialog result of the window with the specified name.
        /// </summary>
        /// <param name="dialogResult">The dialog result to set.</param>
        /// <param name="windowName">Name of the window.  If not specified, the active window of the application is used.</param>
        public void SetDialogResult(bool dialogResult, string windowName = null)
        {
            Window activeWindow = null;

            foreach (Window window in Application.Current.Windows)
            {
                if (window.IsActive)
                    activeWindow = window;

                if (window.Name == windowName)
                {
                    activeWindow = window;
                    break;
                }
            }

            if (activeWindow != null)
                activeWindow.DialogResult = dialogResult;
        }

        /// <summary>
        /// Sets the main window's visibility.
        /// </summary>
        /// <param name="isVisible">if set to <c>true</c> the main window will be visible.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SetMainWindowVisible(bool isVisible)
        {
            if (isVisible)
                Application.Current.MainWindow.Show();
            else
                Application.Current.MainWindow.Hide();
        }

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        /// <typeparam name="TWindow">The type of the window.</typeparam>
        /// <param name="dataContext">The data context.</param>
        /// <returns>The dialog result.</returns>
        public bool ShowDialog<TWindow>(object dataContext = null)
            where TWindow : Window, new()
        {
            TWindow window = new TWindow()
            {
                DataContext = dataContext
            };

            Window mainWindow = Application.Current.MainWindow;
            if (mainWindow.IsVisible)
                window.Owner = mainWindow;

            return window.ShowDialog().Value;
        }

        /// <summary>
        /// Shows the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="caption">The caption.</param>
        public void ShowError(string message, string caption = "Error")
        {
            DispatcherHelper.Invoke(() => MessageBox.Show(Application.Current.MainWindow, message, "Error", MessageBoxButton.OK, MessageBoxImage.Error));
        }

        /// <summary>
        /// Shows the information.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="caption">The caption.</param>
        public void ShowInfo(string message, string caption = "Info")
        {
            DispatcherHelper.Invoke(() => MessageBox.Show(Application.Current.MainWindow, message, caption, MessageBoxButton.OK, MessageBoxImage.Information));
        }
    }
}
