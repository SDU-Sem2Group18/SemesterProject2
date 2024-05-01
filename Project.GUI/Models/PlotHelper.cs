using System;
using System.Diagnostics;
using System.Reflection;
using Avalonia.Platform;
using ScottPlot.Avalonia;
using SkiaSharp;

namespace Project.GUI.Models;

// https://github.com/ScottPlot/ScottPlot/issues/3722
public static class PlotHelper {
    private static readonly FieldInfo? LabelCachedTypeface = typeof(ScottPlot.Label).GetField("CachedTypeface", BindingFlags.NonPublic | BindingFlags.Instance);
    private static readonly SKTypeface? NotoSemiBold = CreateTypeface("NotoSans-SemiBold.ttf");
    private static readonly SKTypeface? NotoRegular = CreateTypeface("NotoSans-Regular.ttf");

    public static void SetPlotTypeface(AvaPlot control) {
        if (LabelCachedTypeface is null || NotoSemiBold is null) {
            Debug.WriteLine("Failed to set plot typeface");
            return;
        }

        LabelCachedTypeface.SetValue(control.Plot.Axes.Title.Label, NotoSemiBold);
        foreach (var axis in control.Plot.Axes.GetAxes()) {
            LabelCachedTypeface.SetValue(axis.TickLabelStyle, NotoRegular);
            LabelCachedTypeface.SetValue(axis.Label, NotoSemiBold);
        }
        foreach (var legendItem in control.Plot.Legend.GetItems()) {
            LabelCachedTypeface.SetValue(legendItem.LabelStyle, NotoRegular);
        }
    }

    private static SKTypeface CreateTypeface(string name) {
        using (var asset = AssetLoader.Open(new Uri($"avares://Project.GUI/Assets/Fonts/{name}"))) {
            return SKTypeface.FromStream(asset);
        }
    }
}