using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;

namespace Project.Modules
{
    
    public struct ResultData {
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

    public class ResultDataManger {
        private List<ResultData> resultDataList;

        public void SaveResultData(string path){
            
            //Implement The logic to store ResultData
            resultDataList.Add(data);
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
    }
}