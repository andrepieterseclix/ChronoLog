using CLog.UI.Common.Helpers;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace CLog.UI.Common.Behaviors
{
    /// <summary>
    /// Represents the behavior that can be attached to a text box to automatically select all the text when the control gets focus.
    /// </summary>
    /// <seealso cref="System.Windows.Interactivity.Behavior{System.Windows.Controls.TextBox}" />
    public sealed class SelectTextOnFocusBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.GotFocus -= AssociatedObject_GotFocus;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Loaded -= AssociatedObject_Loaded;

            AssociatedObject.GotFocus += AssociatedObject_GotFocus;
        }

        private void AssociatedObject_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = e.OriginalSource as TextBox;
            if (textBox == null)
                return;

            e.Handled = true;

            // Mouse click doesn't select the text without a thread, not sure why?
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(10);
                DispatcherHelper.Invoke(textBox.SelectAll);
            });
        }
    }
}
