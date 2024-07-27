using MassRename.Services;
using NSubstitute;

namespace MassRename.ViewModels.Tests;

public class BulkRenameViewModel_Tests
{
    private static readonly Dictionary<string, List<string>> NameSuggestionMap = new()
    {
        { "-", [""] },
        { " -", [""] },
        { " - ", [""] },
        { " -  ", [""] },
        { "Lorem - Ipsum", ["Lorem - Ipsum", "@artist - Lorem - Ipsum"] },
        { "Lorem -Ipsum", ["Lorem - Ipsum", "@artist - Lorem - Ipsum"] },
        { "Lorem-Ipsum", ["Lorem - Ipsum", "@artist - Lorem - Ipsum"] },
        { "Lorem  -    Ipsum", ["Lorem - Ipsum", "@artist - Lorem - Ipsum"] },
        { "Lorem Ipsum", ["Lorem - Ipsum", "@artist - Lorem Ipsum"] },
        { "Lorem", ["@artist - Lorem"] },
        {
            "Lorem Ipsum Dolor",
            ["Lorem - Ipsum Dolor", "Lorem - Ipsum (Dolor)", "Lorem Ipsum - Dolor", "@artist - Lorem Ipsum Dolor"]
        },
    };

    public static IEnumerable<object[]> SuggestionData =>
        new List<object[]>
        {
            new object[] { NameSuggestionMap }
        };

    [Fact]
    public void Test_ApplyAdvancesIndexWhenNotAtEnd()
    {
        var bulkRenameViewModel = new BulkRenameViewModel(new[] { "abc.mp3", "def.flac", "ghi.ogg" }, null);

        var prevIndex = bulkRenameViewModel.CurrentIndex;

        bulkRenameViewModel.ApplyCommand.Execute("Something");

        Assert.Equal(prevIndex + 1, bulkRenameViewModel.CurrentIndex);
    }

    [Fact]
    public void Test_ApplySwitchesToDefaultViewModelWhenAtEnd()
    {
        var navigationService = Substitute.For<INavigationService>();
        var bulkRenameViewModel = new BulkRenameViewModel(["abc.mp3", "def.flac", "ghi.ogg"], navigationService);

        bulkRenameViewModel.ApplyCommand.Execute("Something");
        bulkRenameViewModel.ApplyCommand.Execute("Something");
        bulkRenameViewModel.ApplyCommand.Execute("Something");

        navigationService.Received(1)
            .SwitchTo(Arg.Is<FileCollectionSelectionViewModel>(vm => vm == null));
    }
}