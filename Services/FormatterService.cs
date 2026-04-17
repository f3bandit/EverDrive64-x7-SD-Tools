using System.Diagnostics;
using System.Security.Principal;

namespace EverDrive64SdRepair.Services;

public sealed class FormatterService
{
    private readonly AllocationSizeService _allocationSizeService = new();

    public async Task FormatAsync(DriveInfo drive, IProgress<string>? status, CancellationToken cancellationToken)
    {
        if (!drive.IsReady)
            throw new InvalidOperationException($"The selected drive {drive.Name} is not ready.");

        if (!IsRunningAsAdministrator())
            throw new InvalidOperationException("Formatting requires running the app as Administrator.");

        string exePath = Path.Combine(AppContext.BaseDirectory, "Tools", "fat32format.exe");
        if (!File.Exists(exePath))
            exePath = Path.Combine(AppContext.BaseDirectory, "fat32format.exe");

        if (!File.Exists(exePath))
            throw new FileNotFoundException("Bundled fat32format.exe was not found.", exePath);

        int clusterKb = _allocationSizeService.GetBestClusterSizeKb(drive.TotalSize);
        string clusterArg = _allocationSizeService.GetBestFormatterClusterArgument(drive.TotalSize);
        string driveArg = drive.Name.TrimEnd('\\');
        string args = $"-c{clusterArg} {driveArg}";

        status?.Report($"Formatter path: {exePath}");
        status?.Report($"Formatter arguments: {args}");
        status?.Report($"Selected drive size: {drive.TotalSize} bytes");
        status?.Report($"Expected allocation size: {clusterKb} KB");
        status?.Report("Launching fat32format...");

        var startInfo = new ProcessStartInfo
        {
            FileName = exePath,
            Arguments = args,
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            WorkingDirectory = AppContext.BaseDirectory
        };

        using var process = new Process { StartInfo = startInfo };
        process.Start();

        status?.Report("Sending confirmation to formatter...");
        await process.StandardInput.WriteLineAsync("y");
        await process.StandardInput.FlushAsync();
        process.StandardInput.Close();

        string stdout = await process.StandardOutput.ReadToEndAsync();
        string stderr = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync(cancellationToken);

        ReportLines(stdout, "fat32format", status);
        ReportLines(stderr, "fat32format stderr", status);

        if (process.ExitCode != 0)
            throw new InvalidOperationException($"fat32format failed with code {process.ExitCode}.");

        status?.Report("Format completed.");
    }

    private static void ReportLines(string text, string prefix, IProgress<string>? status)
    {
        if (string.IsNullOrWhiteSpace(text) || status == null)
            return;

        string[] lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines)
            status.Report($"{prefix}: {line}");
    }

    private static bool IsRunningAsAdministrator()
    {
        using WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }
}
