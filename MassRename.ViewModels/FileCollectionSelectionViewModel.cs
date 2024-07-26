using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MassRename.Services;
using MassRename.Services.Abstractions;
using MassRename.ViewModels.Extensions;

namespace MassRename.ViewModels;

public partial class FileCollectionSelectionViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<string> _files = [];
    [ObservableProperty] private ObservableCollection<string> _selectedFiles = [];

    private readonly IFilePickerService _filePickerService;
    private readonly IViewDialogService _viewDialogService;
    private readonly INavigationService _navigationService;
    
    public FileCollectionSelectionViewModel(IFilePickerService filePickerService, IViewDialogService viewDialogService,
        INavigationService navigationService)
    {
        _filePickerService = filePickerService;
        _viewDialogService = viewDialogService;
        _navigationService = navigationService;
    }


    /// <summary>
    /// Prompts the user to choose multiple files which are subsequently inserted into the file collection.
    /// </summary>
    [RelayCommand]
    private async Task Browse()
    {
        var files = await _filePickerService.ShowOpenFilePickerAsync(allowMultiple: true);

        if (files is null)
        {
            await _viewDialogService.ShowMessageDialog(MessageDialogType.Error,
                "There was an error while selecting files.\nPlease try again.");
            return;
        }

        Files.AddRange(files);
        SelectedFiles.AddRange(files);
    }

    /// <summary>
    /// Selects all the files from the initial list.
    /// </summary>
    [RelayCommand]
    private void SelectAll()
    {
        SelectedFiles.Clear();
        SelectedFiles.AddRange(Files);
    }

    /// <summary>
    /// Removes all files from the file collection.
    /// </summary>
    [RelayCommand]
    private void ClearAll()
    {
        Files.Clear();
        SelectedFiles.Clear();
    }

    /// <summary>
    /// Removes all selected files from the file collection and unselects them.
    /// </summary>
    [RelayCommand]
    private void ClearSelected()
    {
        foreach (var selectedFile in SelectedFiles.ToArray())
        {
            Files.Remove(selectedFile);
        }

        SelectedFiles.Clear();
    }

    /// <summary>
    /// Continues to the bulk rename page with the currently selected files.
    /// </summary>
    [RelayCommand]
    private void Continue()
    {
        _navigationService.SwitchTo(new BulkRenameViewModel(SelectedFiles, _navigationService));
    }
}