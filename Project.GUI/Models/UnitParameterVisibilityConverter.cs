using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Data.Converters;
using Project.Modules;

namespace Project.GUI.Models
{
    public class UnitParameterVisibilityConverter : IValueConverter {
        public object Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture) {
            string? parameterName = parameter?.ToString();
            AssetManager.UnitInformation? unitInfo = value as AssetManager.UnitInformation;
                if(unitInfo != null && parameterName != null) {
                    switch(parameterName) {
                        case "max_el":
                            if(unitInfo.MaxElectricity != null) return true;
                            return false;

                        case "gas_cons":
                            if(unitInfo.GasConsumption != null) return true;
                            return false;
                        
                        case "co2_ems":
                            if(unitInfo.Emissions != null) return true;
                            return false;
                        
                        default: return false;
                    }
                } else return false;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}