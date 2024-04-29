using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media;
using ReactiveUI;
using Project.Modules;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Avalonia.Interactivity;
using Avalonia;

namespace Project.GUI.ViewModels
{
    public class GridUnitViewModel : ViewModelBase
    {
        public GridUnitViewModel() {
            
        }

        // Unit Data Source Path
        private string unitSourcePath = "Use the load button to open unit data";
        public string UnitSourcePath {
            get => unitSourcePath;
            set => this.RaiseAndSetIfChanged(ref unitSourcePath, value);
        }
        private IBrush unitSourcePathTextColour = Brush.Parse("#1d1d1d");
        public IBrush UnitSourcePathTextColour {
            get => unitSourcePathTextColour;
            set => this.RaiseAndSetIfChanged(ref unitSourcePathTextColour, value);
        }

        // Grid Data Source Path
        private string gridSourcePath = "Use the load button to open grid data";
        public string GridSourcePath {
            get => gridSourcePath;
            set => this.RaiseAndSetIfChanged(ref gridSourcePath, value);
        }
        private IBrush gridSourcePathTextColour = Brush.Parse("#1d1d1d");
        public IBrush GridSourcePathTextColour {
            get => gridSourcePathTextColour;
            set => this.RaiseAndSetIfChanged(ref gridSourcePathTextColour, value);
        }
        
    }
}