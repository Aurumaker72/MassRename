using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MassRename.Services;
using MassRename.ViewModels;
using MassRename.Views.Avalonia.Services;
using MassRename.Views.Avalonia.Views;

namespace MassRename.Views.Avalonia;

public partial class App : Application, INavigationService
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainWindow = new MainWindow();
            desktop.MainWindow = mainWindow;
            SwitchTo(null);
        }

        base.OnFrameworkInitializationCompleted();
    }

    public void SwitchTo(object? viewModel)
    {
        if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;
        
        var mainWindow = (MainWindow)desktop.MainWindow!;

        if (viewModel is null)
        {
            mainWindow.DataContext = new FileCollectionSelectionViewModel(FilePickerService.Instance,
                DialogService.Instance, this);

            return;
        }

        mainWindow.DataContext = viewModel;
    }
}