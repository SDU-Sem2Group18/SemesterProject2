using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using Project.GUI.Models;

namespace Project.GUI.ViewModels
{
    public class OptimiserViewModel : ViewModelBase
    {
        public OptimiserViewModel() {
            MessageBus.Current.Listen<GridAndUnitDataAvailableMessage>().Subscribe(HandleGridAndUnitDataAvailable);
            MessageBus.Current.Listen<HeatDataAvailableMessage>().Subscribe(HandleHeatDataAvailable);
        }

        private bool gridAdnUnitDataLoaded = false;
        private bool heatDataLoaded = false;

        private void HandleGridAndUnitDataAvailable(GridAndUnitDataAvailableMessage message) { 
            gridAdnUnitDataLoaded = true;
            SourceDataAvailable();
        }

        private void HandleHeatDataAvailable(HeatDataAvailableMessage message) {
            heatDataLoaded = true;
            SourceDataAvailable();
        }

        private void SourceDataAvailable() {
            if(gridAdnUnitDataLoaded && heatDataLoaded) {
                OptimiserStatusText = "Data not yet optimised.";
                OptimiserButtonActive = true;
            }
        }

        private string optimiserStatusText = "Please make sure to load Grid and Unit, as well as Source Data first.";
        public string OptimiserStatusText {
            get => optimiserStatusText;
            set => this.RaiseAndSetIfChanged(ref optimiserStatusText, value);
        }

        private bool optimiserButtonActive = false;
        public bool OptimiserButtonActive {
            get => optimiserButtonActive;
            set => this.RaiseAndSetIfChanged(ref optimiserButtonActive, value);
        }
    }
}