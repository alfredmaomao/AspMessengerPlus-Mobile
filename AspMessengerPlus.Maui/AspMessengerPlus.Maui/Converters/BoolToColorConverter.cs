using System.Globalization;
using Microsoft.Maui.Graphics;

namespace AspMessengerPlus.Maui.Converters;

public class BoolToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isMine)
            return isMine ? Color.FromArgb("#C8F7C5") : Color.FromArgb("#EEEEEE");

        return Color.FromArgb("#EEEEEE");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
