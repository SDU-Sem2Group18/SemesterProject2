using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Modules;

namespace Project.GUI.Models
{
    public class GridDataMessage {
        public AssetManager GridInfo { get; }
        public GridDataMessage(AssetManager gridInfo) {
            GridInfo = gridInfo;
        }
    }
    public class UnitDataMessage {
        public List<AssetManager.UnitInformation> Units { get; }
        public UnitDataMessage(List<AssetManager.UnitInformation> units) {
            Units = units;
        }
    }
    public class HeatDataMessage {
        public List<SourceDataManager.HeatData> HeatData { get; }
        public HeatDataMessage(List<SourceDataManager.HeatData> heatData) {
            HeatData = heatData;
        }
    }
    public class ChangesMadeMessage {
        public ChangesMadeMessage() { }
    }
}