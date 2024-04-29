using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Data.Converters;

namespace Project.GUI.Models
{
    public class WhiteSpaceAlignmentConverter : IValueConverter {
        public object Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture) {
        if (value is float)
        {
            float number = (float)value;
            int intParameter;
            Int32.TryParse(parameter?.ToString(), out intParameter);
            string returnString = string.Format("{0:0.00}", number);
            return (returnString.Length == intParameter - 1) ?  $"  {returnString}" : returnString;
        }
        return value!;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}