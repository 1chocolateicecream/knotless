using System.Collections.Generic;

namespace knotless.models;

// main config class
public class AppConfig
{
    public string project_name { get; set; } = "knotless";
    public string version { get; set; } = "0.1.0";
    public bool auto_mode { get; set; } = false;
    public string TargetPath { get; set; } = string.Empty;
    public List<SortRule> rules { get; set; } = new();
    public BlackHoleRule black_hole { get; set; } = new();
}

// rules for each folder
public class SortRule
{
    public string folder { get; set; } = string.Empty;
    public List<string> extensions { get; set; } = new();
}

// rules for the black hole
public class BlackHoleRule
{
    public bool enabled { get; set; } = true;
    public string folder { get; set; } = "blackhole";
    public double max_age_hours { get; set; } = 24.0;
}