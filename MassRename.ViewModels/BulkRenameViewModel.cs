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
    /// <param name="entries">The amount of entries to skip</param>
    [RelayCommand]
    private void SkipBy(int entries)
    {
        if (CurrentIndex + entries >= FileCount)
        {
            _navigationService.SwitchTo(null);
            return;
        }

        CurrentIndex += entries;
        NameSuggestions = ComputeSuggestions(CurrentFile).ToArray();

        OnPropertyChanged(nameof(CurrentIndex));
        OnPropertyChanged(nameof(CurrentFile));
        OnPropertyChanged(nameof(NameSuggestions));
    }

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

        suggestions.AddRange(ComputeSuggestion_SpacedFused(name));

        return suggestions;
    }

    private List<string> ComputeSuggestion_SpacedFused(string name)
    {
        // 1. Compress repeated spcaes before splitting
        var computed = WhitespaceCompressorRegex().Replace(name, "");

        // 2. Split the string by spaces
        var parts = computed.Split(' ');

        // 3. If there are 2 or less segments, we bail
        if (parts.Length < 2)
        {
            return [];
        }

        // 4. Exactly two segments, we can keep it
        if (parts.Length == 2)
        {
            return [$"{parts[0]} - {parts[1]}"];
        }

        // 5. More than 2 segments, fuse the ones following the first one in-place
        // Artist - Track - Something - Else
        // vvvv
        // Artist - Track (Something Else)
        
        var tail = parts[1..];
        var fused = string.Join(" ", tail);
        
        return [$"{parts[0]} - ({fused})"];
    }
}