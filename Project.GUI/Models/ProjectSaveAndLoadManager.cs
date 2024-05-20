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
        public void SaveProject(string fileName) {
            try {
                CurrentChanges.FilePath = fileName;
                CurrentChanges.FileName = Path.GetFileName(fileName);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProjectData));
                StreamWriter writer = new StreamWriter(File.Create(fileName));
                xmlSerializer.Serialize(writer, CurrentChanges);
                writer.Close();

                SavedChanges = CurrentChanges;
            } catch (Exception) {
                throw;
            }
        }
        public void ReadProjectFromFile(string fileName) {
            byte[] buffer = new byte[Marshal.SizeOf(SavedChanges)];
            ProjectData? returnObject = null;
            try {
                using(FileStream _fs = new FileStream(fileName, FileMode.Open)) {
                    _fs.Read(buffer, 0, buffer.Length);
                    GCHandle h = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                    returnObject = (ProjectData)Marshal.PtrToStructure(h.AddrOfPinnedObject(), typeof(ProjectData))!;
                    h.Free();
                    if(_fs.Position >= _fs.Length) _fs.Close();
                }

                CurrentChanges = (ProjectData)returnObject;
                SavedChanges = (ProjectData)returnObject;
            } catch (Exception) {
                throw;
            }
        }

    }
}