using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Ceos.Interface.WPF.Converters
{
    /// <summary>
    /// This converter combines two converters in one.
    /// The converting process first convert the object throw the FirstConverter than the SecondConverter.
    /// </summary>
    public class CombiningConverter : IValueConverter
    {
        public IValueConverter FirstConverter { get; set; }
        public IValueConverter SecondConverter { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object firstConversion = FirstConverter.Convert(value, targetType, parameter, culture);
            return SecondConverter.Convert(firstConversion, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object firstConversionBack = SecondConverter.ConvertBack(value, targetType, parameter, culture);
            return FirstConverter.Convert(firstConversionBack, targetType, parameter, culture);
        }
    }
}
