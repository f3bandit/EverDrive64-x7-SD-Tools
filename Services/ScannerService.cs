using System.Security.Cryptography;
using EverDrive64SdRepair.Models;

namespace EverDrive64SdRepair.Services;

public sealed class ScannerService
{
    private readonly PartitionCheckService _partitionCheckService = new();
    private readonly SystemFileService _systemFileService = new();

    public async Task<ScanResult> ScanAsync(DriveInfo drive, IProgress<ScanProgress>? progress, CancellationToken cancellationToken)
    {
        var result = new ScanResult();
        string root = drive.RootDirectory.FullName;

        progress?.Report(new ScanProgress
        {
            Percent = 0,
            CurrentPath = root,
            Activity = "Starting SD card scan..."
        });

        _partitionCheckService.Evaluate(drive, result);
        if (result.HasPartitionIssue)
        {
            result.FailedChecks += result.Issues.Count;
        }
        else
        {
            result.PassedChecks++;
            progress?.Report(new ScanProgress
            {
                Percent = 2,
                CurrentPath = root,
                Activity = "PASS - Partition and filesystem look usable."
            });
        }

        string[] requiredItems = _systemFileService.GetRequiredSystemItems();
        int totalEntries = CountEntriesForScan(root) + requiredItems.Length + 1;
        int processed = 0;

        foreach (string item in requiredItems)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string path = Path.Combine(root, item);
            bool exists = Directory.Exists(path) || File.Exists(path);
            processed++;
            if (exists)
            {
                result.PassedChecks++;
                progress?.Report(new ScanProgress
                {
                    Percent = processed * 100 / Math.Max(totalEntries, 1),
                    CurrentPath = path,
                    Activity = $"PASS - Required item present: {item}"
                });
            }
            else
            {
                result.FailedChecks++;
                result.Issues.Add(new ScanIssue
                {
                    Title = "Missing system item",
                    Details = $"Required item '{item}' was not found.",
                    RequiresFullRebuild = false
                });
                progress?.Report(new ScanProgress
                {
                    Percent = processed * 100 / Math.Max(totalEntries, 1),
                    CurrentPath = path,
                    Activity = $"FAIL - Required item missing: {item}"
                });
            }
        }

        var scanRoots = new List<string>();
        foreach (string name in new[] { "ED64", "THEMES", "N64", "TOOLS" })
        {
            string candidate = Path.Combine(root, name);
            if (Directory.Exists(candidate))
                scanRoots.Add(candidate);
        }

        if (scanRoots.Count == 0)
        {
            scanRoots.Add(root);
        }

        foreach (string scanRoot in scanRoots)
        {
            await ScanDirectoryAsync(scanRoot, result, progress, cancellationToken, processed, totalEntries);
            processed += CountEntriesForScan(scanRoot);
        }

        progress?.Report(new ScanProgress
        {
            Percent = 100,
            CurrentPath = root,
            Activity = $"Scan complete. Directories scanned: {result.DirectoriesScanned}, files scanned: {result.FilesScanned}, passed: {result.PassedChecks}, failed: {result.FailedChecks}, CRC files: {result.CrcFilesComputed}."
        });

        return result;
    }

    private async Task ScanDirectoryAsync(string directory, ScanResult result, IProgress<ScanProgress>? progress, CancellationToken cancellationToken, int processedBase, int totalEntries)
    {
        cancellationToken.ThrowIfCancellationRequested();

        result.DirectoriesScanned++;
        progress?.Report(new ScanProgress
        {
            Percent = Math.Min(99, processedBase * 100 / Math.Max(totalEntries, 1)),
            CurrentPath = directory,
            Activity = "Scanning directory..."
        });

        string[] childDirectories;
        string[] childFiles;

        try
        {
            childDirectories = Directory.GetDirectories(directory);
            childFiles = Directory.GetFiles(directory);
            result.PassedChecks++;
            progress?.Report(new ScanProgress
            {
                Percent = Math.Min(99, processedBase * 100 / Math.Max(totalEntries, 1)),
                CurrentPath = directory,
                Activity = "PASS - Directory readable"
            });
        }
        catch (Exception ex)
        {
            result.FailedChecks++;
            result.Issues.Add(new ScanIssue
            {
                Title = "Unreadable directory",
                Details = $"Could not read directory '{directory}': {ex.Message}",
                RequiresFullRebuild = false
            });
            progress?.Report(new ScanProgress
            {
                Percent = Math.Min(99, processedBase * 100 / Math.Max(totalEntries, 1)),
                CurrentPath = directory,
                Activity = $"FAIL - Directory unreadable ({ex.Message})"
            });
            return;
        }

        int localProcessed = processedBase + 1;

        foreach (string file in childFiles)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await ScanFileAsync(file, result, progress, cancellationToken, localProcessed, totalEntries);
            localProcessed++;
        }

        foreach (string childDirectory in childDirectories)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await ScanDirectoryAsync(childDirectory, result, progress, cancellationToken, localProcessed, totalEntries);
            localProcessed += CountEntriesForScan(childDirectory);
        }
    }

    private async Task ScanFileAsync(string file, ScanResult result, IProgress<ScanProgress>? progress, CancellationToken cancellationToken, int processed, int totalEntries)
    {
        result.FilesScanned++;
        progress?.Report(new ScanProgress
        {
            Percent = Math.Min(99, processed * 100 / Math.Max(totalEntries, 1)),
            CurrentPath = file,
            Activity = "Scanning file..."
        });

        try
        {
            var info = new FileInfo(file);
            using var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var sha = SHA256.Create();
            byte[] hash = await sha.ComputeHashAsync(stream, cancellationToken);
            string sha256 = Convert.ToHexString(hash);
            result.CrcFilesComputed++;
            result.PassedChecks++;
            progress?.Report(new ScanProgress
            {
                Percent = Math.Min(99, processed * 100 / Math.Max(totalEntries, 1)),
                CurrentPath = file,
                Activity = $"PASS - File readable, size {info.Length} bytes, SHA-256 {sha256}"
            });
        }
        catch (Exception ex)
        {
            result.FailedChecks++;
            result.Issues.Add(new ScanIssue
            {
                Title = "Unreadable file",
                Details = $"Could not read file '{file}': {ex.Message}",
                RequiresFullRebuild = false
            });
            progress?.Report(new ScanProgress
            {
                Percent = Math.Min(99, processed * 100 / Math.Max(totalEntries, 1)),
                CurrentPath = file,
                Activity = $"FAIL - File unreadable ({ex.Message})"
            });
        }
    }

    private static int CountEntriesForScan(string root)
    {
        try
        {
            int directories = Directory.GetDirectories(root, "*", SearchOption.AllDirectories).Length;
            int files = Directory.GetFiles(root, "*", SearchOption.AllDirectories).Length;
            return directories + files + 1;
        }
        catch
        {
            return 1;
        }
    }
}
