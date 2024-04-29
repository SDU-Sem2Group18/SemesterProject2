using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Data.Converters;

namespace Project.GUI.Models
{
    public class IntHidingConverter : IValueConverter {
        public object Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture) {
            int number;
            if (int.TryParse(value!.ToString(), out number)) {
                if(number == 0) return "";
                else return value;
            }
            return value!;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}