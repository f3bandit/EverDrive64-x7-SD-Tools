namespace EverDrive64SdRepair.Models;

public sealed class OsVersionInfo
{
    public string VersionLabel { get; init; } = string.Empty;
    public Uri DownloadUri { get; init; } = new("https://krikzz.com/");
    public DateTime? LastModifiedUtc { get; init; }
    public string SizeLabel { get; init; } = string.Empty;

    public override string ToString() => $"{VersionLabel} ({SizeLabel})";
}
