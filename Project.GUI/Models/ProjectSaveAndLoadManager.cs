using System;
using System.Collections.Generic;
using System.IO;
using SkiaSharp;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Project.Modules;
using ReactiveUI;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Diagnostics;
using Avalonia.Data.Converters;
using System.Globalization;
using System.Text.Json;
using CsvHelper;

namespace Project.GUI.Models
{
    public class ProjectSaveAndLoadManager
    {
        public struct ProjectData {
            public string FileName;
            public string FilePath;
            public AssetManager.GridInfo GridData;
            public byte[] GridImage;
            public List<AssetManager.UnitInformation> UnitData;
            public List<byte[]> UnitDataImages;
            public List<SourceDataManager.HeatData> HeatData;

            public ProjectData(string? fileName, string? filePath, AssetManager.GridInfo? gridData, List<AssetManager.UnitInformation>? unitData, List<SourceDataManager.HeatData>? heatData) {
                if(fileName != null) FileName = fileName;
                else FileName = "";
                if(filePath != null) FilePath = filePath;
                else FilePath = "";
                if(gridData != null) GridData = (AssetManager.GridInfo)gridData;
                else GridData = new AssetManager.GridInfo();
                if(unitData != null) UnitData = unitData;
                else UnitData = new List<AssetManager.UnitInformation>();
                if(heatData != null) HeatData = heatData;
                else HeatData = new List<SourceDataManager.HeatData>();

                GridImage = new byte[0];
                UnitDataImages = new List<byte[]>();
            }
        }

        public ProjectData CurrentChanges;
        public ProjectData SavedChanges;

        public ProjectSaveAndLoadManager() {
            CurrentChanges = new ProjectData("untitled.hop", null, null, null, null);
            SavedChanges = new ProjectData();

            // Listen To Messages of Data Being Loaded
            MessageBus.Current.Listen<GridDataMessage>().Subscribe(HandleGridDataMessage);
            MessageBus.Current.Listen<UnitDataMessage>().Subscribe(HandleUnitDataMessage);
            MessageBus.Current.Listen<HeatDataMessage>().Subscribe(HandleHeatDataMessage);
        }


        public void HandleGridDataMessage(GridDataMessage message) {
            CurrentChanges.GridData = message.GridInfo.Grid;
            string imagePath = (string)imagePathConverter.Convert(message.GridInfo, typeof(AssetManager), "grid", CultureInfo.InvariantCulture)!;
            if(File.Exists(imagePath)) CurrentChanges.GridImage = SKBitmap.Decode(imagePath).Encode(SKEncodedImageFormat.Png, 100).ToArray();
            MessageBus.Current.SendMessage(new ChangesMadeMessage());
        }
        public void HandleUnitDataMessage(UnitDataMessage message) {
            CurrentChanges.UnitData = message.Units;
            foreach(AssetManager.UnitInformation _ in message.Units) {
                if(CurrentChanges.UnitDataImages == null) CurrentChanges.UnitDataImages = new List<byte[]>();
                string imagePath = (string)imagePathConverter.Convert(_, typeof(AssetManager.UnitInformation), "unit", CultureInfo.InvariantCulture)!;
                if(File.Exists(imagePath)) CurrentChanges.UnitDataImages.Add(SKBitmap.Decode(imagePath).Encode(SKEncodedImageFormat.Png, 100).ToArray());
                else CurrentChanges.UnitDataImages.Add(new byte[0]);
            }
            MessageBus.Current.SendMessage(new ChangesMadeMessage());
        }
        public void HandleHeatDataMessage(HeatDataMessage message) {
            if(CurrentChanges.HeatData == null) CurrentChanges.HeatData = new List<SourceDataManager.HeatData>();
            CurrentChanges.HeatData.AddRange(message.HeatData);
            MessageBus.Current.SendMessage(new ChangesMadeMessage());
        }

        private IValueConverter imagePathConverter = new ImagePathConverter();

