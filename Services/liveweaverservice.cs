using System;
using System.IO;
using knotless.models;
using knotless.services;

namespace knotless.services;

public class LiveWeaverService
{
    private FileSystemWatcher? _watcher;
    private readonly WeaverEngine _engine;
    private AppConfig? _config;

    public LiveWeaverService()
    {
        _engine = new WeaverEngine();
    }

    public void Start(AppConfig config)
    {
        _config = config;
        string path = config.TargetPath;

        if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
            return;

        _watcher = new FileSystemWatcher(path);

        // watching for new files
        _watcher.Created += OnChanged;
        // watching for file moves
        _watcher.Renamed += OnChanged;

        _watcher.EnableRaisingEvents = true;
        Console.WriteLine($"[live] weaver is watching: {path}");
    }

    public void Stop()
    {
        if (_watcher != null)
        {
            _watcher.EnableRaisingEvents = false;
            _watcher.Dispose();
        }
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        // small pause, so the system has time to "write" the file
        System.Threading.Thread.Sleep(500);

        if (_config != null)
        {
            Console.WriteLine($"[live] detected: {e.Name}. weaving...");
            _engine.StartWeaving(_config);
        }
    }
}