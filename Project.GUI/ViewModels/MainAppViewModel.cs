using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI;

namespace Project.GUI.ViewModels
{
    public class MainAppViewModel : ViewModelBase
    {
        private ViewModelBase _contentViewModel;
        public MainAppViewModel() {
            GridUnit = new GridUnitViewModel();
            SourceData = new SourceDataViewModel();
            Optimiser = new OptimiserViewModel();
            
            _contentViewModel = GridUnit;
        }
        public GridUnitViewModel GridUnit { get; }
        public SourceDataViewModel SourceData { get; }
        public OptimiserViewModel Optimiser { get; }

        public ViewModelBase ContentViewModel {
            get => _contentViewModel;
            set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
        }

        public void GridUnitButton() {
            ContentViewModel = GridUnit;
        }

        public void SourceDataButton() {
            ContentViewModel = SourceData;
        }

        public void OptimiserButton() {
            ContentViewModel = Optimiser;
        }

    }
}