using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using ReactiveUI;


namespace Project.GUI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public Interaction<OpenProjectViewModel, System.Uri?> ShowOpenProjectDialog { get; }
    public Interaction<OpenProjectViewModel, System.Uri?> ShowOpenSourceDataDialog { get; }
    public Interaction<OpenProjectViewModel, System.Uri?> ShowOpenUnitDataDialog { get; }
    public Interaction<OpenProjectViewModel, System.Uri?> ShowOpenGridDataDialog { get; }
    public ICommand OpenProjectCommand { get; }
    public ICommand OpenSourceDataCommand { get; }
    public ICommand OpenUnitDataCommand { get; }
    public ICommand OpenGridDataCommand { get; }

    private ViewModelBase _contentViewModel;
    public MainWindowViewModel() {
        ShowOpenProjectDialog = new Interaction<OpenProjectViewModel, System.Uri?>();
        ShowOpenSourceDataDialog = new Interaction<OpenProjectViewModel, System.Uri?>();
        ShowOpenUnitDataDialog = new Interaction<OpenProjectViewModel, System.Uri?>();
        ShowOpenGridDataDialog = new Interaction<OpenProjectViewModel, System.Uri?>();

        OpenProjectCommand = ReactiveCommand.CreateFromTask(async () => {
            var open = new OpenProjectViewModel();
            var result = await ShowOpenProjectDialog.Handle(open);
        });
        OpenSourceDataCommand = ReactiveCommand.CreateFromTask(async () => {
            var open = new OpenProjectViewModel();
            var result = await ShowOpenSourceDataDialog.Handle(open);
        });
        OpenUnitDataCommand = ReactiveCommand.CreateFromTask(async () => {
            var open = new OpenProjectViewModel();
            var result = await ShowOpenUnitDataDialog.Handle(open);
        });
        OpenGridDataCommand = ReactiveCommand.CreateFromTask(async () => {
            var open = new OpenProjectViewModel();
            var result = await ShowOpenGridDataDialog.Handle(open);
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
        MainAppViewModel.Reset();
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
