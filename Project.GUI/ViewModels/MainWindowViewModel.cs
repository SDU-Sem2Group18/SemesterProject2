using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using ReactiveUI;


namespace Project.GUI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public Interaction<OpenProjectViewModel, ReturnProjectViewModel?> ShowOpenProjectDialog { get; }
    public ICommand OpenProjectCommand { get; }

    private ViewModelBase _contentViewModel;
    public MainWindowViewModel() {
        ShowOpenProjectDialog = new Interaction<OpenProjectViewModel, ReturnProjectViewModel?>();
        OpenProjectCommand = ReactiveCommand.CreateFromTask(async () => {
            var open = new OpenProjectViewModel();
            var result = await ShowOpenProjectDialog.Handle(open);
        });
        MainMenu = new MainMenuViewModel();
        _contentViewModel = MainMenu;
    }

    public MainMenuViewModel MainMenu { get; }

    public ViewModelBase ContentViewModel {
        get => _contentViewModel;
        set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
    }

    

    public void OpenProjectButton() {
        
    }

    public void AboutButton() {
        ContentViewModel = new AboutViewModel();
    }

    public void MainMenuButton() {
        ContentViewModel = MainMenu;
    }

    public void QuitButton() {
        System.Environment.Exit(1);
    }
}