        // https://www.codeproject.com/Articles/11271/Read-and-Write-Structures-to-Files-with-NET
        public bool SaveProject(string fileName) {
            try {
                CurrentChanges.FilePath = fileName;
                CurrentChanges.FileName = Path.GetFileName(fileName);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProjectData));
                StreamWriter writer = new StreamWriter(File.Create(fileName));
                xmlSerializer.Serialize(writer, CurrentChanges);
                writer.Close();

                SavedChanges = CurrentChanges;
                return true;
            } catch (Exception) {
                return false;
            }
        }
        public (bool, string?, string?, string?, string?) ReadProjectFromFile(string fileName) {
            try {
                CurrentChanges.FilePath = fileName;
                CurrentChanges.FileName = Path.GetFileName(fileName);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProjectData));
                StreamReader reader = new StreamReader(File.OpenRead(fileName));
                object? returnObject = xmlSerializer.Deserialize(reader);

                string? _tempPath;
                string? _gridDataPath = null;
                string? _unitDataPath = null;
                string? _sourceDataPath = null;

                if(returnObject != null) {
                    ProjectData returnData = (ProjectData)returnObject;
                    _tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                    Directory.CreateDirectory(_tempPath);

                    // Grid Data
                    if(returnData.GridData.Name != null) {
                        _gridDataPath = Path.Combine(_tempPath, Guid.NewGuid().ToString() + ".csv");

                        // Save the Images to a Folder and add the new file References
                        if(returnData.GridImage.Length > 0) {
                            SKImage _image = SKImage.FromEncodedData(returnData.GridImage);
                            string _imagePath = Path.Combine(_tempPath, Guid.NewGuid().ToString() + ".png");
                            returnData.GridData.ImagePath = _imagePath;
                            File.WriteAllBytes(_imagePath, _image.Encode(SKEncodedImageFormat.Png, 100).ToArray());
                        }

                        var file = File.Create(_gridDataPath);
                        file.Close();

                        using(var writer = new StreamWriter(_gridDataPath, true))
                        using(var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) {
                            List<AssetManager.GridInfo> writeList = new List<AssetManager.GridInfo>();
                            writeList.Add(returnData.GridData);
                            csv.WriteRecords(writeList);
                        }
                    }

                    if(returnData.UnitData.Count > 0) {
                        List<AssetManager.UnitInformation> writeList = new List<AssetManager.UnitInformation>();
                        for(int i = 0; i < returnData.UnitData.Count; ++i) {
                            // Check if image exists
                            if(returnData.UnitDataImages[i].Length > 0) {
                                SKImage _image = SKImage.FromEncodedData(returnData.UnitDataImages[i]);
                                string _imagePath = Path.Combine(_tempPath, Guid.NewGuid().ToString() + ".png");
                                returnData.UnitData[i].ImagePath = _imagePath;
                                File.WriteAllBytes(_imagePath, _image.Encode(SKEncodedImageFormat.Png, 100).ToArray());
                            }
                            writeList.Add(returnData.UnitData[i]);
                        }

                        _unitDataPath = Path.Combine(_tempPath, Guid.NewGuid().ToString() + ".csv");
                        var file = File.Create(_unitDataPath);
                        file.Close();

                        using(var writer = new StreamWriter(_unitDataPath, true))
                        using(var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) {
                            csv.WriteRecords(writeList);
                        }
                    }

                    if(returnData.HeatData.Count > 0) {
                        List<SourceDataManager.HeatData> writeList = new List<SourceDataManager.HeatData>();
                        for(int i = 0; i < returnData.HeatData.Count; ++i) {
                            writeList.Add(returnData.HeatData[i]);
                        }
                        _sourceDataPath = Path.Combine(_tempPath, Guid.NewGuid().ToString() + ".csv");
                        var file = File.Create(_sourceDataPath);
                        file.Close();
                        using(var writer = new StreamWriter(_sourceDataPath, true))
                        using(var csv = new CsvWriter(writer, CultureInfo.GetCultureInfo("dk-DK"))) {
                            csv.WriteRecords(writeList);
                        }
                    }
                    
                    return (true, _tempPath, _gridDataPath, _unitDataPath, _sourceDataPath);
                } else return (false, null, null, null, null);
                
            } catch (Exception) {
                return (false, null, null, null, null);
            }
        }

    }
}