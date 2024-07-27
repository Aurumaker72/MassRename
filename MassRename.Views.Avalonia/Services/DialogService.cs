using System.Threading.Tasks;
using MassRename.Services;
using MassRename.Services.Abstractions;

namespace MassRename.Views.Avalonia.Services;

public class DialogService : IDialogService
{
    private DialogService() { }

    public static DialogService Instance { get; } = new();

    
    public async Task ShowMessageDialog(MessageDialogType type, string content)
    {
        // TODO: Implement
    }
}