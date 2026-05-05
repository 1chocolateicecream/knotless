using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using knotless.services;

namespace knotless.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public string Greeting => "welcome to knotless!";

    private string _statusMessage = "ready to weave";
    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    // method that will be called by the button
    public void StartWeavingCommand()
    {
        try
        {
            var config = ConfigLoader.Load();
            var engine = new WeaverEngine();
            engine.StartWeaving(config);
            StatusMessage = "weaving complete. chaos refined.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"error: {ex.Message.ToLower()}";
        }
    }

    public async Task SelectFolderCommand() // method for the new button
    {
        // ищем главное окно
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var topLevel = TopLevel.GetTopLevel(desktop.MainWindow);
            if (topLevel == null)
                return;

            // open folder dialog
            var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = "where should we weave?",
                AllowMultiple = false
            });

            if (folders.Count > 0)
            {
                var config = ConfigLoader.Load();
                config.TargetPath = folders[0].Path.LocalPath;
                // we could save it to settings.json right away, but for now let's just remember it
                StatusMessage = $"target set to: {config.TargetPath.ToLower()}";
            }
        }
    }
}