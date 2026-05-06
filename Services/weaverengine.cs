using System;
using System.IO;
using knotless.models;

namespace knotless.services;

public class WeaverEngine
{
    public (int moved, int deleted) StartWeaving(AppConfig config)
    {
        string desktop = config.TargetPath;

        // if we're in WSL (and we are), look for Windows Desktop
        if (string.IsNullOrEmpty(desktop))
        {
            // check the most common WSL path
            string mntPath = "/mnt/c/Users";
            if (Directory.Exists(mntPath))
            {
                var userDirs = Directory.GetDirectories(mntPath);
                foreach (var dir in userDirs)
                {
                    string potential = Path.Combine(dir, "Desktop");
                    // Ignore system junk
                    if (Directory.Exists(potential) && !dir.EndsWith("Public") && !dir.EndsWith("Default") && !dir.EndsWith("Default User"))
                    {
                        desktop = potential;
                        break;
                    }
                }
            }
        }

        // if we're still empty, take current project folder
        if (string.IsNullOrEmpty(desktop))
        {
            desktop = Directory.GetCurrentDirectory();
        }

        Console.WriteLine($"[info] final target folder: {desktop}");

        int filesMoved = 0;
        int filesDeleted = 0;

        foreach (var rule in config.rules)
        {
            foreach (var ext in rule.extensions)
            {
                // find all files with that extension on desktop
                var files = Directory.GetFiles(desktop, $"*{ext}");

                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    // take creation or last write date
                    DateTime creationTime = fileInfo.LastWriteTime;

                    // making a nice path: folder/year/month
                    string year = creationTime.ToString("yyyy");
                    string month = creationTime.ToString("MMMM").ToLower(); // 'may', 'june' and so on

                    string targetdir = Path.Combine(desktop, rule.folder, year, month);

                    if (!Directory.Exists(targetdir))
                        Directory.CreateDirectory(targetdir);

                    string filename = Path.GetFileName(file);
                    string destination = Path.Combine(targetdir, filename);

                    // move the file (and wrap in try-catch in case the file is open)
                    try
                    {
                        File.Move(file, destination);
                        Console.WriteLine($"[woven] {filename} -> {rule.folder}/{year}/{month}");
                        filesMoved++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[error] couldn't move {filename}: {ex.Message}");
                    }
                }
            }
        }

        // the black hole
        if (config.black_hole != null && config.black_hole.enabled)
        {
            string blackHoleDir = Path.Combine(desktop, config.black_hole.folder);
            if (Directory.Exists(blackHoleDir))
            {
                var allFiles = Directory.GetFiles(blackHoleDir);
                foreach (var file in allFiles)
                {
                    var fileInfo = new FileInfo(file);
                    var age = DateTime.Now - fileInfo.LastWriteTime;

                    if (age.TotalHours >= config.black_hole.max_age_hours)
                    {
                        try
                        {
                            File.Delete(file);
                            Console.WriteLine($"[black hole] swallowed {Path.GetFileName(file)}");
                            filesDeleted++;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[error] black hole couldn't swallow {Path.GetFileName(file)}: {ex.Message}");
                        }
                    }
                }
            }
        }

        Console.WriteLine($"[info] weaving complete. total files moved: {filesMoved}, swallowed: {filesDeleted}");
        return (filesMoved, filesDeleted);
    }
}