using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace MassRename.Views.Avalonia.Converters;

public class AddConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            int a when parameter is int b => a + b,
            int a when parameter is string b => a + int.Parse(b),
            _ => throw new ArgumentException()
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}