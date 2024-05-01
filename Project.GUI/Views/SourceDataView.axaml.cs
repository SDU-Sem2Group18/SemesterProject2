using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ScottPlot;
using ScottPlot.Avalonia;
using Avalonia.Threading;
using ReactiveUI;
using Project.GUI.Models;
using System;
using System.Diagnostics;
using System.Reflection;
using SkiaSharp;
using Avalonia.Platform;

namespace Project.GUI.Views;

public partial class SourceDataView : UserControl
{
    public SourceDataView()
    {
        InitializeComponent();
        MessageBus.Current.Listen<HeatDataLoadedMessage>().Subscribe(HandleMessage);
    }

    private void HandleMessage(HeatDataLoadedMessage message) {
        Debug.WriteLine("Command Received");
        Dispatcher.UIThread.InvokeAsync(() => CreatePlotVisualization(message.Name, (message.xData, message.yData, message.title, message.xName, message.yName)));
    }
    
    public void CreatePlotVisualization(string name, (List<double> xData, List<double> yData, string title, string xName, string yName) args) {
        Debug.WriteLine("PlotVisualisaion Method Called");
        AvaPlot? avaPlot = this.FindControl<AvaPlot>(name);
        
        if(avaPlot == null) return;
        avaPlot.Reset();
        
        var scatter = avaPlot.Plot.Add.Scatter(args.xData.ToArray(), args.yData.ToArray());
        scatter.Color = ScottPlot.Color.FromHex("b5000c");
        avaPlot.Plot.Title(args.title, 25);
        avaPlot.Plot.Axes.Left.Label.Text = args.yName;

        avaPlot.Plot.Axes.DateTimeTicksBottom();
        avaPlot.Plot.Axes.Bottom.Label.Text = args.xName;

        // Minimum Data Calculation
        double min = args.yData.Min();
        var minLine = avaPlot.Plot.Add.HorizontalLine(min);
        minLine.Color = ScottPlot.Color.FromHex("6eb26e");
        minLine.LinePattern = ScottPlot.LinePattern.Dashed;
        LegendItem minLineItem = new() {
            LineColor = Color.FromHex("6eb26e"),
            MarkerFillColor = Color.FromHex("6eb26e"),
            MarkerLineColor = Color.FromHex("6eb26e"),
            LineWidth = 2,
            LabelText = "Min"
        };

        // Maximum Data Calculation
        double max = args.yData.Max();
        var maxLine = avaPlot.Plot.Add.HorizontalLine(max);
        maxLine.Color = ScottPlot.Color.FromHex("006400");
        maxLine.LinePattern = ScottPlot.LinePattern.Dashed;
        LegendItem maxLineItem = new() {
            LineColor = Color.FromHex("006400"),
            MarkerFillColor = Color.FromHex("006400"),
            MarkerLineColor = Color.FromHex("006400"),
            LineWidth = 2,
            LabelText = "Max"
        };

        // Median Calculation
        double median = 0;
        foreach(double _ in args.yData) median += _;
        median /= args.yData.Count();
        var medianLine = avaPlot.Plot.Add.HorizontalLine(median);
        medianLine.Color = ScottPlot.Color.FromHex("00800040");
        medianLine.LinePattern = ScottPlot.LinePattern.Dashed;
        LegendItem medianLineItem = new() {
            LineColor = Color.FromHex("00800040"),
            MarkerFillColor = Color.FromHex("00800040"),
            MarkerLineColor = Color.FromHex("00800040"),
            LineWidth = 2,
            LabelText = "Med"
        };

        LegendItem[] items = { maxLineItem, medianLineItem, minLineItem };
        avaPlot.Plot.ShowLegend(items);
        avaPlot.Plot.Legend.Alignment = Alignment.LowerLeft;
        avaPlot.Plot.Legend.FontSize = 12;
        PlotHelper.SetPlotTypeface(avaPlot);
        avaPlot.Refresh();
    }
}