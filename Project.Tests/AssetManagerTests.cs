namespace Project.Tests;

public class AssetManagerTests
{
    [Fact]
    public void AssetManagerTest1()
    {
        using(AssetManager TestManager = new AssetManager(@"Data/TestUnits.csv")) {
            // Tests relating Grid Struct
            Assert.Equal("Heatington", TestManager.GetGridInfo().GetName());
            Assert.Equal("/Assets/Images/Heatington.png", TestManager.GetGridInfo().GetImage());

            // Tests relating UnitInfo
            for(int _ = 1; _ <= 10; ++_) {
                Assert.Equal(($"{_}", $"{_}", _, _, _, _, _), (TestManager.UnitData[$"{_}"].Name, TestManager.UnitData[$"{_}"].ImagePath, TestManager.UnitData[$"{_}"].MaxHeat, TestManager.UnitData[$"{_}"].ProductionCost, TestManager.UnitData[$"{_}"].MaxElectricity, TestManager.UnitData[$"{_}"].GasConsumption, TestManager.UnitData[$"{_}"].Emissions));
            }

            Assert.Null(TestManager.UnitData["NullTest 1"].MaxElectricity);
            Assert.Null(TestManager.UnitData["NullTest 2"].GasConsumption);
            Assert.Null(TestManager.UnitData["NullTest 3"].Emissions);
        }
        // Tests relating to reading from csv file
        try{
            using(AssetManager TestManager = new AssetManager(@"Data/ErroneousTestUnits.csv")) {}
        }
        catch(Exception ex) {
            Assert.Equal("Error handling csv file. Ensure your file is formatted correctly. Details: \nThe field 'name' in the first line of the CSV file is empty.", ex.Message);
        }
    }
}