using LiveCharts;
using MapOfDiet.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MapOfDiet.Converters
{
    public class TargetValuesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stats = value as IEnumerable<DailyStatistic>;
            return new ChartValues<double>(stats?.Select(s => s.TargetCalories) ?? Enumerable.Empty<double>());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
