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
        // Constructor
        public MainAppViewModel() {
            // Initialise all view models needed in the main app
            GridUnit = new GridUnitViewModel();
            SourceData = new SourceDataViewModel();
            Optimiser = new OptimiserViewModel();

            _contentViewModel = GridUnit;
        }

        // Reset function to reset all view models, called when entering the main app via New Project Button
        public void Reset() {
            GridUnit = new GridUnitViewModel();
            SourceData = new SourceDataViewModel();
            Optimiser = new OptimiserViewModel();

            GridUnitButton();
        }

        // Brushes for the self-made tab view at the top of the sidebar
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

        // Defining view models to be used in the main app
        public GridUnitViewModel GridUnit { get; set;}
        public SourceDataViewModel SourceData { get; set; }
        public OptimiserViewModel Optimiser { get; set; }
        private ViewModelBase _contentViewModel;

        public ViewModelBase ContentViewModel {
            get => _contentViewModel;
            set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
        }

        // Logic for custom tab view, changing the view model and the button's background colours
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