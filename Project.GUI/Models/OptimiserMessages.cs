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
}