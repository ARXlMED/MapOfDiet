using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MapOfDiet.Converters
{
    class GenderToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is char gender && parameter is string param)
            {
                return gender.ToString() == param;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool val && val && parameter is string param)
            {
                return param[0];
            }
            return Binding.DoNothing;
        }
    }
}
