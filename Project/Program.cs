using System.Diagnostics;
using Project.Modules;

namespace Project;

public class Program {
    
    public static void Main() {
        AssetManager assetManager = new AssetManager("/home/nick/Desktop/Uni/SDU Semester 2/Semester Project/Code/Project/Data/GridInfo.csv", "/home/nick/Desktop/Uni/SDU Semester 2/Semester Project/Code/Project/Data/ProductionUnits.csv");

        List<SourceDataManager.HeatData> heatDataList = new List<SourceDataManager.HeatData>();
        using(SourceDataManager sourceDataManager = new SourceDataManager("/home/nick/Desktop/Uni/SDU Semester 2/Semester Project/Code/Project/Data/winter.csv")) {
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