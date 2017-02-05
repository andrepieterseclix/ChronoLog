using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace CLog.UI.Common.Behaviors
{
    public sealed class BindingValidationBehavior : Behavior<TextBox>
    {
        public static DependencyProperty ValidationErrorsProperty = DependencyProperty.Register("ValidationErrors", typeof(ObservableCollection<string>), typeof(BindingValidationBehavior));

        /// <summary>
        /// Gets or sets the validation errors.
        /// </summary>
        /// <value>
        /// The validation errors.
        /// </value>
        public ObservableCollection<string> ValidationErrors
        {
            get { return (ObservableCollection<string>)GetValue(ValidationErrorsProperty); }
            set { SetValue(ValidationErrorsProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            // Validate
            Binding binding = BindingOperations.GetBinding(AssociatedObject, TextBox.TextProperty);

            if (!binding.NotifyOnValidationError)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "{0} requires the '{1}' property of the binding expression to be set to true.", nameof(BindingValidationBehavior), nameof(binding.NotifyOnValidationError)));

            // Subscribe to event handler
            Validation.AddErrorHandler(AssociatedObject, ErrorHandler);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            Validation.RemoveErrorHandler(AssociatedObject, ErrorHandler);
        }

        private void ErrorHandler(object sender, ValidationErrorEventArgs e)
        {
            if (ValidationErrors == null)
                return;

            string message =
                e.Error?.ErrorContent?.ToString() ??
                e.Error?.Exception?.Message;

            if (e.Action == ValidationErrorEventAction.Removed)
                ValidationErrors.Remove(message);
            else if (e.Action == ValidationErrorEventAction.Added)
                ValidationErrors.Add(message);
        }
    }
}
