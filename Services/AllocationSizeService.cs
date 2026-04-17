namespace EverDrive64SdRepair.Services;

public sealed class AllocationSizeService
{
    // This build of fat32format maps -c64 -> 32768 byte clusters on a 32GB card.
    // Large-card mapping for 65536-byte clusters should be validated with the user's formatter build.
    public string GetBestFormatterClusterArgument(long totalBytes)
    {
        const long ThirtyTwoGb = 32L * 1024L * 1024L * 1024L;
        return totalBytes <= ThirtyTwoGb ? "64" : "128";
    }

    public int GetBestClusterSizeBytes(long totalBytes)
    {
        const long ThirtyTwoGb = 32L * 1024L * 1024L * 1024L;
        return totalBytes <= ThirtyTwoGb ? 32768 : 65536;
    }

    public int GetBestClusterSizeKb(long totalBytes) => GetBestClusterSizeBytes(totalBytes) / 1024;
}
