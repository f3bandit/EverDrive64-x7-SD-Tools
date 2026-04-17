namespace EverDrive64SdRepair.Models;

public sealed class BackupManifest
{
    public string SourceDriveRoot { get; init; } = string.Empty;
    public DateTime CreatedUtc { get; init; } = DateTime.UtcNow;
    public List<string> BackedUpCategories { get; init; } = new();
}
