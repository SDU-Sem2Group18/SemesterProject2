using System.Reactive;
using Avalonia.Controls;
using ReactiveUI;

namespace Project.GUI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase _contentViewModel;
    public MainWindowViewModel() {
        MainMenu = new MainMenuViewModel();
        _contentViewModel = MainMenu;
    }

    public MainMenuViewModel MainMenu { get; }

    public ViewModelBase ContentViewModel {
        get => _contentViewModel;
        set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
    }
    public void AboutView() {
        ContentViewModel = new AboutViewModel();
    }

    public void MainMenuView() {
        ContentViewModel = MainMenu;
    }
}
