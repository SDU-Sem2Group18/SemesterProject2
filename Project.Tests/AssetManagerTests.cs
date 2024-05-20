namespace Project.Tests;

public class AssetManagerTests
{
    [Fact]
    public void AssetManagerTest1()
    {
        AssetManager TestManager = new AssetManager(@"Data/TestGrid.csv" ,@"Data/TestUnits.csv");
        
        // Tests relating Grid Loading
        Assert.Equal("Heatington", TestManager.Grid.Name);
        Assert.Equal("/Assets/Images/Heatington.png", TestManager.Grid.ImagePath);

        // Tests relating UnitInfo
        for(int _ = 0; _ <= 9; ++_) {
            Assert.Equal(($"{_+1}", $"{_+1}", _+1, _+1, _+1, _+1, _+1), (TestManager.UnitData[_].Name, TestManager.UnitData[_].ImagePath, TestManager.UnitData[_].MaxHeat, TestManager.UnitData[_].ProductionCost, TestManager.UnitData[_].MaxElectricity, TestManager.UnitData[_].GasConsumption, TestManager.UnitData[_].Emissions));
        }

        Assert.Null(TestManager.UnitData[10].MaxElectricity);
        Assert.Null(TestManager.UnitData[11].GasConsumption);
        Assert.Null(TestManager.UnitData[12].Emissions);
        
        // Tests relating to reading from csv file
        try{
            AssetManager TestManager2 = new AssetManager(@"Data/TestGrid.csv", @"Data/ErroneousTestUnits.csv");
        }
        catch(Exception ex) {
            Assert.Equal("Error handling csv file. Ensure your file is formatted correctly. Details: \nThe field 'name' in the first line of the CSV file is empty.", ex.Message);
        }
    }
}