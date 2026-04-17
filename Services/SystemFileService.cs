using System.IO.Compression;

namespace EverDrive64SdRepair.Services;

public sealed class SystemFileService
{
    public async Task ApplyOsZipAsync(string zipPath, DriveInfo drive, IProgress<string>? status, CancellationToken cancellationToken)
    {
        if (!File.Exists(zipPath))
            throw new FileNotFoundException("Selected OS zip was not found.", zipPath);

        string tempExtract = Path.Combine(Path.GetTempPath(), "EverDrive64SdRepair", Path.GetFileNameWithoutExtension(zipPath) + "_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempExtract);

        try
        {
            ZipFile.ExtractToDirectory(zipPath, tempExtract);
            foreach (string directory in Directory.GetDirectories(tempExtract))
            {
                cancellationToken.ThrowIfCancellationRequested();
                string name = Path.GetFileName(directory);
                status?.Report($"Restoring system directory {name}...");
                CopyDirectory(directory, Path.Combine(drive.RootDirectory.FullName, name), cancellationToken);
            }

            foreach (string file in Directory.GetFiles(tempExtract))
            {
                cancellationToken.ThrowIfCancellationRequested();
                string fileName = Path.GetFileName(file);
                if (string.Equals(fileName, "settings.ini", StringComparison.OrdinalIgnoreCase) && File.Exists(Path.Combine(drive.RootDirectory.FullName, fileName)))
                    continue;
                status?.Report($"Restoring system file {fileName}...");
                File.Copy(file, Path.Combine(drive.RootDirectory.FullName, fileName), overwrite: true);
            }
        }
        finally
        {
            if (Directory.Exists(tempExtract))
                Directory.Delete(tempExtract, recursive: true);
        }

        await Task.CompletedTask;
    }

    public void EnsureRecommendedFolders(DriveInfo drive, IProgress<string>? status)
    {
        string root = drive.RootDirectory.FullName;

        string[] folders = new[]
        {
            Path.Combine(root, "N64"),
            Path.Combine(root, "TOOLS"),
            Path.Combine(root, "THEMES"),
            Path.Combine(root, "ED64"),
            Path.Combine(root, "ED64", "cheats"),
            Path.Combine(root, "ED64", "emu"),
            Path.Combine(root, "ED64", "patcher"),
            Path.Combine(root, "ED64", "save"),
            Path.Combine(root, "ED64", "sys")
        };

        foreach (string folder in folders)
        {
            Directory.CreateDirectory(folder);
            status?.Report($"Ensured folder {folder}...");
        }
    }

    public string[] GetRequiredSystemItems() => new[] { "ED64" };

    private static void CopyDirectory(string sourceDir, string destinationDir, CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(destinationDir);
        foreach (string file in Directory.GetFiles(sourceDir))
        {
            cancellationToken.ThrowIfCancellationRequested();
            string fileName = Path.GetFileName(file);
            if (string.Equals(fileName, "settings.ini", StringComparison.OrdinalIgnoreCase) && File.Exists(Path.Combine(destinationDir, fileName)))
                continue;
            File.Copy(file, Path.Combine(destinationDir, fileName), overwrite: true);
        }

        foreach (string dir in Directory.GetDirectories(sourceDir))
        {
            cancellationToken.ThrowIfCancellationRequested();
            CopyDirectory(dir, Path.Combine(destinationDir, Path.GetFileName(dir)), cancellationToken);
        }
    }
}
