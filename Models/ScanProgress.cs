namespace EverDrive64SdRepair.Models;

public sealed class ScanProgress
{
    public int Percent { get; init; }
    public string CurrentPath { get; init; } = string.Empty;
    public string Activity { get; init; } = string.Empty;
}
