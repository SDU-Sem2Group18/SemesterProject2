using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Modules
{
    public class AssetManager: IDisposable {

        public struct GridInfo {
            private string Name;
            private string ImagePath;

            public GridInfo(string name, string imagePath) {
                Name = name;
                ImagePath = imagePath;
            }

            public string GetName() {
                return Name;
            }

            public string GetImage() {
                return ImagePath;
            }

        }

        public struct UnitInformation {
            private string Name;
            private string ImagePath;
            private float ProducedHeat;
            private float ProductionCost;
            private float Emissions;

            public UnitInformation(string name, string imagePath, float producedHeat, float productionCost, float emissions) {
                Name = name;
                ImagePath = imagePath;
                ProducedHeat = producedHeat;
                ProductionCost = productionCost;
                Emissions = emissions;
            }

            public string GetName() {
                return Name;
            }
            
            public string GetImage() {
                return ImagePath;
            }

            public (float ProducedHeat, float ProductionCost, float Emissions) GetUnitInfo() {
                return (ProducedHeat, ProductionCost, Emissions);
            }
        }

        public enum UnitNames {
            GasBoiler,
            OilBoiler,
            GasMotor,
            ElectricBoiler
        }

        private GridInfo Grid;
        private Dictionary<UnitNames, UnitInformation> UnitData;
        

        public AssetManager() {
            Grid = new(
                "Heatington",
                "../Assets/Images/Heatington.png"
            );
            UnitData = new() {
                {UnitNames.GasBoiler, new(
                    "Gas Boiler",
                    "../Assets/Images/GasBoiler.png",
                    5.0f,
                    500.0f,
                    215.0f
                )},
                {UnitNames.OilBoiler, new(
                    "Oil Boiler",
                    "../Assets/Images/OilBoiler.png",
                    4.0f,
                    700.0f,
                    265.0f
                )},
                {UnitNames.GasMotor, new(
                    "Gas Motor",
                    "../Assets/Images/GasMotor.png",
                    3.6f,
                    1100.0f,
                    640.0f
                )},
                {UnitNames.ElectricBoiler, new(
                    "Electric Boiler",
                    "../Assets/Images/ElectricBoiler.png",
                    8.0f,
                    50.0f,
                    0.0f
                )}
            };
        }

        public GridInfo GetGridInfo() {
            return Grid;
        }

        public UnitInformation GetProductionUnit(UnitNames name){
            return UnitData[name];
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