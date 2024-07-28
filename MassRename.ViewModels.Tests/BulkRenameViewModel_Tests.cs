using MassRename.Services;
using NSubstitute;
using Xunit.Abstractions;

namespace MassRename.ViewModels.Tests;

public class BulkRenameViewModel_Tests
{
    [Fact]
    public void Test_ApplyAdvancesIndexWhenNotAtEnd()
    {
        var bulkRenameViewModel = new BulkRenameViewModel(new[] { "abc.mp3", "def.flac", "ghi.ogg" }, null);

        var prevIndex = bulkRenameViewModel.CurrentIndex;

        bulkRenameViewModel.ApplyCommand.Execute("Something");

        Assert.Equal(prevIndex + 1, bulkRenameViewModel.CurrentIndex);
    }

    [Fact]
    public void Test_ApplySwitchesToDefaultViewModelWhenApplyingAtEnd()
    {
        var navigationService = Substitute.For<INavigationService>();
        var bulkRenameViewModel = new BulkRenameViewModel(["abc.mp3", "def.flac", "ghi.ogg"], navigationService);

        bulkRenameViewModel.ApplyCommand.Execute("Something");
        bulkRenameViewModel.ApplyCommand.Execute("Something");
        bulkRenameViewModel.ApplyCommand.Execute("Something");

        navigationService.Received(1)
            .SwitchTo(Arg.Is<FileCollectionSelectionViewModel>(vm => vm == null));
    }

    [Theory]
    [InlineData(3)]
    [InlineData(50)]
    [InlineData(100)]
    public void Test_SkipSwitchesToDefaultViewModelPastEnd(int skipSize)
    {
        var navigationService = Substitute.For<INavigationService>();
        var bulkRenameViewModel = new BulkRenameViewModel(["abc.mp3", "def.flac", "ghi.ogg"], navigationService);

        bulkRenameViewModel.SkipByCommand.Execute(skipSize);

        navigationService.Received(1)
            .SwitchTo(Arg.Is<FileCollectionSelectionViewModel>(vm => vm == null));
    }

    [Theory]
    [InlineData(new[] { "abc", "def", "ghi", "jkl" }, 0)]
    [InlineData(new[] { "abc", "def", "ghi", "jkl" }, 1)]
    [InlineData(new[] { "abc", "def", "ghi", "jkl" }, 2)]
    [InlineData(new[] { "abc", "def", "ghi", "jkl" }, 3)]
    public void Test_CurrentItemMatchesDataAfterSkip(IList<string> items, int skipSize)
    {
        var navigationService = Substitute.For<INavigationService>();
        var bulkRenameViewModel = new BulkRenameViewModel(items.ToList(), navigationService);

        bulkRenameViewModel.SkipByCommand.Execute(skipSize);

        Assert.Equal(items[skipSize], bulkRenameViewModel.CurrentFile);
    }

    [Theory]
    [InlineData(new[] { "abc", "def", "ghi", "jkl" }, -1)]
    [InlineData(new[] { "abc", "def", "ghi", "jkl" }, -2)]
    [InlineData(new[] { "abc", "def", "ghi", "jkl" }, -3)]
    public void Test_CurrentItemMatchesDataAfterSkipBackwards(IList<string> items, int skipSize)
    {
        var bulkRenameViewModel = new BulkRenameViewModel(items.ToList(), null);

        bulkRenameViewModel.SkipByCommand.Execute(items.Count - 1);
        bulkRenameViewModel.SkipByCommand.Execute(skipSize);

        Assert.Equal(items[^(-skipSize + 1)], bulkRenameViewModel.CurrentFile);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("abc", "def")]
    [InlineData("abc", "def", "ghi")]
    [InlineData("abc", "def", "ghi", "jkl")]
    public void Test_SkipBackwardsIsClamped(params string[] items)
    {
        var bulkRenameViewModel = new BulkRenameViewModel(items.ToList(), null);

        bulkRenameViewModel.SkipByCommand.Execute(items.Length - 1);
        bulkRenameViewModel.SkipByCommand.Execute(-1000);

        Assert.Equal(items[0], bulkRenameViewModel.CurrentFile);
    }

    [Theory]

    // Invalid cases: No valid suggestions 
    [InlineData("", new string[] { }, "Someone")]
    [InlineData(" ", new string[] { }, "Someone")]
    [InlineData("-", new string[] { }, "Someone")]
    [InlineData(" -", new string[] { }, "Someone")]
    [InlineData(" - ", new string[] { }, "Someone")]
    [InlineData(" -  ", new string[] { }, "Someone")]

    // Dash-separated segments with formatting variations
    [InlineData("Lorem - Ipsum", new[]
    {
        "Lorem - Ipsum",
        "@artist - Lorem - Ipsum"
    }, "Someone")]
    [InlineData("Lorem -Ipsum", new[]
    {
        "Lorem - Ipsum",
        "@artist - Lorem - Ipsum"
    }, "Someone")]
    [InlineData("Lorem-Ipsum", new[]
    {
        "Lorem - Ipsum",
        "@artist - Lorem - Ipsum"
    }, "Someone")]
    [InlineData("Lorem  -    Ipsum", new[]
    {
        "Lorem - Ipsum",
        "@artist - Lorem - Ipsum"
    }, "Someone")]
    [InlineData("Lorem - Ipsum Dolor", new[]
    {
        "Lorem - Ipsum Dolor",
        "@artist - Lorem - Ipsum Dolor"
    }, "Someone")]

    // Separatorless format and incomplete variations
    [InlineData("Lorem Ipsum", new[]
    {
        "Lorem - Ipsum",
        "@artist - Lorem Ipsum"
    }, "Someone")]
    [InlineData("Lorem", new[]
    {
        "@artist - Lorem"
    }, "Someone")]
    [InlineData("Lorem Ipsum Dolor", new[]
    {
        "Lorem - Ipsum Dolor",
        "Lorem - Ipsum (Dolor)",
        "Lorem Ipsum - Dolor",
        "@artist - Lorem Ipsum Dolor"
    }, "Someone")]
    public void Test_ValidateSuggestions(string name, ICollection<string> suggestions, string artist)
    {
        var bulkRenameViewModel = new BulkRenameViewModel([name], null);

        bulkRenameViewModel.SelectedArtist = artist;
        
        Assert.Equal(suggestions.Select(x => x.Replace("@artist", artist)), bulkRenameViewModel.NameSuggestions);
    }
}