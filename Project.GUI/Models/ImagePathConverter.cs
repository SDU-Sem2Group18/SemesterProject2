using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Project.GUI.Models;
using Project.Modules;

namespace Project.GUI.Models
{
    public class ImagePathConverter : IValueConverter {
        public object Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture) {
            Debug.WriteLine(value?.ToString());
            AssetManager? assetManager = value as AssetManager;
            if(assetManager!= null) {
                switch(parameter!.ToString()) {
                    case "grid":
                        Debug.WriteLine("Using ImagePathConverter");
                        if(assetManager.GridSourcePath == "") return "/Assets/ImageNotFound.png";

                        Debug.WriteLine(assetManager.Grid.ImagePath);
                        Debug.WriteLine(assetManager.GridSourcePath);
                        if(File.Exists(assetManager.Grid.ImagePath)) return assetManager.Grid.ImagePath;
                        else {
                            Uri imagePath = new Uri($"file:///{assetManager.Grid.ImagePath}");
                            Uri csvPath = new Uri($"file://{assetManager.GridSourcePath}");
                            Debug.WriteLine(imagePath.AbsolutePath);
                            Debug.WriteLine(csvPath.AbsolutePath);

                            string? path = Path.GetDirectoryName(csvPath.AbsolutePath);
                            Uri uri = new Uri("file://" + path! + imagePath.AbsolutePath);
                            Debug.WriteLine(@$"{uri.AbsolutePath.Replace("%20", " ")}");
                            if(File.Exists(@$"{uri.AbsolutePath.Replace("%20", " ")}")) return @$"{uri.AbsolutePath.Replace("%20", " ")}";
                            else return "/Assets/ImageNotFound.png";
                        }

                    default:
                        Debug.WriteLine("Image Not Found");
                        return "/Assets/ImageNotFound.png";
                }
            }
            Debug.WriteLine("AssetManager Not Found");
            return "/Assets/ImageNotFound.png";
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}