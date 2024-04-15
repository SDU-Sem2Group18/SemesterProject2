using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using Microsoft.VisualBasic;

namespace Project.Modules
{
    public class ResultDataManger: IDisposable {

        [Delimiter(",")]
        [CultureInfo("dk-DK")]
        [InjectionOptions(CsvHelper.Configuration.InjectionOptions.Exception)]
        public class ResultData {
            [Index(0)]
            [Name("Unit Name")]
            public string UnitName { get; set; }

            [Index(1)]
            [Name("Produced Heat (MWh)")]
            public float ProducedHeat { get; set; }

            [Index(2)]
            [Name("Produced Electricity (MWh)")]
            public float ProducedElectricity { get; set; }

            [Index(3)]
            [Name("Consumed Electricity (MWh)")]
            public float ConsumedElectricity { get; set; }

            [Index(4)]
            [Name("Energy Consumed (MWh)")]
            public float EnergyConsumed { get; set; }
            
            
            [Index(5)]
            [Name("CO2 Emissions (tonnes)")]
            public float CO2Emissions { get; set; }
            
            
            
            [Index(6)]
            [Name("Production Cost (Kr)")]
            public float ProductionCost { get; set; }
            
            
            [Index(7)]
            [Name("Profit (Kr)")]
            public float Profit { get; set; }

            public ResultData(
            AssetManager.UnitNames unitName, 
            float producedHeat, 
            float producedELectricity, 
            float consumedElectricity, 
            float energyConsumed, 
            float co2Emissions, 
            float productionCost, 
            float profit) {
                UnitName = unitName.ToString();
                ProducedHeat = producedHeat;
                ProducedElectricity = producedELectricity;
                ConsumedElectricity = consumedElectricity;
                EnergyConsumed = energyConsumed;
                CO2Emissions = co2Emissions;
                ProductionCost = productionCost;
                Profit = profit;
            }
        }

        public List<ResultData> ResultDataList = new List<ResultData>();

        public bool SaveResultData(string path, bool overwrite){
            // Check if file path is valid
            FileInfo? fi = null;
            try {
                fi = new FileInfo(path);
            }
            catch (ArgumentException) { }
            catch (PathTooLongException) { }
            catch (NotSupportedException) { }
            if (ReferenceEquals(fi, null)) return false;

            long length;
            if (File.Exists(path)) {
                using(var file = File.Open(path, FileMode.Open)) {
                    length = file.Length;
                }
            } 
            else {
                using(var file = File.Create(path)) {}
                length = 0;
            }
            
            using(var writer = new StreamWriter(path, overwrite))
            using(var csv = new CsvWriter(writer, CultureInfo.GetCultureInfo("dk-DK"))) {
                if(length != 0 && !overwrite) return false;
                else {
                    try {csv.WriteRecords(ResultDataList);}
                    catch(CsvHelper.WriterException) {
                        return false;
                    }
                }
            }
            return true;
        }

        public List<ResultData>? GetResultData(){
            
            //Return the entire list of ResultData
            return ResultDataList;
        }

        //Constructor for ResultDataManager
        public void ResultDataManager(){
            //ResultDataList = new List<ResultData>();
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