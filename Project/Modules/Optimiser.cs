using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace Project.Modules
{
    public class Optimiser
    {
        public Optimiser(List<AssetManager.UnitInformation> unitInformation, List<SourceDataManager.HeatData> heatData) {
            UnitInformation = unitInformation;
            HeatData = heatData;
        }

        [Delimiter(",")]
        [CultureInfo("dk-DK")]
        [InjectionOptions(CsvHelper.Configuration.InjectionOptions.Exception)]
        
        public class OptimisedData {
            [Name("Time from")]
            [Format("dd/MM/yyyy HH:mm")]
            public DateTime TimeFrom { get; set; }

            [Name("Time to")]
            [Format("dd/MM/yyyy HH:mm")]
            public DateTime TimeTo { get; set; }

            [Name("Heat Demand")]
            public float HeatDemand { get; set; }

            [Name("Electricity Price")]
            public float ElectricityPrice { get; set; }

            [Name("Unit 1")]
            [NullValues("-")]
            public string? Unit1Name { get; set; }

            [Name("U1 Production")]
            [NullValues("-")]
            public float? Unit1Production { get; set; }
            
            [Name("Unit 2")]
            [NullValues("-")]
            public string? Unit2Name { get; set; }

            [Name("U2 Production")]
            [NullValues("-")]
            public float? Unit2Production { get; set; }

            [Name("Cost")]
            [NullValues("-")]
            public float? Cost { get; set; }

            [Name("Emissions")]
            [NullValues("-")]
            public float? Emissions { get; set; }

            public OptimisedData(DateTime timeFrom, DateTime timeTo, float heatDemand, float electricity, string? unit1Name, float? unit1Production, string? unit2Name, float? unit2Production, float? cost, float? emissions ) {
                TimeFrom = timeFrom;
                TimeTo = timeTo;
                HeatDemand = heatDemand;
                ElectricityPrice = electricity;
                Unit1Name = unit1Name;
                Unit1Production = unit1Production;
                Unit2Name = unit2Name;
                Unit2Production = unit2Production;
                Cost = cost;
                Emissions = emissions;
            }
        }
        
        public enum OptimisationArgument {
            CO2,
            Cost
        }
        
        private List<AssetManager.UnitInformation> UnitInformation;
        private List<SourceDataManager.HeatData> HeatData;
        

        public (List<OptimisedData> costOptimisedData, List<OptimisedData> emissionOptimisedData) Optimise(List<SourceDataManager.HeatData> heatData) {
            // Return Lists
            List<OptimisedData> costOptimisedData = new List<OptimisedData>();
            List<OptimisedData> emissionOptimisedData = new List<OptimisedData>();
            
            foreach(SourceDataManager.HeatData _ in heatData) {
                // 1. Create two lists containing each production unit, one for cost and one for emission
                List<AssetManager.UnitInformation> costUnitList = new List<AssetManager.UnitInformation>();
                costUnitList.AddRange(UnitInformation);
                List<AssetManager.UnitInformation> emissionUnitList = new List<AssetManager.UnitInformation>();
                emissionUnitList.AddRange(UnitInformation);

                (AssetManager.UnitInformation? producingUnit, AssetManager.UnitInformation? consumingUnit, double? producingUnitShare, double? consumingUnitShare, double? cost, double? emission) bestCostCombination = (null, null, null, null, null, null);
                (AssetManager.UnitInformation? producingUnit, AssetManager.UnitInformation? consumingUnit, double? producingUnitShare, double? consumingUnitShare, double? cost, double? emission) bestEmissionCombination = (null, null, null, null, null, null);

                // 1.1 We start by finding the most efficient combination of electricity producing and consuming units, if they exist
                (bestCostCombination, bestEmissionCombination) = OptimiseElectricityProducingAndConsumingUnits(_.HeatDemand, _.ElectricityPrice);

                // 2. Delete the found combinations from each list
                // ACTUALLY DON'T DO THIS - The producing/consuming units might still form a better combination with other units.
                
                // - CODE REMOVED - //

                // 3. Get the results of the remaining units/combinations, including a possible producing/consuming unit if no combination exists
                var bestTotalCostCombination = FindBestCostCombination(_.HeatDemand, _.ElectricityPrice, costUnitList, bestCostCombination);
                var bestTotalEmissionCombination = FindBestEmissionCombination(_.HeatDemand, _.ElectricityPrice, emissionUnitList, bestEmissionCombination);

                // Logic to add to cost List
                if(bestTotalCostCombination.cost == null) costOptimisedData.Add(new(
                    _.TimeFrom,
                    _.TimeTo,
                    _.HeatDemand,
                    _.ElectricityPrice,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                )); else if(bestTotalCostCombination.unit2.unit == null) costOptimisedData.Add(new(
                    _.TimeFrom,
                    _.TimeTo,
                    _.HeatDemand,
                    _.ElectricityPrice,
                    bestTotalCostCombination.unit1.unit!.Name,
                    (float)bestTotalCostCombination.unit1.producedHeat!,
                    null,
                    null,
                    (float)bestTotalCostCombination.cost,
                    (float)bestTotalCostCombination.emission!
                )); else costOptimisedData.Add(new(
                    _.TimeFrom,
                    _.TimeTo,
                    _.HeatDemand,
                    _.ElectricityPrice,
                    bestTotalCostCombination.unit1.unit!.Name,
                    (float)bestTotalCostCombination.unit1.producedHeat!,
                    bestTotalCostCombination.unit2.unit!.Name,
                    (float)bestTotalCostCombination.unit2.producedHeat!,
                    (float)bestTotalCostCombination.cost,
                    (float)bestTotalCostCombination.emission!
                ));

                if(bestTotalEmissionCombination.emission == null) emissionOptimisedData.Add(new(
                    _.TimeFrom,
                    _.TimeTo,
                    _.HeatDemand,
                    _.ElectricityPrice,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                )); else if(bestTotalEmissionCombination.unit2.unit == null) emissionOptimisedData.Add(new(
                    _.TimeFrom,
                    _.TimeTo,
                    _.HeatDemand,
                    _.ElectricityPrice,
                    bestTotalEmissionCombination.unit1.unit!.Name,
                    (float)bestTotalEmissionCombination.unit1.producedHeat!,
                    null,
                    null,
                    (float)bestTotalEmissionCombination.cost!,
                    (float)bestTotalEmissionCombination.emission!
                )); else emissionOptimisedData.Add(new(
                    _.TimeFrom,
                    _.TimeTo,
                    _.HeatDemand,
                    _.ElectricityPrice,
                    bestTotalEmissionCombination.unit1.unit!.Name,
                    (float)bestTotalEmissionCombination.unit1.producedHeat!,
                    bestTotalEmissionCombination.unit2.unit!.Name,
                    (float)bestTotalEmissionCombination.unit2.producedHeat!,
                    (float)bestTotalEmissionCombination.cost!,
                    (float)bestTotalEmissionCombination.emission!
                ));
                
                //Debug.WriteLine($"HeatDemand: {_.HeatDemand},\tElectricityPrice: {_.ElectricityPrice}");
                //if(bestTotalEmissionCombination.cost == null) Debug.WriteLine("!!! NO UNIT/COMBINATION CAN PRODUCE ENOUGH HEAT FOR THIS TIME. MANUAL INTERVENTION REQUIRED !!!\n");
                //else if(bestTotalEmissionCombination.unit2.unit == null) Debug.WriteLine($"Best Unit: {bestTotalEmissionCombination.unit1.unit!.Name}\nCost: {bestTotalEmissionCombination.cost}\nEmissions: {bestTotalEmissionCombination.emission}\n");
                //else Debug.WriteLine($"Best Combination: {bestTotalEmissionCombination.unit1.unit!.Name} ({bestTotalEmissionCombination.unit1.producedHeat}) + {bestTotalEmissionCombination.unit2.unit.Name} ({bestTotalEmissionCombination.unit2.producedHeat})\nCost: {bestTotalEmissionCombination.cost}\nEmissions: {bestTotalEmissionCombination.emission}\n");

            }
            return (costOptimisedData, emissionOptimisedData);
        } 

        private ((AssetManager.UnitInformation? producingUnit, AssetManager.UnitInformation? consumingUnit, double? producingUnitShare, double? consumingUnitShare, double? cost, double? emission), (AssetManager.UnitInformation? producingUnit, AssetManager.UnitInformation? consumingUnit, double? producingUnitShare, double? consumingUnitShare, double? cost, double? emission)) OptimiseElectricityProducingAndConsumingUnits(float heatDemand, float electricityPrice) {
            (AssetManager.UnitInformation? producingUnit, AssetManager.UnitInformation? consumingUnit, double? producingUnitShare, double? consumingUnitShare, double? cost, double? emission) bestCostCombination = (null, null, null, null, null, null);
            (AssetManager.UnitInformation? producingUnit, AssetManager.UnitInformation? consumingUnit, double? producingUnitShare, double? consumingUnitShare, double? cost, double? emission) bestEmissionCombination = (null, null, null, null, null, null);
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

                        //Debug.WriteLine($"Production Unit: {producingUnit.Name}: {productionUnitShare}");
                        //Debug.WriteLine($"Consuming  Unit: {consumingUnit.Name}: {consumingUnitShare}");

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
                        double costOptimisedEmissions;

                        void DetermineCostShare(double limit) {
                                if(heatDemand <= limit) {
                                productionUnitCostShare = (double)(heatDemand*productionUnitShare);
                                consumingUnitCostShare = (double)(heatDemand*consumingUnitShare);
                                costOptimisedEmissions = (double)((producingUnit.Emissions == null ? 0 : producingUnit.Emissions)*productionUnitCostShare + (consumingUnit.Emissions == null ? 0 : consumingUnit.Emissions)*consumingUnitCostShare);
                            } else {
                                productionUnitCostShare = (double)producingUnit.MaxHeat;
                                consumingUnitCostShare = (double)(limit*consumingUnitShare + (heatDemand - limit));
                                costOptimisedEmissions = (double)(producingUnit.MaxHeat*(producingUnit.Emissions == null ? 0 : producingUnit.Emissions) + (heatDemand - limit)*(consumingUnit.Emissions == null ? 0 : consumingUnit.Emissions));
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
                        if(bestCostCombination.cost == null || optimisedPrice < bestCostCombination.cost) bestCostCombination = (producingUnit, consumingUnit, productionUnitCostShare, consumingUnitCostShare, optimisedPrice, costOptimisedEmissions);

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
                        double emissionOptimisedCost;

                        void DetermineEmissionShare(double limit) {
                                if(heatDemand <= limit) {
                                productionUnitEmissionShare = (double)(heatDemand*productionUnitShare);
                                consumingUnitEmissionShare = (double)(heatDemand*consumingUnitShare);
                                emissionOptimisedCost = (double)(producingUnit.ProductionCost * productionUnitEmissionShare + (consumingUnit.ProductionCost + electricityPrice) * consumingUnitEmissionShare);
                            } else {
                                productionUnitEmissionShare = (double)producingUnit.MaxHeat;
                                consumingUnitEmissionShare = (double)(limit*consumingUnitShare + (heatDemand - limit));
                                emissionOptimisedCost = (double)(producingUnit.MaxHeat * producingUnit.ProductionCost + (heatDemand - limit)*(consumingUnit.ProductionCost + electricityPrice));
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
                        if(bestEmissionCombination.emission == null || optimisedEmissions < bestEmissionCombination.emission) bestEmissionCombination = (producingUnit, consumingUnit, productionUnitEmissionShare, consumingUnitEmissionShare, optimisedEmissions, emissionOptimisedCost);
                        //Debug.WriteLine($"Heat Demand: {heatDemand}\nPrice: {optimisedPrice}\nCost Share: {productionUnitCostShare} + {consumingUnitCostShare}\nEmissions: {optimisedEmissions}\nEmissions Share: {productionUnitEmissionShare} + {consumingUnitEmissionShare}\n");
                    }
                }
                return (bestCostCombination, bestEmissionCombination);

            } else return ((null, null, null, null, null, null), (null, null, null, null, null, null));
            //Debug.WriteLine($"Best Cost Combination: {bestCostCombination.producingUnit?.Name} + {bestCostCombination.consumingUnit?.Name}");
            //Debug.WriteLine($"Best Emission Combination: {bestEmissionCombination.producingUnit?.Name} + {bestEmissionCombination.consumingUnit?.Name}\n");
        }

        private ((AssetManager.UnitInformation? unit, double? producedHeat) unit1, (AssetManager.UnitInformation? unit, double? producedHeat) unit2, double? cost, double? emission) FindBestCostCombination(float heatDemand, float electricityPrice, List<AssetManager.UnitInformation> units, (AssetManager.UnitInformation? producingUnit, AssetManager.UnitInformation? consumingUnit, double? producingUnitShare, double? consumingUnitShare, double? cost, double? emission) bestCostCombination) {
            
            ((AssetManager.UnitInformation? unit, double? producedHeat) unit1, (AssetManager.UnitInformation? unit, double? producedHeat) unit2, double? cost, double? emission) returnData = ((null, null), (null, null), null, null);

            // First, Check all units to see if any single one is sufficient
            foreach(AssetManager.UnitInformation unit in units) {
                if(unit.MaxHeat < heatDemand) continue;
                else {
                    if(unit.MaxElectricity < 0) {
                    // Electricity Consuming Unit
                        if(returnData.cost == null || (heatDemand * electricityPrice + unit.ProductionCost * heatDemand) < returnData.cost) returnData = ((unit, heatDemand), (null, null), (heatDemand * electricityPrice + unit.ProductionCost * heatDemand), (double)((unit.Emissions == null) ? 0 : heatDemand * unit.Emissions));
                        else continue;
                    } else {
                    // All other units
                        if(returnData.cost == null || (heatDemand * electricityPrice + unit.ProductionCost * heatDemand) < returnData.cost) returnData = ((unit, heatDemand), (null, null), (heatDemand * unit.ProductionCost), (double)((unit.Emissions == null) ? 0 : unit.Emissions));
                        else continue;
                    }
                }
            }
            if(returnData.cost != null && returnData.cost <= bestCostCombination.cost) return returnData;

            // Second, Check all pairs of units, in both orders, for we are only checking max. Production capacity of one unit + remaining capacity of other unit
            // This way, two units yield two combinations as opposed to one.
            foreach(AssetManager.UnitInformation outerUnit in units) {
                foreach(AssetManager.UnitInformation innerUnit in units) {
                    if(outerUnit == innerUnit || outerUnit.MaxHeat + innerUnit.MaxHeat < heatDemand) continue;
                    if(returnData.cost == null) {
                        if(outerUnit.MaxElectricity != null && outerUnit.MaxElectricity < 0) {
                            if(heatDemand > outerUnit.MaxHeat && heatDemand > innerUnit.MaxHeat) {
                                returnData = ((outerUnit, outerUnit.MaxHeat), (innerUnit, heatDemand - outerUnit.MaxHeat), outerUnit.MaxHeat * (electricityPrice + outerUnit.ProductionCost) + ((heatDemand - outerUnit.MaxHeat) * innerUnit.ProductionCost), outerUnit.MaxHeat * ((outerUnit.Emissions == null) ? 0 : outerUnit.Emissions) + (heatDemand - outerUnit.MaxHeat) * ((innerUnit.Emissions == null) ? 0 : innerUnit.Emissions));
                            } else continue;
                        }
                        else if(innerUnit.MaxElectricity != null && innerUnit.MaxElectricity < 0) {
                            if(heatDemand > outerUnit.MaxHeat && heatDemand > innerUnit.MaxHeat) {
                                returnData = ((outerUnit, outerUnit.MaxHeat), (innerUnit, heatDemand - outerUnit.MaxHeat), outerUnit.MaxHeat * outerUnit.ProductionCost + ((heatDemand - outerUnit.MaxHeat) * (electricityPrice + innerUnit.ProductionCost)), outerUnit.MaxHeat * ((outerUnit.Emissions == null) ? 0 : outerUnit.Emissions) + (heatDemand - outerUnit.MaxHeat) * ((innerUnit.Emissions == null) ? 0 : innerUnit.Emissions));
                            } else continue;
                        } else {
                            if(heatDemand > outerUnit.MaxHeat && heatDemand > innerUnit.MaxHeat) {
                                returnData = ((outerUnit, outerUnit.MaxHeat), (innerUnit, heatDemand - outerUnit.MaxHeat), outerUnit.MaxHeat * outerUnit.ProductionCost + ((heatDemand - outerUnit.MaxHeat) * innerUnit.ProductionCost), outerUnit.MaxHeat * ((outerUnit.Emissions == null) ? 0 : outerUnit.Emissions) + (heatDemand - outerUnit.MaxHeat) * ((innerUnit.Emissions == null) ? 0 : innerUnit.Emissions));
                            } else continue;
                        }
                    } else {
                        if(outerUnit.MaxElectricity != null && outerUnit.MaxElectricity < 0) {
                            if(heatDemand > outerUnit.MaxHeat && heatDemand > innerUnit.MaxHeat && (outerUnit.MaxHeat * (electricityPrice + outerUnit.ProductionCost) + ((heatDemand - outerUnit.MaxHeat) * innerUnit.ProductionCost)) < returnData.cost) {
                                returnData = ((outerUnit, outerUnit.MaxHeat), (innerUnit, heatDemand - outerUnit.MaxHeat), outerUnit.MaxHeat * (electricityPrice + outerUnit.ProductionCost) + ((heatDemand - outerUnit.MaxHeat) * innerUnit.ProductionCost), outerUnit.MaxHeat * ((outerUnit.Emissions == null) ? 0 : outerUnit.Emissions) + (heatDemand - outerUnit.MaxHeat) * ((innerUnit.Emissions == null) ? 0 : innerUnit.Emissions));
                            } else continue;
                        }
                        else if(innerUnit.MaxElectricity != null && innerUnit.MaxElectricity < 0) {
                            if(heatDemand > outerUnit.MaxHeat && heatDemand > innerUnit.MaxHeat && (outerUnit.MaxHeat * outerUnit.ProductionCost + ((heatDemand - outerUnit.MaxHeat) * (electricityPrice + innerUnit.ProductionCost))) < returnData.cost) {
                                returnData = ((outerUnit, outerUnit.MaxHeat), (innerUnit, heatDemand - outerUnit.MaxHeat), outerUnit.MaxHeat * outerUnit.ProductionCost + ((heatDemand - outerUnit.MaxHeat) * (electricityPrice + innerUnit.ProductionCost)), outerUnit.MaxHeat * ((outerUnit.Emissions == null) ? 0 : outerUnit.Emissions) + (heatDemand - outerUnit.MaxHeat) * ((innerUnit.Emissions == null) ? 0 : innerUnit.Emissions));
                            } else continue;
                        } else {
                            if(heatDemand > outerUnit.MaxHeat && heatDemand > innerUnit.MaxHeat && (outerUnit.MaxHeat * outerUnit.ProductionCost + ((heatDemand - outerUnit.MaxHeat) * innerUnit.ProductionCost)) < returnData.cost) {
                                returnData = ((outerUnit, outerUnit.MaxHeat), (innerUnit, heatDemand - outerUnit.MaxHeat), outerUnit.MaxHeat * outerUnit.ProductionCost + ((heatDemand - outerUnit.MaxHeat) * innerUnit.ProductionCost), outerUnit.MaxHeat * ((outerUnit.Emissions == null) ? 0 : outerUnit.Emissions) + (heatDemand - outerUnit.MaxHeat) * ((innerUnit.Emissions == null) ? 0 : innerUnit.Emissions));
                            } else continue;
                        }
                    }
                }
            }

            // Lastly, Check if the best combination here is more cost efficient than the combo of producing/consuming unit
            if(returnData.cost != null && bestCostCombination.cost != null && returnData.cost < bestCostCombination.cost) return returnData;
            else if(bestCostCombination.cost != null && (returnData.cost == null || bestCostCombination.cost <= returnData.cost)) return ((bestCostCombination.producingUnit, bestCostCombination.producingUnitShare), (bestCostCombination.consumingUnit, bestCostCombination.consumingUnitShare), bestCostCombination.cost, bestCostCombination.emission);
            
            return ((null, null), (null, null), null, null);
        }

        private ((AssetManager.UnitInformation? unit, double? producedHeat) unit1, (AssetManager.UnitInformation? unit, double? producedHeat) unit2, double? cost, double? emission) FindBestEmissionCombination(float heatDemand, float electricityPrice, List<AssetManager.UnitInformation> units, (AssetManager.UnitInformation? producingUnit, AssetManager.UnitInformation? consumingUnit, double? producingUnitShare, double? consumingUnitShare, double? cost, double? emission) bestCostCombination) {
            ((AssetManager.UnitInformation? unit, double? producedHeat) unit1, (AssetManager.UnitInformation? unit, double? producedHeat) unit2, double? cost, double? emission) returnData = ((null, null), (null, null), null, null);
            
            // First, Check all units to see if any single one is sufficient
            foreach(AssetManager.UnitInformation unit in units) {
                if(unit.MaxHeat < heatDemand) continue;
                else {
                    if(unit.MaxElectricity < 0) {
                    // Electricity Consuming Unit
                        if(returnData.emission == null || ((unit.Emissions == null ? 0 : unit.Emissions) * heatDemand) < returnData.emission) returnData = ((unit, heatDemand), (null, null), (heatDemand * electricityPrice + unit.ProductionCost * heatDemand), (double)(unit.Emissions == null ? 0 : unit.Emissions) * heatDemand);
                        else continue;
                    } else {
                    // All other units
                        if(returnData.emission == null || ((unit.Emissions == null ? 0 : unit.Emissions) * heatDemand) < returnData.cost) returnData = ((unit, heatDemand), (null, null), (heatDemand * unit.ProductionCost), (double)(unit.Emissions == null ? 0 : unit.Emissions) * heatDemand);
                        else continue;
                    }
                }
            }
            if(returnData.cost != null && returnData.cost <= bestCostCombination.cost) return returnData;
            
            // Second, Check all pairs of units, in both orders, for we are only checking max. Production capacity of one unit + remaining capacity of other unit
            // This way, two units yield two combinations as opposed to one.
            foreach(AssetManager.UnitInformation outerUnit in units) {
                foreach(AssetManager.UnitInformation innerUnit in units) {
                    if(outerUnit == innerUnit || outerUnit.MaxHeat + innerUnit.MaxHeat < heatDemand) continue;
                    if(returnData.emission == null) {
                        if(outerUnit.MaxElectricity != null && outerUnit.MaxElectricity < 0) {
                            if(heatDemand > outerUnit.MaxHeat && heatDemand > innerUnit.MaxHeat) {
                                returnData = ((outerUnit, outerUnit.MaxHeat), (innerUnit, heatDemand - outerUnit.MaxHeat), outerUnit.MaxHeat * (electricityPrice + outerUnit.ProductionCost) + ((heatDemand - outerUnit.MaxHeat) * innerUnit.ProductionCost), outerUnit.MaxHeat * ((outerUnit.Emissions == null) ? 0 : outerUnit.Emissions) + (heatDemand - outerUnit.MaxHeat) * ((innerUnit.Emissions == null) ? 0 : innerUnit.Emissions));
                            } else continue;
                        }
                        else if(innerUnit.MaxElectricity != null && innerUnit.MaxElectricity < 0) {
                            if(heatDemand > outerUnit.MaxHeat && heatDemand > innerUnit.MaxHeat) {
                                returnData = ((outerUnit, outerUnit.MaxHeat), (innerUnit, heatDemand - outerUnit.MaxHeat), outerUnit.MaxHeat * outerUnit.ProductionCost + ((heatDemand - outerUnit.MaxHeat) * (electricityPrice + innerUnit.ProductionCost)), outerUnit.MaxHeat * ((outerUnit.Emissions == null) ? 0 : outerUnit.Emissions) + (heatDemand - outerUnit.MaxHeat) * ((innerUnit.Emissions == null) ? 0 : innerUnit.Emissions));
                            } else continue;
                        } else {
                            if(heatDemand > outerUnit.MaxHeat && heatDemand > innerUnit.MaxHeat) {
                                returnData = ((outerUnit, outerUnit.MaxHeat), (innerUnit, heatDemand - outerUnit.MaxHeat), outerUnit.MaxHeat * outerUnit.ProductionCost + ((heatDemand - outerUnit.MaxHeat) * innerUnit.ProductionCost), outerUnit.MaxHeat * ((outerUnit.Emissions == null) ? 0 : outerUnit.Emissions) + (heatDemand - outerUnit.MaxHeat) * ((innerUnit.Emissions == null) ? 0 : innerUnit.Emissions));
                            } else continue;
                        }
                    } else {
                        if(outerUnit.MaxElectricity != null && outerUnit.MaxElectricity < 0) {
                            if(heatDemand > outerUnit.MaxHeat && heatDemand > innerUnit.MaxHeat && (outerUnit.MaxHeat * (outerUnit.Emissions == null ? 0 : outerUnit.Emissions) + ((heatDemand - outerUnit.MaxHeat) * (innerUnit.Emissions == null ? 0 : innerUnit.Emissions))) < (returnData.emission == null ? 0 : returnData.emission)) {
                                returnData = ((outerUnit, outerUnit.MaxHeat), (innerUnit, heatDemand - outerUnit.MaxHeat), outerUnit.MaxHeat * (electricityPrice + outerUnit.ProductionCost) + ((heatDemand - outerUnit.MaxHeat) * innerUnit.ProductionCost), outerUnit.MaxHeat * ((outerUnit.Emissions == null) ? 0 : outerUnit.Emissions) + (heatDemand - outerUnit.MaxHeat) * ((innerUnit.Emissions == null) ? 0 : innerUnit.Emissions));
                            } else continue;
                        }
                        else if(innerUnit.MaxElectricity != null && innerUnit.MaxElectricity < 0) {
                            if(heatDemand > outerUnit.MaxHeat && heatDemand > innerUnit.MaxHeat && (outerUnit.MaxHeat * (outerUnit.Emissions == null ? 0 : outerUnit.Emissions) + ((heatDemand - outerUnit.MaxHeat) * (innerUnit.Emissions == null ? 0 : innerUnit.Emissions))) < (returnData.emission == null ? 0 : returnData.emission)) {
                                returnData = ((outerUnit, outerUnit.MaxHeat), (innerUnit, heatDemand - outerUnit.MaxHeat), outerUnit.MaxHeat * outerUnit.ProductionCost + ((heatDemand - outerUnit.MaxHeat) * (electricityPrice + innerUnit.ProductionCost)), outerUnit.MaxHeat * ((outerUnit.Emissions == null) ? 0 : outerUnit.Emissions) + (heatDemand - outerUnit.MaxHeat) * ((innerUnit.Emissions == null) ? 0 : innerUnit.Emissions));
                            } else continue;
                        } else {
                            if(heatDemand > outerUnit.MaxHeat && heatDemand > innerUnit.MaxHeat && (outerUnit.MaxHeat * (outerUnit.Emissions == null ? 0 : outerUnit.Emissions) + ((heatDemand - outerUnit.MaxHeat) * (innerUnit.Emissions == null ? 0 : innerUnit.Emissions))) < (returnData.emission == null ? 0 : returnData.emission)) {
                                returnData = ((outerUnit, outerUnit.MaxHeat), (innerUnit, heatDemand - outerUnit.MaxHeat), outerUnit.MaxHeat * outerUnit.ProductionCost + ((heatDemand - outerUnit.MaxHeat) * innerUnit.ProductionCost), outerUnit.MaxHeat * ((outerUnit.Emissions == null) ? 0 : outerUnit.Emissions) + (heatDemand - outerUnit.MaxHeat) * ((innerUnit.Emissions == null) ? 0 : innerUnit.Emissions));
                            } else continue;
                        }
                    }
                }
            }

            // Lastly, Check if the best combination here is more cost efficient than the combo of producing/consuming unit
            if(returnData.emission != null && bestCostCombination.emission != null && returnData.emission < bestCostCombination.emission) return returnData;
            else if(bestCostCombination.emission != null && (returnData.emission == null || bestCostCombination.emission <= returnData.emission)) return ((bestCostCombination.producingUnit, bestCostCombination.producingUnitShare), (bestCostCombination.consumingUnit, bestCostCombination.consumingUnitShare), bestCostCombination.cost, bestCostCombination.emission);
            
            return ((null, null), (null, null), null, null);
        }
    }
}

/*
    ..................................................
    :                    ......                      :
    :                 .:||||||||:.                   :
    :                /            \                  :
    :               (   o      o   )                 :
    :-------@@@@----------:  :----------@@@@---------:
    :                     `--'                       :
    :                                                :
    :                                                :
    :         K I L R O Y   S A Y S   H I !          :
    :................................................:
*/

// P.s. Ironically, this class is the least optimised in terms of code readability :')