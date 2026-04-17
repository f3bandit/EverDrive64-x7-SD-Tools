using EverDrive64SdRepair.Models;

namespace EverDrive64SdRepair.Services;

public sealed class PartitionCheckService
{
    public void Evaluate(DriveInfo drive, ScanResult result)
    {
        if (!drive.IsReady)
        {
            result.HasPartitionIssue = true;
            result.Issues.Add(new ScanIssue
            {
                Title = "Drive not ready",
                Details = "The removable drive is not mounted and ready.",
                RequiresFullRebuild = true
            });
            return;
        }

        if (!string.Equals(drive.DriveFormat, "FAT32", StringComparison.OrdinalIgnoreCase))
        {
            result.HasPartitionIssue = true;
            result.Issues.Add(new ScanIssue
            {
                Title = "Unsupported file system",
                Details = $"Detected file system: {drive.DriveFormat}. FAT32 is required.",
                RequiresFullRebuild = true
            });
        }

        if (drive.AvailableFreeSpace <= 0)
        {
            result.Issues.Add(new ScanIssue
            {
                Title = "No free space",
                Details = "The card reports no free writable space.",
                RequiresFullRebuild = false
            });
        }
    }
}
