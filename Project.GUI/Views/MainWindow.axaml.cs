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

    private async Task DoShowDialogAsync(InteractionContext<OpenProjectViewModel, ReturnProjectViewModel?> interaction) {
        var dialog = new OpenProjectWindow();
        dialog.DataContext = interaction.Input;

        var result = await dialog.ShowDialog<ReturnProjectViewModel?>(this);
        interaction.SetOutput(result);
    }
}