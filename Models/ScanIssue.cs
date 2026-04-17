namespace EverDrive64SdRepair.Models;

public sealed class ScanIssue
{
    public string Title { get; init; } = string.Empty;
    public string Details { get; init; } = string.Empty;
    public bool RequiresFullRebuild { get; init; }

    public override string ToString() => $"{Title}: {Details}";
}
