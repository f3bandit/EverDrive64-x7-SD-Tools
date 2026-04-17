using System.Text.RegularExpressions;
using EverDrive64SdRepair.Models;

namespace EverDrive64SdRepair.Services;

public sealed class OsRepositoryService
{
    private static readonly Uri BaseUri = new("https://krikzz.com/pub/support/everdrive-64/x-series/OS/");
    private static readonly Regex LinkRegex = new(
        @"<a\s+href=""(?<href>OS-V[^""]+\.zip)"">(?<name>OS-V[^<]+\.zip)</a>\s*(?<date>\d{2}-[A-Za-z]{3}-\d{4}\s+\d{2}:\d{2})?\s*(?<size>[\d\.]+[KMG]?)?",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public async Task<IReadOnlyList<OsVersionInfo>> GetAvailableVersionsAsync(CancellationToken cancellationToken)
    {
        using var client = new HttpClient();
        string html = await client.GetStringAsync(BaseUri, cancellationToken);
        var results = new List<OsVersionInfo>();

        foreach (Match match in LinkRegex.Matches(html))
        {
            string fileName = match.Groups["name"].Value;
            string version = Path.GetFileNameWithoutExtension(fileName);
            DateTime? modified = null;
            if (DateTime.TryParse(match.Groups["date"].Value, out DateTime parsed))
            {
                modified = parsed.ToUniversalTime();
            }

            results.Add(new OsVersionInfo
            {
                VersionLabel = version,
                DownloadUri = new Uri(BaseUri, fileName),
                LastModifiedUtc = modified,
                SizeLabel = string.IsNullOrWhiteSpace(match.Groups["size"].Value) ? "Unknown" : match.Groups["size"].Value
            });
        }

        return results
            .OrderByDescending(v => NormalizeVersion(v.VersionLabel))
            .ToList();
    }

    public async Task<string> DownloadSelectedVersionAsync(OsVersionInfo version, string downloadRoot, CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(downloadRoot);
        string targetFile = Path.Combine(downloadRoot, Path.GetFileName(version.DownloadUri.LocalPath));
        using var client = new HttpClient();
        using var response = await client.GetAsync(version.DownloadUri, cancellationToken);
        response.EnsureSuccessStatusCode();
        await using var input = await response.Content.ReadAsStreamAsync(cancellationToken);
        await using var output = File.Create(targetFile);
        await input.CopyToAsync(output, cancellationToken);
        return targetFile;
    }

    private static Version NormalizeVersion(string value)
    {
        string cleaned = value.Replace("OS-V", string.Empty, StringComparison.OrdinalIgnoreCase);
        return Version.TryParse(cleaned, out Version? version) ? version : new Version(0, 0);
    }
}
