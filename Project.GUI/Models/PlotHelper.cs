using System;
using System.Diagnostics;
using System.Reflection;
using Avalonia.Platform;
using ScottPlot.Avalonia;
using SkiaSharp;

namespace Project.GUI.Models;

// https://github.com/ScottPlot/ScottPlot/issues/3722
// Credits to https://github.com/kebox7, fairly modified by me
// Deprecated as of 04.05.2024, as setting fontface was added to ScottPlot 5.0.34 (we use 5.0.32)
// For the purposes of showcase, this will remain and be used as a reference to its necessity when I added and adapted it for my own needs.
// - Nick
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