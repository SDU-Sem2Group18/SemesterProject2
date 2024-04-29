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

        private AssetManager.GridInfo gridInfo = new AssetManager.GridInfo();
        public AssetManager.GridInfo GridInfo {
            get => gridInfo;
            set => this.RaiseAndSetIfChanged(ref gridInfo, value);
        }
        public void LoadGridData() {
            GridSourcePathTextColour = Brush.Parse("#1d1d1d");
            try {
                Debug.WriteLine(GridSourcePath);
                using (AssetManager assetManager = new AssetManager()) {
                    assetManager.GetGriddataFromFile(GridSourcePath);
                    Debug.WriteLine("AssetManager created");
                    GridInfo = assetManager.Grid;
                }
            } catch (Exception e) {
                Debug.WriteLine(e.Message);
                GridSourcePath = "Error parsing grid data";
                GridSourcePathTextColour = Brush.Parse("#b5000c");
            }
        }

        public ObservableCollection<AssetManager.UnitInformation> UnitData { get; set; }
        public void LoadUnitData() {
            UnitSourcePathTextColour = Brush.Parse("#1d1d1d");
            try {
                Debug.WriteLine(UnitSourcePath);
                using (AssetManager assetManager = new AssetManager()) {
                    List<AssetManager.UnitInformation> unitData = new List<AssetManager.UnitInformation>();
                    assetManager.GetUnitDataFromFile(UnitSourcePath);
                    Debug.WriteLine("SourceDataManager created");
                    foreach(AssetManager.UnitInformation _ in assetManager.UnitData) unitData.Add(_);
                }
            } catch (Exception e) {
                Debug.WriteLine(e.Message);
                UnitSourcePath = "Error parsing source data";
                UnitSourcePathTextColour = Brush.Parse("#b5000c");
            }
        }

    }
}