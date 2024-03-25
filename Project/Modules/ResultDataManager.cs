using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Modules
{
    
    public class ResultData
{
    private int unitId;
    private float producedHeat;
    private float producedElectricity;
    private float consumedElectricity;
    private float productionCost;
    private float energyConsumed;
    private float CO2Emissions;

    public int GetUnitId()
    {
        return unitId;
    }

    public float GetProducedHeat()
    {
        return producedHeat;
    }

    public float GetProducedElectricity()
    {
        return producedElectricity;
    }

    public float GetConsumedElectricity()
    {
        return consumedElectricity;
    }

    public float GetProductionCost()
    {
        return productionCost;
    }

    public float GetEnergyConsumed()
    {
        return energyConsumed;
    }

    public float GetCO2Emissions()
    {
        return CO2Emissions;
    }

    // Assuming there might be constructors and setters 
    // You would also add those here based on our requirements
}
    public class ResultDataManger
    {
        private List<ResultData> resultDataList;

        public void StoreResultData(ResultData data){
            
            //Implement The logic to store ResultData
            resultDataList.Add(data);
        }

        public ResultData RetrieveResltData(int unitId){
            // Implement the logic to retrieve a specific ResultData by unitId
            return resultDataList.Find(data => data.GetUnitId() == unitId);
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