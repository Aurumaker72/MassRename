namespace MassRename.Services;

/// <summary>
/// Interface exposing navigation functionality provided by the view.
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Switches the navigation context to a ViewModel instance
    /// </summary>
    /// <param name="viewModel">The ViewModel backing the next view, or null if the default should be chosen</param>
    void SwitchTo(object? viewModel);
}