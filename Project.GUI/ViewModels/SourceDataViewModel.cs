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
using Avalonia.Threading;
using Project.GUI.Models;

namespace Project.GUI.ViewModels
{
    public class SourceDataViewModel : ViewModelBase
    {
        // Constructor
        public SourceDataViewModel() {
            HeatData = new ObservableCollection<SourceDataManager.HeatData>();
            MessageBus.Current.Listen<RefreshPlotsMessage>().Subscribe(HandleRefreshPlotsMessage);
        }

        private void HandleRefreshPlotsMessage(RefreshPlotsMessage message) {
            if(sourceDataVisible) {
                LoadSourceData();
            }
        }

        // Properties related to the source data
        private bool sourceDataVisible = false;
        public bool SourceDataVisible {
            get => sourceDataVisible;
            set => this.RaiseAndSetIfChanged(ref sourceDataVisible, value);
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

                    List<double> xData = HeatData.Select(data => data.TimeFrom.ToOADate()).ToList();
                    List<double> yData = HeatData.Select(data => (double)data.HeatDemand).ToList();
                    string title = "Heat Demand";
                    Debug.WriteLine("Sending Command");
                    MessageBus.Current.SendMessage(new HeatDataLoadedMessage("HeatPlot", (xData, yData, title, "Date & Time", "Heat Demand (MWh)")));

                    yData = HeatData.Select(data => (double)data.ElectricityPrice).ToList();
                    title = "Electricity Price";
                    Debug.WriteLine("Sending Command");
                    MessageBus.Current.SendMessage(new HeatDataLoadedMessage("ElectricityPlot", (xData, yData, title, "Date & Time", "Electricity Price (Kr)")));
                    SourceDataVisible = true;
                    MessageBus.Current.SendMessage(new HeatDataAvailableMessage(HeatData.ToList()));
                }
            } catch (Exception e) {
                Debug.WriteLine(e.Message);
                SourcePath = "Error parsing source data";
                SourcePathTextColour = Brush.Parse("#b5000c");
            }
        }
        
    }
}