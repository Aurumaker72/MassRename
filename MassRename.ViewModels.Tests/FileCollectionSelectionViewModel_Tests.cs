using MassRename.Services;
using MassRename.Services.Abstractions;
using MassRename.ViewModels.Extensions;
using NSubstitute;

namespace MassRename.ViewModels.Tests;

public class FileCollectionSelectionViewModel_Tests
{
    [Fact]
    public async Task Test_BrowseFilePickerNullResultCallsViewDialogServiceWithError()
    {
        var filePickerService = Substitute.For<IFilePickerService>();
        var viewDialogService = Substitute.For<IViewDialogService>();

        filePickerService.ShowOpenFilePickerAsync(allowMultiple: true).Returns((string[]?)null);

        FileCollectionSelectionViewModel fileCollectionSelectionViewModel = new(filePickerService, viewDialogService, null);
        await fileCollectionSelectionViewModel.BrowseCommand.ExecuteAsync(null);

        await viewDialogService.Received(1).ShowMessageDialog(Arg.Is<MessageDialogType>(x => x == MessageDialogType.Error), Arg.Any<string>());
    }

    [Theory]
    [InlineData("abc.mp3")]
    [InlineData("abc.mp3", "def.mp3")]
    public async Task Test_BrowseStoresCorrectDataIntoFileCollections(params string[] files)
    {
        var filePickerService = Substitute.For<IFilePickerService>();
        var viewDialogService = Substitute.For<IViewDialogService>();

        filePickerService.ShowOpenFilePickerAsync(allowMultiple: true).Returns(files);

        FileCollectionSelectionViewModel fileCollectionSelectionViewModel = new(filePickerService, viewDialogService, null);
        await fileCollectionSelectionViewModel.BrowseCommand.ExecuteAsync(null);

        Assert.Equal(files, fileCollectionSelectionViewModel.Files);
        Assert.Equal(files, fileCollectionSelectionViewModel.SelectedFiles);
    }

    [Theory]
    [InlineData("abc.mp3")]
    [InlineData("abc.mp3", "def.mp3")]
    public async Task Test_SelectAllWorks(params string[] files)
    {
        var filePickerService = Substitute.For<IFilePickerService>();
        var viewDialogService = Substitute.For<IViewDialogService>();

        filePickerService.ShowOpenFilePickerAsync(allowMultiple: true).Returns(files);

        FileCollectionSelectionViewModel fileCollectionSelectionViewModel = new(filePickerService, viewDialogService, null);
        await fileCollectionSelectionViewModel.BrowseCommand.ExecuteAsync(null);

        fileCollectionSelectionViewModel.SelectAllCommand.Execute(null);

        Assert.Equal(fileCollectionSelectionViewModel.Files, fileCollectionSelectionViewModel.SelectedFiles);
        Assert.Equal(files, fileCollectionSelectionViewModel.SelectedFiles);
    }

    [Theory]
    [InlineData("abc.mp3")]
    [InlineData("abc.mp3", "def.mp3")]
    public async Task Test_ClearAllWorks(params string[] files)
    {
        var filePickerService = Substitute.For<IFilePickerService>();
        var viewDialogService = Substitute.For<IViewDialogService>();

        filePickerService.ShowOpenFilePickerAsync(allowMultiple: true).Returns(files);

        FileCollectionSelectionViewModel fileCollectionSelectionViewModel = new(filePickerService, viewDialogService, null);
        await fileCollectionSelectionViewModel.BrowseCommand.ExecuteAsync(null);

        fileCollectionSelectionViewModel.ClearAllCommand.Execute(null);

        Assert.Empty(fileCollectionSelectionViewModel.Files);
        Assert.Empty(fileCollectionSelectionViewModel.SelectedFiles);
    }

    [Theory]
    [InlineData(new[] { "abc.mp3" }, new[] { "abc.mp3" }, new string[] { })]
    [InlineData(new[] { "abc.mp3" }, new[] { "abc.mp3", "def.mp3" }, new[] { "def.mp3" })]
    [InlineData(new[] { "abc.mp3", "def.mp3" }, new[] { "abc.mp3", "def.mp3" }, new string[] { })]
    [InlineData(new[] { "abc.mp3", "def.mp3", "ghi.mp3" }, new[] { "abc.mp3", "def.mp3" }, new string[] { })]
    public async Task Test_ClearSelectedWorks(string[] selected, string[] files, string[] expected)
    {
        var filePickerService = Substitute.For<IFilePickerService>();
        var viewDialogService = Substitute.For<IViewDialogService>();

        filePickerService.ShowOpenFilePickerAsync(allowMultiple: true).Returns(files);

        FileCollectionSelectionViewModel fileCollectionSelectionViewModel = new(filePickerService, viewDialogService, null);
        await fileCollectionSelectionViewModel.BrowseCommand.ExecuteAsync(null);

        fileCollectionSelectionViewModel.SelectedFiles.Clear();
        fileCollectionSelectionViewModel.SelectedFiles.AddRange(selected);

        fileCollectionSelectionViewModel.ClearSelectedCommand.Execute(null);

        Assert.Equal(expected, fileCollectionSelectionViewModel.Files);
        Assert.Empty(fileCollectionSelectionViewModel.SelectedFiles);
    }

    [Fact]
    private void Test_ContinueCallsNavigationServiceWithBulkRenameViewModel()
    {
        var navigationService = Substitute.For<INavigationService>();

        FileCollectionSelectionViewModel fileCollectionSelectionViewModel = new(null, null, navigationService);
        fileCollectionSelectionViewModel.ContinueCommand.Execute(null);

        navigationService.Received(1).SwitchTo(Arg.Is<object?>(x => x is BulkRenameViewModel));
    }
}