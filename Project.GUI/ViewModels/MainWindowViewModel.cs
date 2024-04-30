﻿using System;
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
    // Properties needed by children view models to load data from files
    public Interaction<OpenProjectViewModel, System.Uri?> ShowOpenProjectDialog { get; }
    public Interaction<OpenProjectViewModel, System.Uri?> ShowOpenSourceDataDialog { get; }
    public Interaction<OpenProjectViewModel, System.Uri?> ShowOpenUnitDataDialog { get; }
    public Interaction<OpenProjectViewModel, System.Uri?> ShowOpenGridDataDialog { get; }
    public ICommand OpenProjectCommand { get; }
    public ICommand OpenSourceDataCommand { get; }
    public ICommand OpenUnitDataCommand { get; }
    public ICommand OpenGridDataCommand { get; }

    // Constructor
    public MainWindowViewModel() {
        // Initialising interactions and commands for children to load data from files
        ShowOpenProjectDialog =    new Interaction<OpenProjectViewModel, Uri?>();
        ShowOpenSourceDataDialog = new Interaction<OpenProjectViewModel, Uri?>();
        ShowOpenUnitDataDialog =   new Interaction<OpenProjectViewModel, Uri?>();
        ShowOpenGridDataDialog =   new Interaction<OpenProjectViewModel, Uri?>();

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

        // Initialising children view models of the next underlying layer
        MainMenu = new MainMenuViewModel();
        MainAppViewModel = new MainAppViewModel();
        About = new AboutViewModel();

        // Setting default view model to MainMenuViewModel
        _contentViewModel = MainMenu;
    }

    // Defining children view models of the next underlying layer
    public MainMenuViewModel MainMenu { get; }
    public AboutViewModel About { get; }
    public MainAppViewModel MainAppViewModel { get; }
    private ViewModelBase _contentViewModel;

    public ViewModelBase ContentViewModel {
        get => _contentViewModel;
        set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
    }

    // Defining button logic for main menu buttons
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
