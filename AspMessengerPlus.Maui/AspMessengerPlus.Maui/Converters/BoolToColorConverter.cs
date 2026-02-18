using System.Globalization;
using Microsoft.Maui.Graphics;

namespace AspMessengerPlus.Maui.Converters;

public class BoolToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isMine)
        {
            return isMine
                ? Color.FromArgb("#2563EB")  
                : Color.FromArgb("#1E293B");  
        }

        return Color.FromArgb("#1E293B");
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
