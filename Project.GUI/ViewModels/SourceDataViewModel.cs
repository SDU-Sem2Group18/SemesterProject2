using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media;
using ReactiveUI;
using Project.Modules;

namespace Project.GUI.ViewModels
{
    public class SourceDataViewModel : ViewModelBase
    {
        public SourceDataViewModel() {
            
        }

        private string sourcePath = "Use the load button to open source data";
        public string SourcePath {
            get => sourcePath;
            set => this.RaiseAndSetIfChanged(ref sourcePath, value);
        }

        private IBrush sourcePathTextColour = Brush.Parse("#1d1d1d");
        public IBrush SourcePathTextColour {
            get => sourcePathTextColour;
            set => this.RaiseAndSetIfChanged(ref sourcePathTextColour, value);
        }

        public void LoadSourceData() {
            try {
                using SourceDataManager sourceDataManager = new SourceDataManager(SourcePath) {

                };
            } catch (Exception e) {

            }
            
        }
    }
}