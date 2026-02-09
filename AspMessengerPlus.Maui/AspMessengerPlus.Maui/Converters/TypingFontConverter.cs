using System.Globalization;

namespace AspMessengerPlus.Maui.Converters;

public class TypingFontConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool isTyping && isTyping
            ? FontAttributes.Italic
            : FontAttributes.None;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
