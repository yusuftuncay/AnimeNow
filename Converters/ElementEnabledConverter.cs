using System.Globalization;

namespace AnimeNow.Converters
{
    // AnimeDetailPage
    public class ElementEnabledConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || !targetType.IsAssignableFrom(typeof(bool)))
                return false;

            foreach (var value in values)
                if (value is not bool b || !b)
                    return false;

            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
