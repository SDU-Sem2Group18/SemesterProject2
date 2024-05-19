using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using Project.GUI.Models;
using System.Runtime.CompilerServices;
using Project.Modules;
using System.Collections.ObjectModel;
using DynamicData;

namespace Project.GUI.ViewModels
{
    public class OptimiserViewModel : ViewModelBase
    {
        public OptimiserViewModel() {
            CostOptimisedData = new ObservableCollection<Optimiser.OptimisedData>();
            EmissionOptimisedData = new ObservableCollection<Optimiser.OptimisedData>();

            MessageBus.Current.Listen<GridAndUnitDataAvailableMessage>().Subscribe(HandleGridAndUnitDataAvailable);
            MessageBus.Current.Listen<HeatDataAvailableMessage>().Subscribe(HandleHeatDataAvailable);
            MessageBus.Current.Listen<RefreshOptimiserPlotsMessage>().Subscribe(HandleRefreshOptimiserPlotsMessage);
        }
        private List<AssetManager.UnitInformation> units = new List<AssetManager.UnitInformation>();
        private List<SourceDataManager.HeatData> heatData = new List<SourceDataManager.HeatData>();

        public ObservableCollection<Optimiser.OptimisedData> CostOptimisedData { get; set; }
        public ObservableCollection<Optimiser.OptimisedData> EmissionOptimisedData { get; set; }

        private bool gridAdnUnitDataLoaded = false;
        private bool heatDataLoaded = false;

        private void HandleGridAndUnitDataAvailable(GridAndUnitDataAvailableMessage message) { 
            gridAdnUnitDataLoaded = true;
            units.Clear();
            units.AddRange(message.Units);
            SourceDataAvailable();
        }

        private void HandleHeatDataAvailable(HeatDataAvailableMessage message) {
            heatDataLoaded = true;
            heatData.Clear();
            heatData.AddRange(message.HeatData);
            SourceDataAvailable();
        }

        private void HandleRefreshOptimiserPlotsMessage(RefreshOptimiserPlotsMessage message) {
            if(OptimisedDataVisible) Optimise();
        }
        private void SourceDataAvailable() {
            if(gridAdnUnitDataLoaded && heatDataLoaded) {
                Optimise();
            }
        }

        private string optimiserStatusText = "Please make sure to load Grid and Unit, as well as Source Data first.";
        public string OptimiserStatusText {
            get => optimiserStatusText;
            set => this.RaiseAndSetIfChanged(ref optimiserStatusText, value);
        }

        private bool optimisedDataVisible = false;
        public bool OptimisedDataVisible {
            get => optimisedDataVisible;
            set => this.RaiseAndSetIfChanged(ref optimisedDataVisible, value);
        }

        public void Optimise() {
            OptimiserStatusText = "Optimising...";
            
            List<Optimiser.OptimisedData> costOptimisedDataList = new List<Optimiser.OptimisedData>();
            List<Optimiser.OptimisedData> emissionOptimisedDataList = new List<Optimiser.OptimisedData>();

            Optimiser optimiser = new Optimiser(units, heatData);
            (costOptimisedDataList, emissionOptimisedDataList) = optimiser.Optimise(heatData);

            CostOptimisedData.Clear();
            CostOptimisedData.AddRange(costOptimisedDataList);
            List<double> CostxData = costOptimisedDataList.Select(data => data.TimeFrom.ToOADate()).ToList();
            List<double> CostyCostData = costOptimisedDataList.Select(data => (double)data.Cost!).ToList();
            List<double> CostyEmissionData = costOptimisedDataList.Select(data => (double)data.Emissions!).ToList();
            MessageBus.Current.SendMessage(new HeatDataOptimisedMessage("CostPlot", (CostxData, CostyCostData, CostyEmissionData, "Date & Time", "Cost/Emissions")));

            EmissionOptimisedData.Clear();
            EmissionOptimisedData.AddRange(emissionOptimisedDataList);
            List<double> EmissionxData = emissionOptimisedDataList.Select(data => data.TimeFrom.ToOADate()).ToList();
            List<double> EmissionyCostData = emissionOptimisedDataList.Select(data => (double)data.Cost!).ToList();
            List<double> EmissionyEmissionData = emissionOptimisedDataList.Select(data => (double)data.Emissions!).ToList();
            MessageBus.Current.SendMessage(new HeatDataOptimisedMessage("EmissionPlot", (EmissionxData, EmissionyCostData, EmissionyEmissionData, "Date & Time", "Cost/Emissions")));

            OptimiserStatusText = "Successfully Optimised Data!";
            OptimisedDataVisible = true;
        }

    }
}