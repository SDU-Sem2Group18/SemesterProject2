using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Project.GUI.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    public double MainMenuLeftMargin { get; set; } = 100;

    [ObservableProperty]
    private int _width;

    [ObservableProperty]
    private int _height;
    
    public MainWindowViewModel() {

    }
}
