using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScottPlot;

namespace Project.Modules   
{
    public class Visualization
    {
        private string Name;
        
        private string ImagePath;

        public string GetName(){
            
            
            
            return Name;
        }

        public string GetImagePath(){
            
            
            return ImagePath;
        }

        public void Display(){



        }

        public Visualization(string name, string imagePath){
            Name = name;
            ImagePath = imagePath; 

        }


    }

    public class DataVisualizationManager: IDisposable
    {
        // Constructor
        public DataVisualizationManager() {}

        public void CreatePlotVisualization(List<double> xData, List<double> yData, string filePath, string title, string xName, string yName)
        {
            var plt = new ScottPlot.Plot();

            var scatter = plt.Add.Scatter(xData.ToArray(), yData.ToArray());
            plt.Title(title);
            plt.XLabel(xName);
            plt.YLabel(yName);
            plt.Axes.DateTimeTicksBottom();

            // Minimum Data Calculation
            double min = yData[0];
            foreach(double _ in yData) if(_ < min) min = _;
            var minLine = plt.Add.HorizontalLine(min);
            minLine.Color = ScottPlot.Color.FromHex("4fb051");
            minLine.LinePattern = ScottPlot.LinePattern.Dashed;
            minLine.LabelStyle.Text = "Minimum";
            minLine.LabelStyle.OffsetX = 1100f;
            minLine.LabelStyle.OffsetY = -7.5f;
            //minLine.Axes.YAxis = plt.Axes.Right;
            

            // Maximum Data Calculation
            double max = yData[0];
            foreach(double _ in yData) if(_ > max) max = _;
            var maxLine = plt.Add.HorizontalLine(max);
            maxLine.Color = ScottPlot.Color.FromHex("bf1123");
            maxLine.LinePattern = ScottPlot.LinePattern.Dashed;
            maxLine.LabelStyle.Text = "Maximum";
            maxLine.LabelStyle.OffsetX = 1100f;
            maxLine.LabelStyle.OffsetY = 7.5f;
            //maxLine.Axes.YAxis = plt.Axes.Right;
            

            // Median Calculation
            double median = 0;
            foreach(double _ in yData) median += _;
            median /= yData.Count();
            var medianLine = plt.Add.HorizontalLine(median);
            medianLine.Color = ScottPlot.Color.FromHex("deb03c");
            medianLine.LinePattern = ScottPlot.LinePattern.Dashed;
            medianLine.LabelStyle.Text = "Median";
            medianLine.LabelStyle.OffsetX = 1100f;
            //medianLine.Axes.YAxis = plt.Axes.Right;

            //plt.ShowLegend(Alignment.UpperRight);
            plt.SavePng(filePath,1200, 600);

            // Create a new Visualization with the path to the new plot image
            var visualization = new Visualization("Visualization", filePath);
            visualization.Display();
        }

        // Assuming that you have a method to get X and Y data from SourceDataManager, it might look like this:
        public void CreateVisualisationFromSourceData(SourceDataManager sourceDataManager)
        {
            var heatDataList = sourceDataManager.GetHeatData();

            // Extracting the x and y data from heatDataList
            List<double> xData = heatDataList.Select(data => data.TimeFrom.ToOADate()).ToList();
            List<double> yData = heatDataList.Select(data => (double)data.HeatDemand).ToList();
            string title = "Heat Demand";
            CreatePlotVisualization(xData, yData, @"Data/HeatDemand.png", title, "Time", "Heat Demand (MWh)");

            yData = heatDataList.Select(data => (double)data.ElectricityPrice).ToList();
            title = "Electricity Price";
            CreatePlotVisualization(xData, yData, @"Data/ElectricityPrice.png", title, "Time", "Electricity Price (Kr)");
        }

        public void CreateVisualisationFromResultData(ResultDataManager resultDataManager)
        {
            var resultDataList = resultDataManager.GetResultData();

    // Extracting the x and y data from resultDataList
            List<string> unitName = resultDataList.Select(data => data.UnitName).ToList();
            List<double> producedHeatData = resultDataList.Select(data => (double)data.ProducedHeat).ToList();
            List<double> producedElectricityData = resultDataList.Select(data => (double)data.ProducedElectricity).ToList();
            List<double> consumedElectricityData = resultDataList.Select(data => (double)data.ConsumedElectricity).ToList();
            List<double> energyConsumedData = resultDataList.Select(data => (double)data.EnergyConsumed).ToList();
            List<double> co2EmissionsData = resultDataList.Select(data => (double)data.CO2Emissions).ToList();
            List<double> productionCostData = resultDataList.Select(data => (double)data.ProductionCost).ToList();
            List<double> profitData = resultDataList.Select(data => (double)data.Profit).ToList();

    
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected bool Disposed {get; private set; }
        protected virtual void Dispose(bool disposing) {
            Disposed = true;
        }
    }
}