using System.IO;
using System.Text.Json;
using knotless.models;

namespace knotless.services;

public static class ConfigLoader
{
    private const string configpath = "settings.json";

    public static AppConfig Load()
    {
        if (!File.Exists(configpath))
            return new AppConfig(); // if file doesn't exist, return default

        string json = File.ReadAllText(configpath);

        // magic of turning text into objects
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        return JsonSerializer.Deserialize<AppConfig>(json, options) ?? new AppConfig();
    }

    public static void Save(AppConfig config)
    {
        // serialize back into json, beautifully formatted
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(config, options);
        File.WriteAllText(configpath, json);
    }
}