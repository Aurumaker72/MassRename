namespace MassRename.Services.Abstractions;

/// <summary>
/// Represents a type of message dialog
/// </summary>
public enum MessageDialogType
{
    /// <summary>
    /// A dialog providing the user with neutral-sentiment information
    /// </summary>
    Information,
    
    /// <summary>
    /// A dialog warning the user about an action
    /// </summary>
    Warning,
    
    /// <summary>
    /// A dialog notifying the user about a failed action
    /// </summary>
    Error,
}