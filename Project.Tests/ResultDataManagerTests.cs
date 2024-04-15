namespace Project.Tests;

public class ResultDataManagerTests
{
    [Fact]
    public void WriteTest()
    {
        using(ResultDataManger TestManager = new ResultDataManger()) {
            for(float x = 1.0f; x <= 50f; ++x) {
                TestManager.ResultDataList?.Add(new ResultDataManger.ResultData(
                AssetManager.UnitNames.ElectricBoiler,
                x,
                x,
                x,
                x,
                x,
                x,
                x
            ));}

            Assert.True(TestManager.SaveResultData(@"Data/ResultTest.csv", false), "WriteTest1 Fail");
            Assert.True(TestManager.SaveResultData(@"Data/ResultTest.csv", true), "WriteTest2 Fail");
            Assert.False(TestManager.SaveResultData(@"Data/ResultTest.csv", false), "WriteTest3 Fail");
            //File.Delete(@"Data/ResultTest.csv");
        }
    }
}