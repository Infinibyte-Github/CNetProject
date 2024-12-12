namespace CNetProject;

using System;
using System.Globalization;

public class BookDetailConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Book book)
        {
            return $"{book.Format} | Author: {book.Author} | Pages: {book.Pages}";
        }
        return "Unknown Book Details";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Not needed for this case
        return null;
    }
}
