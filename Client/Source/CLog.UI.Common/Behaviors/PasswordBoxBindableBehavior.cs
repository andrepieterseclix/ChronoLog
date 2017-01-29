using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace CLog.UI.Common.Behaviors
{
    public sealed class PasswordBoxBindableBehavior : Behavior<PasswordBox>
    {
        public static DependencyProperty PasswordTextProperty = DependencyProperty.Register("PasswordText", typeof(string), typeof(PasswordBoxBindableBehavior), new UIPropertyMetadata(PasswordText_Changed));

        public string PasswordText
        {
            get { return (string)GetValue(PasswordTextProperty); }
            set { SetValue(PasswordTextProperty, value); }
        }

        private static void PasswordText_Changed(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            PasswordBoxBindableBehavior behavior = obj as PasswordBoxBindableBehavior;
            if (behavior == null)
                return;

            if (!Equals(behavior.AssociatedObject.Password, behavior.PasswordText))
                behavior.AssociatedObject.Password = behavior.PasswordText;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PasswordChanged += AssociatedObject_PasswordChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PasswordChanged -= AssociatedObject_PasswordChanged;
        }

        private void AssociatedObject_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!Equals(PasswordText, AssociatedObject.Password))
                PasswordText = AssociatedObject.Password;
        }
    }
}
