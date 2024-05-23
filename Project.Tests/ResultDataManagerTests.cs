using Project.Modules;
using ScottPlot.Extensions;
namespace Project.Tests;

public class ResultDataManagerTests
{
    [Fact]
    public void WriteTest()
    {
        List<Optimiser.OptimisedData> PassList = [
            new Optimiser.OptimisedData(
                DateTime.Now,
                DateTime.Now,
                1.0f,
                1.0f,
                "Unit1",
                1.0f,
                "Unit2",
                1.0f,
                1.0f,
                1.0f
            ),
            new Optimiser.OptimisedData(
                DateTime.Now,
                DateTime.Now,
                1.0f,
                1.0f,
                "Unit1",
                1.0f,
                null,
                null,
                1.0f,
                1.0f
            ),
            new Optimiser.OptimisedData(
                DateTime.Now,
                DateTime.Now,
                1.0f,
                1.0f,
                null,
                null,
                null,
                null,
                null,
                null
            ),
        ];

        using(ResultDataManager resultDataManager = new ResultDataManager(PassList, "Data/ResultDataSaveTest.csv")) {
            Assert.True(resultDataManager.SaveResultData(true));
        }
    }
}