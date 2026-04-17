namespace EverDrive64SdRepair.Services;

public sealed class DriveDetectionService
{
    public IReadOnlyList<DriveInfo> GetRemovableDrives()
    {
        return DriveInfo.GetDrives()
            .Where(d => d.DriveType == DriveType.Removable && d.IsReady)
            .OrderBy(d => d.Name)
            .ToList();
    }

    public string DescribeDrive(DriveInfo drive)
    {
        string label = string.IsNullOrWhiteSpace(drive.VolumeLabel) ? "No Label" : drive.VolumeLabel;
        double gb = drive.TotalSize / 1024d / 1024d / 1024d;
        return $"{drive.Name} - {label} - {gb:F1} GB - {drive.DriveFormat}";
    }

    public bool IsLargeCard(DriveInfo drive)
    {
        const long ThirtyTwoGb = 32L * 1024L * 1024L * 1024L;
        return drive.TotalSize > ThirtyTwoGb;
    }
}
