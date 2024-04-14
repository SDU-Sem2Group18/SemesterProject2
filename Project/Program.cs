using Project.Modules;

namespace Project;

public class Program {
    public static void Main() {
        Modules.SourceDataManager sourceDataManager = new("Data/summer.csv");
    }
}