using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media;
using Project.GUI.Models;
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

        private IBrush gridUnitButtonColour = Brush.Parse("#800008");
        public IBrush GridUnitButtonColour {
            get => gridUnitButtonColour;
            set => this.RaiseAndSetIfChanged(ref gridUnitButtonColour, value);
        }
        private IBrush sourceDataButtonColour = Brush.Parse("#b5000c");
        public IBrush SourceDataButtonColour {
            get => sourceDataButtonColour;
            set => this.RaiseAndSetIfChanged(ref sourceDataButtonColour, value);
        }
        private IBrush optimiserButtonColour = Brush.Parse("#b5000c");
        public IBrush OptimiserButtonColour {
            get => optimiserButtonColour;
            set => this.RaiseAndSetIfChanged(ref optimiserButtonColour, value);
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
            GridUnitButtonColour = Brush.Parse("#800008");
            SourceDataButtonColour = Brush.Parse("#b5000c");
            OptimiserButtonColour = Brush.Parse("#b5000c");
        }

        public void SourceDataButton() {
            ContentViewModel = SourceData;
            GridUnitButtonColour = Brush.Parse("#b5000c");
            SourceDataButtonColour = Brush.Parse("#800008");
            OptimiserButtonColour = Brush.Parse("#b5000c");
        }

        public void OptimiserButton() {
            ContentViewModel = Optimiser;
            GridUnitButtonColour = Brush.Parse("#b5000c");
            SourceDataButtonColour = Brush.Parse("#b5000c");
            OptimiserButtonColour = Brush.Parse("#800008");
        }

    }
}