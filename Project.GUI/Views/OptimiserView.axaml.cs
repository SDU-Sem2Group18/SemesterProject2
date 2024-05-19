using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Project.GUI.Models;
using ReactiveUI;
using ScottPlot;
using ScottPlot.Avalonia;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.GUI.Views;

public partial class OptimiserView : UserControl
{
    public OptimiserView()
    {
        InitializeComponent();
        MessageBus.Current.Listen<HeatDataOptimisedMessage>().Subscribe(HandleHeatDataOptimisedMessage);
    }

    private void HandleHeatDataOptimisedMessage(HeatDataOptimisedMessage message)
    {
        Dispatcher.UIThread.InvokeAsync(() => CreatePlotVisualization(message.Name, (message.xData, message.yCostData, message.yEmissionData, message.xName, message.yName)));
    }

    public void CreatePlotVisualization(string name, (List<double> xData, List<double> yCostData, List<double> yEmissionData, string xName, string yName) args) {
        AvaPlot? avaPlot = this.FindControl<AvaPlot>(name);
        
        if(avaPlot == null) return;
        avaPlot.Reset();
        
        var costScatter = avaPlot.Plot.Add.Scatter(args.xData.ToArray(), args.yCostData.ToArray());
        var emissionScatter = avaPlot.Plot.Add.Scatter(args.xData.ToArray(), args.yEmissionData.ToArray());

        costScatter.Color = ScottPlot.Color.FromHex("b5000c");
        emissionScatter.Color = ScottPlot.Color.FromHex("4fb051");
        avaPlot.Plot.Axes.Left.Label.Text = args.yName;

        avaPlot.Plot.Axes.DateTimeTicksBottom();
        avaPlot.Plot.Axes.Bottom.Label.Text = args.xName;

        LegendItem costGraphItem = new() {
            LineColor = Color.FromHex("b5000c"),
            MarkerFillColor = Color.FromHex("b5000c"),
            MarkerLineColor = Color.FromHex("b5000c"),
            LineWidth = 2,
            LabelText = "Cost"
        };
        LegendItem emissionGraphItem = new() {
            LineColor = Color.FromHex("4fb051"),
            MarkerFillColor = Color.FromHex("4fb051"),
            MarkerLineColor = Color.FromHex("4fb051"),
            LineWidth = 2,
            LabelText = "Emissions"
        };

        LegendItem[] items = { costGraphItem, emissionGraphItem};
        avaPlot.Plot.ShowLegend(items);
        avaPlot.Plot.Legend.Alignment = Alignment.UpperLeft;
        avaPlot.Plot.Legend.FontSize = 12;
        PlotHelper.SetPlotTypeface(avaPlot);
        avaPlot.Interaction.Disable();
        avaPlot.Refresh();
    }
}