using Project.Modules;

namespace Project;

public class Program {
    
    public static void Main() {
        AssetManager assetManager = new AssetManager("/home/nick/Desktop/Uni/SDU Semester 2/Semester Project/Code/Project/Data/GridInfo.csv", "/home/nick/Desktop/Uni/SDU Semester 2/Semester Project/Code/Project/Data/ProductionUnits.csv");

        List<SourceDataManager.HeatData> heatDataList = new List<SourceDataManager.HeatData>();
        using(SourceDataManager sourceDataManager = new SourceDataManager("/home/nick/Desktop/Uni/SDU Semester 2/Semester Project/Code/Project/Data/summer.csv")) {
            heatDataList = sourceDataManager.GetHeatData();
        }

        Optimiser optimiser = new Optimiser(assetManager.UnitData, heatDataList);
        optimiser.Optimise(5, 1100);
    }
}