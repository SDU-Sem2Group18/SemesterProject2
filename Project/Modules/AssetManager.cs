using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System.Globalization;
using System.Diagnostics;

namespace Project.Modules
{
    public class AssetManager: IDisposable {

        public struct GridInfo {
            private string Name;
            private string ImagePath;

            public GridInfo(string name, string imagePath) {
                Name = name;
                ImagePath = imagePath;
            }

            public string GetName() {
                return Name;
            }

            public string GetImage() {
                return ImagePath;
            }

        }

        public struct UnitInformation {
            [Name("name")]
            public string Name { get; set; }

            [Name("image")]
            public string ImagePath { get; set; }

            [Name("max_heat")]
            public float MaxHeat { get; set; }

            [Name("prod_cost")]
            public float ProductionCost { get; set; }
            
            [Name("max_el")]
            [NullValues("-")]
            public float? MaxElectricity { get; set; }

            [Name("gas_cons")]
            [NullValues("-")]
            public float? GasConsumption { get; set; }

            [Name("co2_ems")]
            [NullValues("-")]
            public float? Emissions { get; set; }
        }

        private GridInfo Grid;
        public Dictionary<string, UnitInformation> UnitData;
        
        private string SourcePath;

        public AssetManager(string sourcePath) {
            Grid = new(
                "Heatington",
                "/Assets/Images/Heatington.png"
            );
            SourcePath = sourcePath;
            UnitData = new Dictionary<string, UnitInformation>();
            GetUnitDataFromFile(sourcePath);
        }

        public GridInfo GetGridInfo() {
            return Grid;
        }

        public void GetUnitDataFromFile(string path) {
            Dictionary<string, UnitInformation> returnDictionary = new Dictionary<string, UnitInformation>();
            List<UnitInformation> units = new List<UnitInformation>();

            try {
                using (var reader = new StreamReader(path))
                using (var csv = new CsvReader(reader, new CultureInfo("dk-DK", false))) {
                    var records = csv.GetRecords<UnitInformation>();
                    foreach (var record in records) {
                        returnDictionary.Add(record.Name, record);
                    }
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                throw new Exception($"Error handling csv file. Ensure your file is formatted correctly. Details: \n{ex.Message}"); 
            }
            UnitData = returnDictionary;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected bool Disposed {get; private set; }
        protected virtual void Dispose(bool disposing) {
            Disposed = true;
        }
    }
}