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
    public class SourceDataViewModel : ViewModelBase
    {
        // Constructor
        public SourceDataViewModel() {
            HeatData = new ObservableCollection<SourceDataManager.HeatData>();
        }

        // Properties related to the source data
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

        // Properties and Methods related to HeatData, as specified in the SourceDataManager
        public ObservableCollection<SourceDataManager.HeatData> HeatData { get; set; }
        public void LoadSourceData() {
            SourcePathTextColour = Brush.Parse("#1d1d1d");
            try {
                Debug.WriteLine(SourcePath);
                using (SourceDataManager sourceDataManager = new SourceDataManager(SourcePath)) {
                    List<SourceDataManager.HeatData> heatData = sourceDataManager.GetHeatData();
                    Debug.WriteLine("SourceDataManager created");
                    if(HeatData.Count != 0) HeatData.Clear();
                    foreach(SourceDataManager.HeatData _ in heatData) HeatData.Add(_);
                }
            } catch (Exception e) {
                Debug.WriteLine(e.Message);
                SourcePath = "Error parsing source data";
                SourcePathTextColour = Brush.Parse("#b5000c");
            }
        }
        
    }
}