using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace CLog.UI.Common.Behaviors
{
    /// <summary>
    /// Represents the Selected Item tree view behavior.
    /// </summary>
    /// <seealso cref="System.Windows.Interactivity.Behavior{System.Windows.Controls.TreeView}" />
    public class SelectedItemTreeViewBehavior : Behavior<TreeView>
    {
        public static DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(SelectedItemTreeViewBehavior));

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SelectedItemChanged += TreeView_SelectedItemChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.SelectedItemChanged -= TreeView_SelectedItemChanged;
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItem = e.NewValue;
        }
    }
}
