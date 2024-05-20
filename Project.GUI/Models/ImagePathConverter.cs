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

            switch(parameter!.ToString()) {
                case "grid":
                AssetManager? assetManager = value as AssetManager;
                if(assetManager != null) {
                    Debug.WriteLine("Using ImagePathConverter for Grid");
                    if(assetManager.GridSourcePath == "") return "/Assets/ImageNotFound.png";

                    Debug.WriteLine(assetManager.Grid.ImagePath);
                    Debug.WriteLine(assetManager.GridSourcePath);

                    // Logic to create final URI
                    if(File.Exists(assetManager.Grid.ImagePath)) return assetManager.Grid.ImagePath;
                    else {
                        // Path in CSV is not an absolute path, logic to create absolute path from relative path provided in CSV
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
                } return "/Assets/ImageNotFound.png";
                
                // Same logic as above, but accessing data in a different way due to working with units
                case "unit":
                    AssetManager.UnitInformation? unitInfo = value as AssetManager.UnitInformation;
                    if(unitInfo != null) {
                        Debug.WriteLine("Using ImagePathConverter for Units");
                        if(unitInfo.ImagePath == "") return "/Assets/ImageNotFound.png";

                        Debug.WriteLine(unitInfo.ImagePath);
                        Debug.WriteLine(unitInfo.SelfPath);
                        if(File.Exists(unitInfo.ImagePath)) return unitInfo.ImagePath;
                        else {
                            Uri imagePath = new Uri($"file:///{unitInfo.ImagePath}");
                            Uri csvPath = new Uri($"file://{unitInfo.SelfPath}");
                            Debug.WriteLine(imagePath.AbsolutePath);
                            Debug.WriteLine(csvPath.AbsolutePath);

                            string? path = Path.GetDirectoryName(csvPath.AbsolutePath);
                            Uri uri = new Uri("file://" + path! + imagePath.AbsolutePath);
                            Debug.WriteLine(@$"{uri.AbsolutePath.Replace("%20", " ")}");
                            if(File.Exists(@$"{uri.AbsolutePath.Replace("%20", " ")}")) return @$"{uri.AbsolutePath.Replace("%20", " ")}";
                            else return "/Assets/ImageNotFound.png";
                        }
                    } return "/Assets/ImageNotFound.png";

                default:
                    Debug.WriteLine("Image Not Found");
                    return "/Assets/ImageNotFound.png";
            }
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}