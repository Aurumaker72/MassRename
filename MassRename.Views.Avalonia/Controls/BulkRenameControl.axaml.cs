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

    private void AddArtistButton_OnClick(object? _, RoutedEventArgs __)
    {
        var artist = ArtistTextBox.Text;

        if (artist is null)
        {
            return;
        }
        
        BulkRenameViewModel.KnownArtists.Remove(artist);
        BulkRenameViewModel.KnownArtists.Insert(0, artist);
    }
    
    private void ArtistTextBox_OnKeyUp(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            AddArtistButton_OnClick(null, null!);
        }
    }
}