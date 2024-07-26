using System.Threading.Tasks;
using MassRename.Services;
using MassRename.Services.Abstractions;

namespace MassRename.Views.Avalonia.Services;

public class ViewDialogService : IViewDialogService
{
    private ViewDialogService() { }

    public static ViewDialogService Instance { get; } = new();

    
    public async Task ShowMessageDialog(MessageDialogType type, string content)
    {
        // TODO: Implement
    }
}