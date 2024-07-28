using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
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
    public string CurrentFile => Path.GetFileNameWithoutExtension(_files[CurrentIndex]);

    /// <summary>
    /// The name suggestions for <see cref="CurrentFile"/>.
    /// </summary>
    public string[] NameSuggestions { get; private set; }

    /// <summary>
    /// All artist names which have appeared in the queue or have been explicitly added.
    /// </summary>
    public ObservableCollection<string> KnownArtists { get; } = [ "Unknown Artist" ];

    /// <summary>
    /// The currently selected artist override.
    /// </summary>
    [ObservableProperty] private string _selectedArtist = "Unknown Artist";
    
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
    private void Apply(string name)
    {
        // TODO: Rename the file to the selected name
        SkipByCommand.Execute(1);
    }

    /// <summary>
    /// Skips the specified amount of entries
    /// </summary>
    /// <param name="entries">
    /// The amount of entries to skip.
    /// Skipping beyond the last element will navigate to the default ViewModel,
    /// and skipping to before the first element will be clamped.
    /// </param>
    [RelayCommand]
    private void SkipBy(int entries)
    {
        if (CurrentIndex + entries >= FileCount)
        {
            _navigationService.SwitchTo(null);
            return;
        }

        CurrentIndex = Math.Max(0, CurrentIndex + entries);
        NameSuggestions = ComputeSuggestions(CurrentFile).ToArray();

        OnPropertyChanged(nameof(CurrentIndex));
        OnPropertyChanged(nameof(CurrentFile));
        OnPropertyChanged(nameof(NameSuggestions));
    }

    partial void OnSelectedArtistChanged(string _)
    {
        // Suggestions depend on the selected artist, so we'll recompute those.
        NameSuggestions = ComputeSuggestions(CurrentFile).ToArray();
        OnPropertyChanged(nameof(NameSuggestions));
    }
    
    #region Suggestion generation

    [GeneratedRegex(@"\s{2,}")]
    private static partial Regex WhitespaceCompressorRegex();

    /// <summary>
    /// Computes name suggestions for the specified file name
    /// </summary>
    /// <param name="name">The file stem</param>
    /// <returns>An array of name suggestions</returns>
    private List<string> ComputeSuggestions(string name)
    {
        List<string> suggestions = [];

        if (string.IsNullOrWhiteSpace(name))
        {
            return suggestions;
        }

        AppendSuggestion_DashSeparated(suggestions, name);
        AppendSuggestion_SeparatorlessAndIncomplete(suggestions, name);

        return suggestions;
    }

    private void AppendSuggestion_DashSeparated(List<string> suggestions, string name)
    {
        var computed = WhitespaceCompressorRegex().Replace(name, " ").Trim();
        if (string.IsNullOrWhiteSpace(computed) || computed == "-")
        {
            return;
        }

        var parts = computed.Split('-');

        if (parts.Length == 2)
        {
            suggestions.Add($"{parts[0].Trim()} - {parts[1].Trim()}");
            suggestions.Add($"{SelectedArtist} - {parts[0].Trim()} - {parts[1].Trim()}");
        }
        else if (parts.Length > 2)
        {
            var mainPart = parts[0].Trim();
            var secondPart = parts[1].Trim();
            var remainingParts = string.Join(" ", parts[2..]).Trim();

            suggestions.Add($"{mainPart} - {secondPart}");
            suggestions.Add($"{SelectedArtist} - {mainPart} - {secondPart} {remainingParts}");
        }
    }

    private void AppendSuggestion_SeparatorlessAndIncomplete(List<string> suggestions, string name)
    {
        var computed = WhitespaceCompressorRegex().Replace(name, " ").Trim();
        if (string.IsNullOrWhiteSpace(computed) || name.Contains('-'))
        {
            return;
        }

        var parts = computed.Split(' ');
        
        if (name.Contains('-')) return;

        if (parts.Length == 1)
        {
            suggestions.Add($"{SelectedArtist} - {parts[0].Trim()}");
        }
        else if (parts.Length == 2)
        {
            suggestions.Add($"{parts[0].Trim()} - {parts[1].Trim()}");
            suggestions.Add($"{SelectedArtist} - {parts[0].Trim()} {parts[1].Trim()}");
        }
        else if (parts.Length > 2)
        {
            var firstPart = parts[0].Trim();
            var secondPart = parts[1].Trim();
            var remainingParts = string.Join(" ", parts[2..]).Trim();

            suggestions.Add($"{firstPart} - {string.Join(" ", parts[1..]).Trim()}");
            suggestions.Add($"{firstPart} - {secondPart} ({remainingParts})");
            suggestions.Add($"{firstPart} {secondPart} - {remainingParts}");
            suggestions.Add($"{SelectedArtist} - {computed}");
        }
    }

    #endregion
}