using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MassRename.Services;

namespace MassRename.ViewModels;

public partial class BulkRenameViewModel : ObservableObject
{
    /// <summary>
    /// The current index into the rename queue. Never exceeds <see cref="FileCount"/>.
    /// </summary>
    public int CurrentIndex { get; private set; }
    
    /// <summary>
    /// The size of the rename queue.
    /// </summary>
    public int FileCount => _files.Count;
    
    /// <summary>
    /// The current name in the rename queue.
    /// </summary>
    public string CurrentFile => _files[CurrentIndex];

    /// <summary>
    /// The name suggestions for <see cref="CurrentFile"/>.
    /// </summary>
    public string[] NameSuggestions { get; private set; }

    private readonly IReadOnlyList<string> _files;
    private readonly INavigationService _navigationService;
    
    public BulkRenameViewModel(IReadOnlyList<string> files, INavigationService navigationService)
    {
        _files = files;
        _navigationService = navigationService;
        SkipByCommand.Execute(0);
    }

    /// <summary>
    /// Applies the specified name to the current file
    /// </summary>
    /// <param name="name">The stem to give the file</param>
    [RelayCommand]
    private async Task Apply(string name)
    {
        // TODO: Rename the file to the selected name
        SkipByCommand.Execute(1);
    }

    /// <summary>
    /// Skips the specified amount of entries
    /// </summary>
    /// <param name="entries">The amount of entries to skip</param>
    [RelayCommand]
    private void SkipBy(int entries)
    {
        entries = Math.Max(entries, 0);

        if (CurrentIndex + entries >= FileCount)
        {
            _navigationService.SwitchTo(null);
            return;
        }

        CurrentIndex += entries;
        NameSuggestions = ComputeSuggestions(CurrentFile);

        OnPropertyChanged(nameof(CurrentIndex));
        OnPropertyChanged(nameof(CurrentFile));
        OnPropertyChanged(nameof(NameSuggestions));
    }

    /// <summary>
    /// Computes name suggestions for the specified file name
    /// </summary>
    /// <param name="name">The file stem</param>
    /// <returns>An array of name suggestions</returns>
    private string[] ComputeSuggestions(string name)
    {
        return [name, "jordan"];
    }
}