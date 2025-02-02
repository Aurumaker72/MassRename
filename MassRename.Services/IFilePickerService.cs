﻿using MassRename.Services.Abstractions;

namespace MassRename.Services;

/// <summary>
///     Interface for a service exposing a file picker from the view.
/// </summary>
public interface IFilePickerService
{
    /// <summary>
    /// Shows a file picker to open a file with the specified options.
    /// </summary>
    /// <param name="title">The dialog's title.</param>
    /// <param name="options">The options that may be used.</param>
    /// <param name="allowMultiple">If true, allow multiple files to be selected.</param>
    /// <returns>A <see cref="string[]"/> with the selected files' paths, if available</returns>
    Task<string[]?> ShowOpenFilePickerAsync(string title = "Open file...",
        IReadOnlyList<FilePickerOption>? options = null, bool allowMultiple = false);

    /// <summary>
    /// Shows a file picker to open a file with the specified options.
    /// </summary>
    /// <param name="title">The dialog's title.</param>
    /// <param name="options">The options that may be used.</param>
    /// <returns>A <see cref="string"/> with the selected file's path, if available</returns>
    Task<string?> ShowSaveFilePickerAsync(string title = "Save file...",
        IReadOnlyList<FilePickerOption>? options = null);
    
    /// <summary>
    /// Shows a folder picker to open a folder with the specified options.
    /// </summary>
    /// <param name="title">The dialog's title.</param>
    /// <param name="allowMultiple">If true, allow multiple files to be selected.</param>
    /// <returns>A <see cref="string[]"/> with the selected folders' paths, if available</returns>
    Task<string[]?> ShowOpenFolderPickerAsync(string title = "Open folder...", bool allowMultiple = false);
}