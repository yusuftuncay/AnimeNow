using AnimeNow.Models;
using AnimeNow.ViewModels;
using System.Globalization;

namespace AnimeNow.Converters
{
    // AnimeDetailPage & SettingsPage
    public class PickerDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AniListAnimeDetail_Episode ep)
                return $"Episode {ep.Number}";
            else if (value is Hostname hn)
                return hn.Key;
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
