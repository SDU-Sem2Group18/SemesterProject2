using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using ReactiveUI;


namespace Project.GUI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public Interaction<OpenProjectViewModel, System.Uri?> ShowOpenProjectDialog { get; }
    public Interaction<OpenProjectViewModel, System.Uri?> ShowOpenSourceDataDialog { get; }
    public Interaction<OpenProjectViewModel, System.Uri?> ShowOpenAssetDataDialog { get; }
    public ICommand OpenProjectCommand { get; }
    public ICommand OpenSourceDataCommand { get; }
    public ICommand OpenAssetDataCommand { get; }

    private ViewModelBase _contentViewModel;
    public MainWindowViewModel() {
        ShowOpenProjectDialog = new Interaction<OpenProjectViewModel, System.Uri?>();
        ShowOpenSourceDataDialog = new Interaction<OpenProjectViewModel, System.Uri?>();
        ShowOpenAssetDataDialog = new Interaction<OpenProjectViewModel, System.Uri?>();

        OpenProjectCommand = ReactiveCommand.CreateFromTask(async () => {
            var open = new OpenProjectViewModel();
            var result = await ShowOpenProjectDialog.Handle(open);
        });
        OpenSourceDataCommand = ReactiveCommand.CreateFromTask(async () => {
            var open = new OpenProjectViewModel();
            var result = await ShowOpenSourceDataDialog.Handle(open);
        });
        OpenAssetDataCommand = ReactiveCommand.CreateFromTask(async () => {
            var open = new OpenProjectViewModel();
            var result = await ShowOpenAssetDataDialog.Handle(open);
        });

        MainMenu = new MainMenuViewModel();
        MainAppViewModel = new MainAppViewModel();
        About = new AboutViewModel();
        _contentViewModel = MainMenu;
    }

    public MainMenuViewModel MainMenu { get; }
    public AboutViewModel About { get; }
    public MainAppViewModel MainAppViewModel { get; }

    public ViewModelBase ContentViewModel {
        get => _contentViewModel;
        set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
    }

    public void NewProjectButton() {
        ContentViewModel = MainAppViewModel;
    }

    public void AboutButton() {
        ContentViewModel = About;
    }

    public void MainMenuButton() {
        ContentViewModel = MainMenu;
    }

    public void QuitButton() {
        System.Environment.Exit(1);
    }
}
