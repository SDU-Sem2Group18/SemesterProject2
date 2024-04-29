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
            [Name("name")]
            public string Name { get; set; }

            [Name("image")]
            public string ImagePath { get; set; }
            
            [Name("architecture")]
            public string Architecture { get; set; }

            [Name("size")]
            public int Size { get; set; }
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

        public GridInfo Grid;
        public List<UnitInformation> UnitData;
        
        private string GridSourcePath;
        private string UnitSourcePath;

        public AssetManager() {
            GridSourcePath = "";
            UnitSourcePath = "";
            UnitData = new List<UnitInformation>();
            Grid = new GridInfo();
        }

        public void GetGriddataFromFile(string path) {
            GridInfo gridInfo = new GridInfo();
            List<GridInfo> grids = new List<GridInfo>();

            try {
                using (var reader = new StreamReader(path))
                using (var csv = new CsvReader(reader, new CultureInfo("dk-DK", false))) {
                    var records = csv.GetRecords<GridInfo>();
                    foreach (var record in records) {
                        grids.Add(record);
                    }
                    gridInfo = grids[0];
                }
            } catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                throw new Exception($"Error handling csv file. Ensure your file is formatted correctly. Details: \n{ex.Message}"); 
            }
            Grid = gridInfo;
        }

        public void GetUnitDataFromFile(string path) {
            List<UnitInformation> returnList = new List<UnitInformation>();
            List<UnitInformation> units = new List<UnitInformation>();

            try {
                using (var reader = new StreamReader(path))
                using (var csv = new CsvReader(reader, new CultureInfo("dk-DK", false))) {
                    var records = csv.GetRecords<UnitInformation>();
                    foreach (var record in records) {
                        returnList.Add(record);
                    }
                }
            } catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                throw new Exception($"Error handling csv file. Ensure your file is formatted correctly. Details: \n{ex.Message}"); 
            }
            UnitData = returnList;
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