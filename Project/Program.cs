using System.Diagnostics;
using Project.Modules;

namespace Project;

public class Program {
    
    public static void Main() {
        AssetManager assetManager = new AssetManager("C:/Users/Nick/Semester Project/SemesterProject2/Project/Data/GridInfo.csv", "C:/Users/Nick/Semester Project/SemesterProject2/Project/Data/ProductionUnits.csv");

        List<SourceDataManager.HeatData> heatDataList = new List<SourceDataManager.HeatData>();
        using(SourceDataManager sourceDataManager = new SourceDataManager("C:/Users/Nick/Semester Project/SemesterProject2/Project/Data/summer.csv")) {
            heatDataList = sourceDataManager.GetHeatData();
        }

        List<Optimiser.OptimisedData> costOptimisedDataList = new List<Optimiser.OptimisedData>();
        List<Optimiser.OptimisedData> emissionOptimisedDataList = new List<Optimiser.OptimisedData>();

        Optimiser optimiser = new Optimiser(assetManager.UnitData, heatDataList);
        (costOptimisedDataList, emissionOptimisedDataList) = optimiser.Optimise(heatDataList);
        
        using(ResultDataManager costResultDataManager = new ResultDataManager(costOptimisedDataList, "Data/costOptimised.csv")) {
            costResultDataManager.SaveResultData(true);
        }
        using(ResultDataManager emissionResultDataManager = new ResultDataManager(emissionOptimisedDataList, "Data/emissionOptimised.csv")) {
            emissionResultDataManager.SaveResultData(true);
        }

    }
}