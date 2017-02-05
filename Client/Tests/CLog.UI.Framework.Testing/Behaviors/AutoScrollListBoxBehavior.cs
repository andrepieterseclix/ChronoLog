using System.Collections.Specialized;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace CLog.UI.Framework.Testing.Behaviors
{
    public class AutoScrollListBoxBehavior : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        private void AssociatedObject_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            AssociatedObject.Loaded -= AssociatedObject_Loaded;

            INotifyCollectionChanged observable = AssociatedObject.ItemsSource as INotifyCollectionChanged;

            if (observable == null)
                return;

            observable.CollectionChanged += Observable_CollectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            INotifyCollectionChanged observable = AssociatedObject.ItemsSource as INotifyCollectionChanged;

            if (observable == null)
                return;

            observable.CollectionChanged -= Observable_CollectionChanged;
        }

        private void Observable_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (AssociatedObject.Items.Count > 0)
                AssociatedObject.ScrollIntoView(AssociatedObject.Items[AssociatedObject.Items.Count - 1]);
        }
    }
}
