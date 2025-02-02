using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using MassRename.Services;
using MassRename.Services.Abstractions;
using MassRename.Views.Avalonia.Helpers;
using MassRename.Views.Avalonia.Services.Abstractions;

namespace MassRename.Views.Avalonia.Services;

/// <summary>
///     A <see langword="class" /> that implements the <see cref="IFilePickerService" /> <see langword="interface" /> using
///     <see cref="IStorageProvider"/> and <see cref="IStorageFile"/>
/// </summary>
public sealed class FilePickerService : IFilePickerService
{
    private FilePickerService() { }

    public static FilePickerService Instance { get; } = new();

    public async Task<string[]?> ShowOpenFilePickerAsync(string title = "Open file...", IReadOnlyList<FilePickerOption>? options = null, bool allowMultiple = false)
    {
        if (!Dispatcher.UIThread.CheckAccess())
            return await Dispatcher.UIThread.InvokeAsync(() => ShowOpenFilePickerAsync(title, options, allowMultiple));

        var provider = WindowHelper.GetFirstActiveWindow().StorageProvider;
        var storageFiles = await provider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = title,
            FileTypeFilter = options?.Select(FilePickerTypeExtensions.ToFilePickerFileType).ToArray(),
            AllowMultiple = allowMultiple
        });
        return storageFiles.Count != 0 ? storageFiles.Select(sf => sf.Path.LocalPath).ToArray() : null;
    }

    public async Task<string?> ShowSaveFilePickerAsync(string title = "Open file...", IReadOnlyList<FilePickerOption>? options = null)
    {
        if (!Dispatcher.UIThread.CheckAccess())
            return await Dispatcher.UIThread.InvokeAsync(() => ShowSaveFilePickerAsync(title, options));

        var provider = WindowHelper.GetFirstActiveWindow().StorageProvider;
        var storageFile = await provider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = title,
            FileTypeChoices = options?.Select(FilePickerTypeExtensions.ToFilePickerFileType).ToArray()
        });
        return storageFile?.Path.LocalPath;
    }

    public async Task<string[]?> ShowOpenFolderPickerAsync(string title = "Open folder...", bool allowMultiple = false)
    {
        if (!Dispatcher.UIThread.CheckAccess())
            return await Dispatcher.UIThread.InvokeAsync(() => ShowOpenFolderPickerAsync(title, allowMultiple));
        var provider = WindowHelper.GetFirstActiveWindow().StorageProvider;
        var storageFolders = await provider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            Title = title,
            AllowMultiple = allowMultiple
        });
        return storageFolders.Count != 0 ? storageFolders.Select(sf => sf.Path.AbsolutePath).ToArray() : null;
    }
}