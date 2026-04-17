using System.Text.Json;
using EverDrive64SdRepair.Models;

namespace EverDrive64SdRepair.Services;

public sealed class BackupService
{
    private static readonly string[] RootDirectoriesToBackup = { "ED64", "THEMES", "N64", "TOOLS" };

    public async Task<string> CreateBackupAsync(DriveInfo drive, string backupRoot, IProgress<string>? status, CancellationToken cancellationToken)
    {
        string driveName = drive.Name.TrimEnd('\\').TrimEnd(':');
        string targetDir = Path.Combine(backupRoot, $"Backup_{driveName}_{DateTime.Now:yyyyMMdd_HHmmss}");
        Directory.CreateDirectory(targetDir);
        var categories = new List<string>();

        foreach (string name in RootDirectoriesToBackup)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string source = Path.Combine(drive.RootDirectory.FullName, name);
            if (!Directory.Exists(source))
                continue;

            string destination = Path.Combine(targetDir, name);
            status?.Report($"Backing up {name}...");
            CopyDirectory(source, destination, cancellationToken);
            categories.Add(name);
        }

        var manifest = new BackupManifest
        {
            SourceDriveRoot = drive.RootDirectory.FullName,
            CreatedUtc = DateTime.UtcNow,
            BackedUpCategories = categories
        };

        await File.WriteAllTextAsync(
            Path.Combine(targetDir, "backup_manifest.json"),
            JsonSerializer.Serialize(manifest, new JsonSerializerOptions { WriteIndented = true }),
            cancellationToken);

        return targetDir;
    }

    public void RestoreBackup(string backupDir, DriveInfo drive, IProgress<string>? status, CancellationToken cancellationToken)
    {
        foreach (string name in RootDirectoriesToBackup)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string source = Path.Combine(backupDir, name);
            if (!Directory.Exists(source))
                continue;

            status?.Report($"Restoring {name}...");
            string destination = Path.Combine(drive.RootDirectory.FullName, name);
            CopyDirectory(source, destination, cancellationToken);
        }
    }

    private static void CopyDirectory(string sourceDir, string destDir, CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(destDir);

        foreach (string file in Directory.GetFiles(sourceDir))
        {
            cancellationToken.ThrowIfCancellationRequested();
            string destFile = Path.Combine(destDir, Path.GetFileName(file));
            File.Copy(file, destFile, overwrite: true);
        }

        foreach (string dir in Directory.GetDirectories(sourceDir))
        {
            cancellationToken.ThrowIfCancellationRequested();
            string destSubDir = Path.Combine(destDir, Path.GetFileName(dir));
            CopyDirectory(dir, destSubDir, cancellationToken);
        }
    }
}
