using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Modules
{
    
    public class ResultData {
        public int unitId { get; set; }
        public float producedHeat { get; set; }
        public float producedElectricity { get; set; }
        public float consumedElectricity { get; set; }
        public float productionCost { get; set; }
        public float energyConsumed { get; set; }
        public float CO2Emissions { get; set; }
    }

    public class ResultDataManger {
        private List<ResultData> resultDataList;

        public void StoreResultData(ResultData data){
            
            //Implement The logic to store ResultData
            resultDataList.Add(data);
        }

        public ResultData RetrieveResltData(int unitId){
            // Implement the logic to retrieve a specific ResultData by unitId
            return resultDataList.Find(data => data.unitId == unitId);
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