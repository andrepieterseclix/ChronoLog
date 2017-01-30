using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CLog.UI.Framework.Testing.Converters
{
    public sealed class IntToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int count = (int)value;

            Visibility v = count > 0
                ? Visibility.Visible
                : Visibility.Collapsed;

            return v;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
