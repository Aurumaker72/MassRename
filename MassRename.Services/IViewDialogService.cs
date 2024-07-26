using MassRename.Services.Abstractions;

namespace MassRename.Services;

/// <summary>
/// Interface exposing custom dialogs provided by the view.
/// </summary>
public interface IViewDialogService
{
    /// <summary>
    /// Shows a message dialog with no choices 
    /// </summary>
    /// <param name="type">The type of message dialog</param>
    /// <param name="content">The message dialog's content</param>
    Task ShowMessageDialog(MessageDialogType type, string content);
}