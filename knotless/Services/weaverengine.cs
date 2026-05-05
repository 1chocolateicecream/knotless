using System;
using System.IO;
using knotless.models;

namespace knotless.services;

public class WeaverEngine
{
    public void StartWeaving(AppConfig config)
    {
        string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        if (string.IsNullOrEmpty(desktop) || !Directory.Exists(desktop))
        {
            // If Desktop folder is not found, try to use the current directory or home
            desktop = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Desktop");
            if (!Directory.Exists(desktop))
            {
                desktop = Directory.GetCurrentDirectory();
                Console.WriteLine($"[warning] desktop not found, using: {desktop}");
            }
        }

        foreach (var rule in config.rules)
        {
            // make path to target folder (e.g., desktop/images)
            string targetdir = Path.Combine(desktop, rule.folder);

            foreach (var ext in rule.extensions)
            {
                // find all files with that extension on desktop
                var files = Directory.GetFiles(desktop, $"*{ext}");

                foreach (var file in files)
                {
                    if (!Directory.Exists(targetdir))
                        Directory.CreateDirectory(targetdir);

                    string filename = Path.GetFileName(file);
                    string destination = Path.Combine(targetdir, filename);

                    // move the file (and wrap in try-catch in case the file is open)
                    try
                    {
                        File.Move(file, destination);
                        Console.WriteLine($"[woven] {filename} -> {rule.folder}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[error] couldn't move {filename}: {ex.Message}");
                    }
                }
            }
        }
    }
}