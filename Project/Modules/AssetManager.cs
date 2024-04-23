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
            private float MaxHeat;
            private float ProductionCost;
            private float? MaxElectricity { get; set; }
            private float? GasConsumption { get; set; }
            private float? Emissions { get; set; }

            public UnitInformation(string name, string imagePath, float producedHeat, float productionCost) {
                Name = name;
                ImagePath = imagePath;
                MaxHeat = producedHeat;
                ProductionCost = productionCost;
            }

            public string GetName() {
                return Name;
            }
            
            public string GetImage() {
                return ImagePath;
            }

            public (float MaxHeat, float ProductionCost, float? MaxElectricity, float? GasConsumption, float? Emissions) GetUnitInfo() {
                return (MaxHeat, ProductionCost, MaxElectricity, GasConsumption, Emissions);
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
                "/Assets/Images/Heatington.png"
            );
            UnitData = new() {
                {UnitNames.GasBoiler, new(
                    "Gas Boiler",
                    "/Assets/Images/GasBoiler.png",
                    5.0f,
                    500.0f
                )},
                {UnitNames.OilBoiler, new(
                    "Oil Boiler",
                    "/Assets/Images/OilBoiler.png",
                    4.0f,
                    700.0f
                )},
                {UnitNames.GasMotor, new(
                    "Gas Motor",
                    "/Assets/Images/GasMotor.png",
                    3.6f,
                    1100.0f
                )},
                {UnitNames.ElectricBoiler, new(
                    "Electric Boiler",
                    "/Assets/Images/ElectricBoiler.png",
                    8.0f,
                    50.0f
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