using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Data.Converters;
using Project.Modules;

namespace Project.GUI.Models
{
    public class GasOrOilConverter : IValueConverter {
        public object Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture) {
            string? parameterName = parameter?.ToString();
            string? unitName = value?.ToString();
            if(unitName != null && parameterName != null) {
                switch(parameterName) {
                    case "upper":
                        if(unitName.ToLower().StartsWith("oil")) return "Oil";
                        else if(unitName.ToLower().StartsWith("gas")) return "Gas";
                        else return "";

                    case "lower":
                        if(unitName.ToLower().StartsWith("oil")) return "oil";
                        else if(unitName.ToLower().StartsWith("gas")) return "gas";
                        else return "";
                }
            } return "";
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}