using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Project.Modules
{
    public class Optimiser
    {
        public Optimiser(List<AssetManager.UnitInformation> unitInformation, List<SourceDataManager.HeatData> heatData) {
            UnitInformation = unitInformation;
            HeatData = heatData;
        }
        
        public enum OptimisationArgument {
            CO2,
            Cost
        }
        
        public List<AssetManager.UnitInformation> UnitInformation;
        public List<SourceDataManager.HeatData> HeatData;
        public (AssetManager.UnitInformation? producingUnit, AssetManager.UnitInformation? consumingUnit, double? producingUnitShare, double? consumingUnitShare, double? cost) bestCostCombination = (null, null, null, null, null);
        public (AssetManager.UnitInformation? producingUnit, AssetManager.UnitInformation? consumingUnit, double? producingUnitShare, double? consumingUnitShare, double? emission) bestEmissionCombination = (null, null, null, null, null);

        public bool Optimise(float heatDemand, float electricityPrice) {
            OptimiseElectricityProducingAndConsumingUnits(heatDemand, electricityPrice);
            return true;
        }

        private void OptimiseElectricityProducingAndConsumingUnits(float heatDemand, float electricityPrice) {
        // Optimisation Algorithm of Electricity Producing and Consuming Units
            List<AssetManager.UnitInformation> producingUnits = new List<AssetManager.UnitInformation>();
            List<AssetManager.UnitInformation> consumingUnits = new List<AssetManager.UnitInformation>();

            foreach(AssetManager.UnitInformation unit in UnitInformation) {
                if(unit.MaxElectricity != null && unit.MaxElectricity > 0) producingUnits.Add(unit);
                if(unit.MaxElectricity != null && unit.MaxElectricity < 0) consumingUnits.Add(unit);
            }

            // Nested Function to check all possible combinations of producing and consuming units
            if(producingUnits.Count != 0 && consumingUnits.Count != 0) {
                foreach(AssetManager.UnitInformation producingUnit in producingUnits) {
                    foreach(AssetManager.UnitInformation consumingUnit in consumingUnits) {
                        // If the current combination cannot fulfill the heat demand at all, continue
                        if(producingUnit.MaxHeat + consumingUnit.MaxHeat < heatDemand) continue;

                        double? productionUnitEfficiency = producingUnit.MaxElectricity / producingUnit.MaxHeat;
                        double? consumingUnitEfficiency = consumingUnit.MaxHeat / consumingUnit.MaxElectricity * (-1);
                        if(productionUnitEfficiency == null || consumingUnitEfficiency == null) continue;

                        double productionUnitShare = (double)(1/(1 + productionUnitEfficiency * consumingUnitEfficiency));
                        double consumingUnitShare = (double)(productionUnitEfficiency * consumingUnitEfficiency * productionUnitShare);

                        Debug.WriteLine($"Production Unit: {producingUnit.Name}: {productionUnitShare}");
                        Debug.WriteLine($"Consuming  Unit: {consumingUnit.Name}: {consumingUnitShare}");

                        #pragma warning disable CS8629 // Nullable value type may be null.
                        double maxSharedHeat = (double)(producingUnit.MaxHeat + (consumingUnitEfficiency * producingUnit.MaxElectricity));
                        #pragma warning restore CS8629 

                        // Optimise for Cost
                        double sharedHeatCost(double x) {return (double)((productionUnitShare * producingUnit.ProductionCost + consumingUnitShare * consumingUnit.ProductionCost) * x); }
                        double totalHeatCost(double x, double middleBound) {
                            if(x <= middleBound) return sharedHeatCost(x);
                            return sharedHeatCost(middleBound) + (double)((x-middleBound)*(electricityPrice + consumingUnit.ProductionCost) / consumingUnitEfficiency);
                        }

                        double optimisedPrice = totalHeatCost(heatDemand, maxSharedHeat);
                        double productionUnitCostShare;
                        double consumingUnitCostShare;

                        void DetermineCostShare(double limit) {
                                if(heatDemand <= limit) {
                                productionUnitCostShare = (double)(heatDemand*productionUnitShare);
                                consumingUnitCostShare = (double)(heatDemand*consumingUnitShare);
                            } else {
                                productionUnitCostShare = (double)producingUnit.MaxHeat;
                                consumingUnitCostShare = (double)(consumingUnitEfficiency*producingUnit.MaxElectricity! + (heatDemand - limit)*consumingUnit.MaxHeat);
                            }
                        }
                        DetermineCostShare(maxSharedHeat);

                        for(double _ = maxSharedHeat - 0.01f; _ >= 0; _-=0.01f) {
                            if(heatDemand - _ > consumingUnit.MaxHeat) break;
                            if(totalHeatCost(heatDemand, _) < optimisedPrice) {
                                optimisedPrice = totalHeatCost(heatDemand, _);
                                DetermineCostShare(_);
                            }
                        }
                        if(bestCostCombination.cost == null || optimisedPrice < bestCostCombination.cost) bestCostCombination = (producingUnit, consumingUnit, productionUnitCostShare, consumingUnitCostShare, optimisedPrice);

                        // Optimise for Emissions
                        #pragma warning disable CS8629 // Nullable value type may be null.
                        double producingUnitEmissions = (double)producingUnit.Emissions;
                        #pragma warning restore CS8629
                        double consumingUnitEmissions;
                        if(consumingUnit.Emissions == null) consumingUnitEmissions = 0; else consumingUnitEmissions = (double)consumingUnit.Emissions;

                        double sharedEmissionCost(double x) {return (double)((productionUnitShare * producingUnitEmissions + consumingUnitShare * consumingUnitEmissions) * x); }
                        double totalEmissionCost(double x, double middleBound) {
                            if(x <= middleBound) return sharedEmissionCost(x);
                            return sharedHeatCost(middleBound) + (double)((x-middleBound)*(electricityPrice + consumingUnitEmissions) / consumingUnitEfficiency);
                        }

                        double optimisedEmissions = totalEmissionCost(heatDemand, maxSharedHeat);
                        double productionUnitEmissionShare;
                        double consumingUnitEmissionShare;

                        void DetermineEmissionShare(double limit) {
                                if(heatDemand <= limit) {
                                productionUnitEmissionShare = (double)(heatDemand*productionUnitShare);
                                consumingUnitEmissionShare = (double)(heatDemand*consumingUnitShare);
                            } else {
                                productionUnitEmissionShare = (double)producingUnit.MaxHeat;
                                consumingUnitEmissionShare = (double)(consumingUnitEfficiency*producingUnit.MaxElectricity! + (heatDemand - limit)*consumingUnit.MaxHeat);
                            }
                        }
                        DetermineEmissionShare(maxSharedHeat);

                        for(double _ = maxSharedHeat - 0.01f; _ >= 0; _-=0.01f) {
                            if(heatDemand - _ > consumingUnit.MaxHeat) break;
                            if(totalEmissionCost(heatDemand, _) < optimisedEmissions) {
                                optimisedEmissions = totalEmissionCost(heatDemand, _);
                                DetermineEmissionShare(_);
                            }
                        }
                        if(bestEmissionCombination.emission == null || optimisedEmissions < bestEmissionCombination.emission) bestEmissionCombination = (producingUnit, consumingUnit, productionUnitEmissionShare, consumingUnitEmissionShare, optimisedEmissions);


                        Debug.WriteLine($"Heat Demand: {heatDemand}\nPrice: {optimisedPrice}\nCost Share: {productionUnitCostShare} + {consumingUnitCostShare}\nEmissions: {optimisedEmissions}\nEmissions Share: {productionUnitEmissionShare} + {consumingUnitEmissionShare}\n");
                    }
                }
            } else return;
            Debug.WriteLine($"Best Cost Combination: {bestCostCombination.producingUnit?.Name} + {bestCostCombination.consumingUnit?.Name}");
            Debug.WriteLine($"Best Emission Combination: {bestEmissionCombination.producingUnit?.Name} + {bestEmissionCombination.consumingUnit?.Name}");
        }
    }
}