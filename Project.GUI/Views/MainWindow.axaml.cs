using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Project.GUI.ViewModels;
using ReactiveUI;

namespace Project.GUI.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated(action => action(ViewModel!.ShowOpenProjectDialog.RegisterHandler(DoShowProjectDialogAsync)));
        this.WhenActivated(action => action(ViewModel!.ShowOpenSourceDataDialog.RegisterHandler(DoShowSourceDataDialogAsync)));
        this.WhenActivated(action => action(ViewModel!.ShowOpenUnitDataDialog.RegisterHandler(DoShowUnitDataDialogAsync)));
        this.WhenActivated(action => action(ViewModel!.ShowOpenGridDataDialog.RegisterHandler(DoShowGridDataDialogAsync)));
    }

    

    public static FilePickerFileType ProjectFile { get; } = new("Heat Optimisation Project") {Patterns = new[] { "*.hop" } };
    public static FilePickerFileType SourceDataFile { get; } = new("Heat/Electricity Source Data") {Patterns = new[] { "*.csv" } };
    public static FilePickerFileType AssetDataFile { get; } = new("Grid/Unit Source Data") {Patterns = new[] { "*.csv" } };

    private async void DoShowProjectDialogAsync(InteractionContext<OpenProjectViewModel, System.Uri?> interaction) {
        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions() {
            Title = "Open Project",
            FileTypeFilter = [ProjectFile],
            AllowMultiple = false,
        });

        if(files.Count >= 1) interaction.SetOutput(files[0].Path);
        else interaction.SetOutput(null);
    }

    private async void DoShowSourceDataDialogAsync(InteractionContext<OpenProjectViewModel, System.Uri?> interaction) {
        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions() {
            Title = "Open Source Data",
            FileTypeFilter = [SourceDataFile],
            AllowMultiple = false,
        });

        if(files.Count >= 1) {
            interaction.SetOutput(files[0].Path);
            if(files[0].TryGetLocalPath() == null) ViewModel!.MainAppViewModel.SourceData.SourcePath = files[0].Path.AbsolutePath.Replace("%20", " ");
            else ViewModel!.MainAppViewModel.SourceData.SourcePath = files[0].TryGetLocalPath()!.Replace("%20", " ");
            ViewModel!.MainAppViewModel.SourceData.LoadSourceData();
        }
        else interaction.SetOutput(null);
    }

    private async void DoShowUnitDataDialogAsync(InteractionContext<OpenProjectViewModel, System.Uri?> interaction) {
        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions() {
            Title = "Open Grid/Unit Data",
            FileTypeFilter = [AssetDataFile],
            AllowMultiple = false,
        });

        if(files.Count >= 1) {
            interaction.SetOutput(files[0].Path);
            if(files[0].TryGetLocalPath() == null) ViewModel!.MainAppViewModel.SourceData.SourcePath = files[0].Path.AbsolutePath.Replace("%20", " ");
            else ViewModel!.MainAppViewModel.GridUnit.UnitSourcePath = files[0].TryGetLocalPath()!.Replace("%20", " ");
            //ViewModel!.MainAppViewModel.SourceData.LoadSourceData();
        }
    }

    private async void DoShowGridDataDialogAsync(InteractionContext<OpenProjectViewModel, System.Uri?> interaction) {
        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions() {
            Title = "Open Grid/Unit Data",
            FileTypeFilter = [AssetDataFile],
            AllowMultiple = false,
        });

        if(files.Count >= 1) {
            interaction.SetOutput(files[0].Path);
            if(files[0].TryGetLocalPath() == null) ViewModel!.MainAppViewModel.SourceData.SourcePath = files[0].Path.AbsolutePath.Replace("%20", " ");
            else ViewModel!.MainAppViewModel.GridUnit.GridSourcePath = files[0].TryGetLocalPath()!.Replace("%20", " ");
            //ViewModel!.MainAppViewModel.SourceData.LoadSourceData();
        }
    }
}