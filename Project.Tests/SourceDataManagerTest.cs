namespace Project.Tests;

public class SourceDataManagerTest
{
    [Fact]
    public void SourceDataManagerTest1()
    {
        List<SourceDataManager.HeatData> SummerData = new List<SourceDataManager.HeatData>();
        using (SourceDataManager sourceDataManager = new(@"Data/TestData.csv")) {
            SummerData = sourceDataManager.GetHeatData();
        }
        int x = 1;
        foreach (var data in SummerData) {
            var expectations = new List<Tuple<object, object>>() {
                new(data.TimeFrom, new DateTime(2024, 1, x, 0, 0, 0, 0)),
                new(data.TimeTo, new DateTime(2024, 1, x, 1, 0, 0, 0)),
                new(data.HeatDemand, (float)x),
                new(data.ElectricityPrice, (float)x)
            };
            Assert.All(expectations, pair => Assert.Equal(pair.Item1, pair.Item2));

            ++x;
        }
    }
}