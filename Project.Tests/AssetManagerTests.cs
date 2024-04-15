namespace Project.Tests;

public class AssetManagerTests
{
    [Fact]
    public void AssetManagerTest1()
    {
        using(AssetManager TestManager = new AssetManager()) {
            // Tests relating Grid Struct
            Assert.Equal("Heatington", TestManager.GetGridInfo().GetName());
            Assert.Equal("../Assets/Images/Heatington.png", TestManager.GetGridInfo().GetImage());

            // Tests relating to UnitData Struct
            Assert.Equal(TestManager.GetProductionUnit(AssetManager.UnitNames.GasBoiler), 
            new(
                "Gas Boiler",
                "../Assets/Images/GasBoiler.png",
                5.0f,
                500.0f,
                215.0f
            ));
            Assert.Equal(TestManager.GetProductionUnit(AssetManager.UnitNames.OilBoiler),
            new(
                "Oil Boiler",
                "../Assets/Images/OilBoiler.png",
                4.0f,
                700.0f,
                265.0f
            ));
            Assert.Equal(TestManager.GetProductionUnit(AssetManager.UnitNames.GasMotor),
            new(
                "Gas Motor",
                "../Assets/Images/GasMotor.png",
                3.6f,
                1100.0f,
                640.0f
            ));
            Assert.Equal(TestManager.GetProductionUnit(AssetManager.UnitNames.ElectricBoiler),
            new(
                "Electric Boiler",
                "../Assets/Images/ElectricBoiler.png",
                8.0f,
                50.0f,
                0.0f
            ));
        }
    }
}