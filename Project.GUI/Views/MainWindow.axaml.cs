using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using Project.GUI.ViewModels;
using ReactiveUI;

namespace Project.GUI.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated(action => action(ViewModel!.ShowOpenProjectDialog.RegisterHandler(DoShowDialogAsync)));
    }

    private async void DoShowDialogAsync(InteractionContext<OpenProjectViewModel, System.Uri?> interaction) {
        //var dialog = new OpenProjectWindow();
        //dialog.DataContext = interaction.Input;
        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions() {
            Title = "Open Project",
            AllowMultiple = false,
        });

        if(files.Count >= 1) {
            Debug.WriteLine(files[0].Name);
        }

        //var result = await dialog.ShowDialog<ReturnProjectViewModel?>(this);
        interaction.SetOutput(files[0].Path);
    }
}