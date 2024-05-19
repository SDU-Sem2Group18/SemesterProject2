using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Modules;

namespace Project.GUI.Models
{
    public class GridAndUnitDataAvailableMessage {
        public List<AssetManager.UnitInformation> Units { get; } 
        public GridAndUnitDataAvailableMessage(List<AssetManager.UnitInformation> units) {
            Units = units;
        }
    }

    public class HeatDataAvailableMessage {
        public List<SourceDataManager.HeatData> HeatData { get; }
        public HeatDataAvailableMessage(List<SourceDataManager.HeatData> heatData) {
            HeatData = heatData;
        }
    }

    public class HeatDataOptimisedMessage {
        public string Name { get; }
        public List<double> xData { get; }
        public List<double> yCostData { get; }
        public List<double> yEmissionData { get; }
        public string xName { get; }
        public string yName { get; }

        public HeatDataOptimisedMessage(string name, (List<double> xData, List<double> ycostData, List<double> yemissionData, string xName, string yName) args) {
            Name = name;
            xData = args.xData;
            yCostData = args.ycostData;
            yEmissionData = args.yemissionData;
            xName = args.xName;
            yName = args.yName;
        }
    }

    public class RefreshOptimiserPlotsMessage {
        public RefreshOptimiserPlotsMessage() { }
    }
}