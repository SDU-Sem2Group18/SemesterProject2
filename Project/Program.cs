using Project.Modules;

namespace Project;

public class Program {
    public static void Main() {
        Console.WriteLine("Hello World!");
    
        using(SourceDataManager sourceDataManager = new SourceDataManager(@"Data/summer.csv"))
        using(DataVisualizationManager dataVisualizationManager = new DataVisualizationManager()) {
            dataVisualizationManager.CreateVisualisationFromSourceData(sourceDataManager);
        }
    }
}