using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.VisualTree;
using Project.Modules;

namespace Project.GUI.Models
{
    public class BindingVisibilityConverter : IValueConverter {
        public object Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture) {
            switch(parameter!.ToString()) {
                case "grid":
                    AssetManager? assetManager = value as AssetManager;
                    if(assetManager != null) {
                        if(assetManager.Grid.Name == null) return false;
                        else return true;
                    }
                    return true;

                case "unit":
                    ObservableCollection<AssetManager.UnitInformation>? unitInfo = value as ObservableCollection<AssetManager.UnitInformation>;
                    Debug.WriteLine("Gathering Unit Data");
                    Debug.WriteLine(unitInfo);
                    Debug.WriteLine(unitInfo?.Count);
                    if(unitInfo?.Count != 0) {
                        return true;
                    } else return false;
                    

                default:
                    return true;
            }
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}