using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace Project.Modules
{
    [Delimiter(",")]
    [CultureInfo("dk-DK")]
    [InjectionOptions(CsvHelper.Configuration.InjectionOptions.Exception)]
    public class ResultData {
        [Index(0)]
        [Name("Unit Name")]
        public AssetManager.UnitNames unitName { get; set; }

        [Index(1)]
        [Name("Produced Heat (MWh)")]
        public float producedHeat { get; set; }

        [Index(2)]
        [Name("Produced Electricity (MWh)")]
        public float producedElectricity { get; set; }

        [Index(3)]
        [Name("Consumed Electricity (MWh)")]
        public float consumedElectricity { get; set; }

        [Index(4)]
        [Name("Energy Consumed (MWh)")]
        public float energyConsumed { get; set; }
        
        
        [Index(5)]
        [Name("CO2 Emissions (tonnes)")]
        public float CO2Emissions { get; set; }
        
        
        
        [Index(6)]
        [Name("Production Cost (Kr)")]
        public float productionCost { get; set; }
        
        
        [Index(7)]
        [Name("Profit (Kr)")]
        public float profit { get; set; }
    }

    public class ResultDataManger: IDisposable {
        private List<ResultData>? resultDataList;

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
            long length = fi.Length; 
            
            using(var writer = new StreamWriter(path, !overwrite))
            using(var csv = new CsvWriter(writer, CultureInfo.GetCultureInfo("dk-DK"))) {
                if(length != 0 && !overwrite) return false;
                else {
                    try {csv.WriteRecords(resultDataList);}
                    catch(CsvHelper.WriterException) {
                        return false;
                    }
                }
            }
            return true;
        }

        public ResultData RetrieveResultData(AssetManager.UnitNames unitName){
            // Implement the logic to retrieve a specific ResultData by unitId
            return resultDataList.Find(data => data.unitName == unitName);
        }

        public List<ResultData> RetrieveAllResultData(){
            
            //Return the entire list of ResultData
            return resultDataList;
        }

        //Constructor for ResultDataManager
        public List<ResultData> ResultDataManager(){
            resultDataList = new List<ResultData>();
            return resultDataList;
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