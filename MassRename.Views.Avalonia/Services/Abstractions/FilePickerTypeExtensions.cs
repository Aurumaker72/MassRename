using Avalonia.Platform.Storage;
using MassRename.Services.Abstractions;

namespace MassRename.Views.Avalonia.Services.Abstractions;

public static class FilePickerTypeExtensions
{
    /// <summary>
    /// Converts a <see cref="FilePickerOption"/> to a <see cref="FilePickerFileType"/>.
    /// </summary>
    /// <param name="option">The file picker option.</param>
    /// <returns>the equivalent FilePickerFileType</returns>
    public static FilePickerFileType ToFilePickerFileType(this FilePickerOption option)
    {
        return new FilePickerFileType(option.Name)
        {
            Patterns = option.Patterns,
            AppleUniformTypeIdentifiers = option.AppleTypeIds
        };
    }
}