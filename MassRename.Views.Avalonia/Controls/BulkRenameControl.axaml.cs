using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MassRename.ViewModels;

namespace MassRename.Views.Avalonia.Controls;

public partial class BulkRenameControl : UserControl
{
    private BulkRenameViewModel BulkRenameViewModel => (BulkRenameViewModel)DataContext!;

    public BulkRenameControl()
    {
        InitializeComponent();
    }

    private void ApplyButton_OnClick(object? _, RoutedEventArgs __)
    {
        BulkRenameViewModel.ApplyCommand.Execute(CustomNameTextBox.Text);
    }

    private void CustomNameTextBox_OnKeyUp(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            ApplyButton_OnClick(null, null!);
        }
    }
}