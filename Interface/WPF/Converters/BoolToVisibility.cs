using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Ceos.Interface.WPF.Converters
{
    /// <summary>
    /// Converts a boolean value into a Visibility value.
    /// If the boolean is true, the conversion results in Visible. Otherwise, it results, by default, in Collapsed.
    /// The parameter value can be used to set the false value conversion result.
    /// </summary>
    public class BoolToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value)
                {
                    return Visibility.Visible;
                }
                else
                {
                    string falseVisibility = parameter as string;
                    if (falseVisibility == null || !Enum.IsDefined(typeof(Visibility), falseVisibility))
                    {
                        return Visibility.Collapsed;
                    }
                    else
                    {
                        return (Visibility)Enum.Parse(typeof(Visibility), falseVisibility);
                    }
                }
            }
            else
                return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Visibility)
            {
                if ((Visibility)value == Visibility.Visible)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return DependencyProperty.UnsetValue;
        }
    }
}
