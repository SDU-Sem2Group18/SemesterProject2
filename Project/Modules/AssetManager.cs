using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System.Globalization;
using System.Diagnostics;
using CsvHelper.Configuration;

namespace Project.Modules
{
    public class AssetManager {

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

        public class UnitInformation {
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

            public string SelfPath { get; set;}
            public void SetSelfPath(string path) {
                SelfPath = path;
            }
        }

        public sealed class UnitInformationMap : ClassMap<UnitInformation> {
            public UnitInformationMap() {
                AutoMap(new CultureInfo("dk-DK", false));
                Map(m => m.SelfPath).Ignore();
            }
        }

        public GridInfo Grid;
        public List<UnitInformation> UnitData;
        
        public string GridSourcePath;
        public string UnitSourcePath;

        public AssetManager(string gridPath, string unitPath) {
            GridSourcePath = gridPath;
            UnitSourcePath = unitPath;
            UnitData = new List<UnitInformation>();
            Grid = new GridInfo();
            if(GridSourcePath != "") GetGridDataFromFile();
            if(UnitSourcePath != "") GetUnitDataFromFile();
        }

        public void GetGridDataFromFile() {
            GridInfo gridInfo = new GridInfo();
            List<GridInfo> grids = new List<GridInfo>();

            try {
                using (var reader = new StreamReader(GridSourcePath))
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

        public void GetUnitDataFromFile() {
            List<UnitInformation> returnList = new List<UnitInformation>();

            try {
                using (var reader = new StreamReader(UnitSourcePath))
                using (var csv = new CsvReader(reader, new CultureInfo("dk-DK", false))) {
                    csv.Context.RegisterClassMap<UnitInformationMap>();
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
    }
}