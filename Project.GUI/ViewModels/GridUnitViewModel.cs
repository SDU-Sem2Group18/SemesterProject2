using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media;
using ReactiveUI;
using Project.Modules;
using Project.GUI.Models;
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
        // Constructor
        public GridUnitViewModel() {
            UnitData = new ObservableCollection<AssetManager.UnitInformation>();
            internalGridSourcePath = "";
            internalUnitSourcePath = "";
        }

        // Unit Data Source Path
        public string internalUnitSourcePath { get; set; }
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

        // Grid Data Source Path and Colour
        public string internalGridSourcePath { get; set; }
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

        // Observable AssetManager
        private AssetManager assetManager = new AssetManager("", "");
        public AssetManager GridUnitAssetManager {
            get => assetManager;
            set => this.RaiseAndSetIfChanged(ref assetManager, value);
        }

        // Properties and Methods related to GridData, as specified in the AssetManager, as well as content visibility bool
        private bool gridDataHeaderVisible = false;
        public bool GridDataHeaderVisible {
            get => gridDataHeaderVisible;
            set => this.RaiseAndSetIfChanged(ref gridDataHeaderVisible, value);
        }
        private AssetManager.GridInfo gridInfo = new AssetManager.GridInfo();
        public AssetManager.GridInfo GridInfo {
            get => gridInfo;
            set => this.RaiseAndSetIfChanged(ref gridInfo, value);
        }
        public void LoadGridData() {
            Debug.WriteLine("Loading Grid Data");
            GridSourcePathTextColour = Brush.Parse("#1d1d1d");
            try {
                GridUnitAssetManager = new AssetManager(internalGridSourcePath, "");
                Debug.WriteLine("AssetManager created");
                GridInfo = GridUnitAssetManager.Grid;
                GridDataHeaderVisible = true;
                GridSourcePath = internalGridSourcePath;
                MessageBus.Current.SendMessage(new GridDataMessage(GridUnitAssetManager));
                SenddataLoadedMessage();
            } catch (Exception e) {
                Debug.WriteLine(e.Message);
                GridSourcePath = "Error parsing grid data";
                GridSourcePathTextColour = Brush.Parse("#b5000c");
            }
        }

        // Properties and Methods related to UnitData, as specified in the AssetManager, as well as content visibility bool
        private bool unitDataHeaderVisible = false;
        public bool UnitDataHeaderVisible {
            get => unitDataHeaderVisible;
            set => this.RaiseAndSetIfChanged(ref unitDataHeaderVisible, value);
        }
        public ObservableCollection<AssetManager.UnitInformation> UnitData { get; set; }
        public void LoadUnitData() {
            UnitSourcePathTextColour = Brush.Parse("#1d1d1d");
            try {
                Debug.WriteLine(UnitSourcePath);
                if(GridSourcePath != "Use the load button to open grid data") GridUnitAssetManager = new AssetManager(internalGridSourcePath, internalUnitSourcePath);
                else GridUnitAssetManager = new AssetManager("", internalUnitSourcePath);
                List<AssetManager.UnitInformation> unitData = new List<AssetManager.UnitInformation>();
                
                Debug.WriteLine("SourceDataManager created");
                foreach(AssetManager.UnitInformation _ in GridUnitAssetManager.UnitData) {
                    _.SetSelfPath(internalUnitSourcePath);
                    unitData.Add(_);
                }
                foreach(AssetManager.UnitInformation _ in unitData) UnitData.Add(_);
                UnitDataHeaderVisible = true;
                UnitSourcePath = internalUnitSourcePath;
                MessageBus.Current.SendMessage(new UnitDataMessage(UnitData.ToList()));
                SenddataLoadedMessage();
                
            } catch (Exception e) {
                Debug.WriteLine(e.Message);
                UnitSourcePath = "Error parsing source data";
                UnitSourcePathTextColour = Brush.Parse("#b5000c");
            }
        }

        private void SenddataLoadedMessage() {
            if(GridDataHeaderVisible && UnitDataHeaderVisible) MessageBus.Current.SendMessage(new GridAndUnitDataAvailableMessage(UnitData.ToList()));
        }

    }
}