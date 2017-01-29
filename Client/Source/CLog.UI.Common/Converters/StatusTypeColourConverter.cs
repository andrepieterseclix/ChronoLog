using CLog.UI.Common.Services;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CLog.UI.Common.Converters
{
    /// <summary>
    /// Represents the value converter for <see cref="StatusMessageType"/> to a colour brush.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public sealed class StatusTypeColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            StatusMessageType type = (StatusMessageType)value;

            switch (type)
            {
                case StatusMessageType.Info:
                    return Brushes.SlateGray;
                case StatusMessageType.Warning:
                    return Brushes.OrangeRed;
                case StatusMessageType.Error:
                    return Brushes.Red;
                default:
                    return Brushes.Black;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
