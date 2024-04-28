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
    public ICommand OpenProjectCommand { get; }

    private ViewModelBase _contentViewModel;
    public MainWindowViewModel() {
        ShowOpenProjectDialog = new Interaction<OpenProjectViewModel, System.Uri?>();
        OpenProjectCommand = ReactiveCommand.CreateFromTask(async () => {
            var open = new OpenProjectViewModel();
            var result = await ShowOpenProjectDialog.Handle(open);
        });
        MainMenu = new MainMenuViewModel();
        About = new AboutViewModel();
        _contentViewModel = MainMenu;
    }

    public MainMenuViewModel MainMenu { get; }
    public AboutViewModel About { get; }

    public ViewModelBase ContentViewModel {
        get => _contentViewModel;
        set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
    }

    public void NewProjectButton() {
        ContentViewModel = new MainAppViewModel();
    }

    public void OpenProjectButton() {
        
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
