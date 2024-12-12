namespace CNetProject;

using System.Globalization;


public class MovieDetailConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Movie movie)
        {
            return $"{movie.Format} | Directed by: {movie.Director} | Year: {movie.ReleaseYear}";
        }
        return "Unknown Movie Details";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Not needed for this case
        return null;
    }
}