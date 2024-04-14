using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Modules
{
    public class SourceDataManager
    {
        private Dictionary<string, Dictionary<DateTime, (float HeatDemand, float ElectricityPrice)>> InputData;
        string SourcePath;

        public SourceDataManager() {
            SourcePath = "./Data/source.csv";
            InputData = ParseCSV(SourcePath);
        }

        public Dictionary<DateTime, (float HeatDemand, float ElectricityPrice)> GetInformation(DateTime from, DateTime to, string name) {
            return null;
        }

        private Dictionary<string, Dictionary<DateTime, (float HeatDemand, float ElectricityPrice)>> ParseCSV(string sourcePath) {
            return null;
        }
    }
}