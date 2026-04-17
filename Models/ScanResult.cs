namespace EverDrive64SdRepair.Models;

public sealed class ScanResult
{
    public List<ScanIssue> Issues { get; } = new();
    public bool HasPartitionIssue { get; set; }
    public int PassedChecks { get; set; }
    public int FailedChecks { get; set; }
    public int FilesScanned { get; set; }
    public int DirectoriesScanned { get; set; }
    public int CrcFilesComputed { get; set; }
    public bool RequiresFullRebuild => HasPartitionIssue || Issues.Any(i => i.RequiresFullRebuild);
}
