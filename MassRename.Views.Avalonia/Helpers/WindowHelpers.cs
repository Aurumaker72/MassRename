using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace MassRename.Views.Avalonia.Helpers;

internal static class WindowHelper
{
    /// <summary>
    ///     Gets the first active <see cref="Window" /> from the <see cref="Application" />'s window collection
    /// </summary>
    /// <returns>The first active <see cref="Window" />, or <see cref="MainWindow" /></returns>
    internal static Window GetFirstActiveWindow()
    {
        return ((IClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!).Windows
            .FirstOrDefault(
                x => x?.IsActive ?? false,
                (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!.MainWindow!);
    }

    internal static IEnumerable<Window> IterateWindows()
    {
        return ((IClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!).Windows;
    }

    internal static Views.MainWindow MainWindow =>
        (Views.MainWindow)(Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!.MainWindow;
}