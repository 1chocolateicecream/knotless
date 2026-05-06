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
    private readonly LiveWeaverService _liveService = new();
    public string Greeting => "welcome to knotless!";

    private string _statusMessage = "ready to weave";
    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    private bool _isLiveActive;
    public string LiveStatus => _isLiveActive ? "live mode: active" : "live mode: off";

    // method that will be called by the button
    public async Task StartWeavingCommand()
    {
        try
        {
            StatusMessage = "weaving... please wait.";
            var config = ConfigLoader.Load();
            var engine = new WeaverEngine();

            // run heavy lifting in the background to keep UI responsive
            var result = await Task.Run(() => engine.StartWeaving(config));

            StatusMessage = $"chaos refined. moved: {result.moved}, swallowed: {result.deleted}.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"error: {ex.Message.ToLower()}";
        }
    }

    public void ToggleLiveCommand()
    {
        try
        {
            var config = ConfigLoader.Load();
            if (!_isLiveActive)
            {
                _liveService.Start(config);
                _isLiveActive = true;
                StatusMessage = "live mode started. watching for changes...";
            }
            else
            {
                _liveService.Stop();
                _isLiveActive = false;
                StatusMessage = "live mode stopped.";
            }
            OnPropertyChanged(nameof(LiveStatus));
        }
        catch (Exception ex)
        {
            StatusMessage = $"error: {ex.Message.ToLower()}";
        }
    }

    public async Task SelectFolderCommand() // method for the new button
    {
        // find the main window to open the dialog
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
                ConfigLoader.Save(config); // saving directly to settings.json
                StatusMessage = $"target set to: {config.TargetPath.ToLower()}";
            }
        }
    }
}