using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EverDrive64SdRepair.Dialogs;
using EverDrive64SdRepair.Models;
using EverDrive64SdRepair.Services;

namespace EverDrive64SdRepair;

public partial class MainForm : Form
{
    private readonly DriveDetectionService _driveDetectionService = new();
    private readonly ScannerService _scannerService = new();
    private readonly BackupService _backupService = new();
    private readonly OsRepositoryService _osRepositoryService = new();
    private readonly SystemFileService _systemFileService = new();
    private readonly FormatterService _formatterService = new();
    private readonly AllocationSizeService _allocationSizeService = new();

    private IReadOnlyList<DriveInfo> _currentDrives = Array.Empty<DriveInfo>();
    private IReadOnlyList<OsVersionInfo> _currentVersions = Array.Empty<OsVersionInfo>();
    private string? _lastBackupFolder;
    private bool _busy;

    public MainForm()
    {
        InitializeComponent();
        ApplyDarkTheme();
        txtBackupPath.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "EverDrive64_Backups");
        Load += MainForm_Load;
    }

    private async void MainForm_Load(object? sender, EventArgs e)
    {
        try
        {
            await InitializeAsync();
        }
        catch (Exception ex)
        {
            Log("Startup failed: " + ex.Message);
            SetStatus("Startup failed.");
        }
    }

    private async Task InitializeAsync()
    {
        RefreshDrivesInternal();
        await RefreshOsVersionsInternalAsync();
        Log("Ready.");
        SetStatus("Ready");
    }

    private void ApplyDarkTheme()
    {
        BackColor = Color.FromArgb(32, 32, 32);
        ForeColor = Color.White;

        foreach (Control control in Controls)
        {
            ApplyDarkThemeRecursive(control);
        }

        menuStrip1.BackColor = Color.FromArgb(30, 30, 30);
        menuStrip1.ForeColor = Color.Gainsboro;
        statusStrip1.BackColor = Color.FromArgb(30, 30, 30);
        statusStrip1.ForeColor = Color.Gainsboro;
    }

    private void ApplyDarkThemeRecursive(Control control)
    {
        switch (control)
        {
            case GroupBox groupBox:
                groupBox.ForeColor = Color.Gainsboro;
                groupBox.BackColor = Color.Transparent;
                break;
            case Button button:
                button.BackColor = Color.FromArgb(70, 70, 70);
                button.ForeColor = Color.Gainsboro;
                button.FlatStyle = FlatStyle.Flat;
                break;
            case ComboBox comboBox:
                comboBox.BackColor = Color.FromArgb(40, 40, 40);
                comboBox.ForeColor = Color.Gainsboro;
                break;
            case TextBox textBox:
                textBox.BackColor = Color.FromArgb(40, 40, 40);
                textBox.ForeColor = Color.Gainsboro;
                textBox.BorderStyle = BorderStyle.FixedSingle;
                break;
            case RichTextBox richTextBox:
                richTextBox.BackColor = Color.FromArgb(24, 24, 24);
                richTextBox.ForeColor = Color.Gainsboro;
                richTextBox.BorderStyle = BorderStyle.FixedSingle;
                richTextBox.ReadOnly = true;
                break;
            case Label label:
                label.ForeColor = Color.Gainsboro;
                break;
        }

        foreach (Control child in control.Controls)
        {
            ApplyDarkThemeRecursive(child);
        }
    }

    private async Task RunBusyAsync(Func<Task> action)
    {
        if (_busy)
            return;

        try
        {
            _busy = true;
            SetButtonsEnabled(false);
            await action();
        }
        catch (Exception ex)
        {
            Log("Error: " + ex.Message);
            MessageBox.Show(this, ex.Message, "Operation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            SetStatus("Operation failed");
        }
        finally
        {
            _busy = false;
            SetButtonsEnabled(true);
        }
    }

    private void SetButtonsEnabled(bool enabled)
    {
        btnRefreshDrives.Enabled = enabled;
        btnScan.Enabled = enabled;
        btnRefreshVersions.Enabled = enabled;
        btnPickBackup.Enabled = enabled;
        btnBackup.Enabled = enabled;
        btnRestore.Enabled = enabled;
        btnFormat.Enabled = enabled;
        btnRepair.Enabled = enabled;
        btnFullRebuild.Enabled = enabled;
        btnFirstTimeSetup.Enabled = enabled;
        cmbDrives.Enabled = enabled;
        cmbOsVersion.Enabled = enabled;
    }

    private void RefreshDrivesInternal()
    {
        _currentDrives = _driveDetectionService.GetRemovableDrives();
        cmbDrives.Items.Clear();
        foreach (var drive in _currentDrives)
        {
            cmbDrives.Items.Add(_driveDetectionService.DescribeDrive(drive));
        }

        if (cmbDrives.Items.Count > 0)
        {
            cmbDrives.SelectedIndex = 0;
        }

        UpdateDriveDisplay();
        Log($"Refreshed drives. Found {cmbDrives.Items.Count} removable drive(s).");
    }

    private async Task RefreshOsVersionsInternalAsync()
    {
        SetStatus("Loading OS versions...");
        Log("Loading OS versions from krikzz.com...");
        _currentVersions = await _osRepositoryService.GetAvailableVersionsAsync(CancellationToken.None);
        cmbOsVersion.Items.Clear();
        foreach (var version in _currentVersions)
        {
            string label = string.IsNullOrWhiteSpace(version.SizeLabel)
                ? version.VersionLabel
                : $"{version.VersionLabel} - {version.SizeLabel}";
            cmbOsVersion.Items.Add(label);
        }

        if (cmbOsVersion.Items.Count > 0)
        {
            cmbOsVersion.SelectedIndex = 0;
        }

        Log($"Loaded {_currentVersions.Count} OS version(s).");
        SetStatus("OS versions loaded");
    }

    private DriveInfo? GetSelectedDrive(bool showMessage = true)
    {
        int index = cmbDrives.SelectedIndex;
        if (index < 0 || index >= _currentDrives.Count)
        {
            if (showMessage)
            {
                MessageBox.Show(this, "Select an SD card first.", "No SD Card Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return null;
        }

        return _currentDrives[index];
    }

    private OsVersionInfo? GetSelectedVersion(bool showMessage = true)
    {
        int index = cmbOsVersion.SelectedIndex;
        if (index < 0 || index >= _currentVersions.Count)
        {
            if (showMessage)
            {
                MessageBox.Show(this, "Select an OS version first.", "No OS Version Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return null;
        }

        return _currentVersions[index];
    }

    private void UpdateDriveDisplay()
    {
        var drive = GetSelectedDrive(false);
        if (drive == null)
        {
            lblDrive.Text = "Target drive:";
            btnFormat.Text = "Format SD Card";
            btnFirstTimeSetup.Text = "Run First-Time Setup";
            return;
        }

        int clusterKb = _allocationSizeService.GetBestClusterSizeKb(drive.TotalSize);
        lblDrive.Text = $"Target drive: {drive.Name} ({clusterKb} KB clusters)";
        btnFormat.Text = $"Format SD Card ({clusterKb} KB)";
        btnFirstTimeSetup.Text = $"Run First-Time Setup ({clusterKb} KB)";
    }

    private void Log(string message)
    {
        txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        txtLog.SelectionStart = txtLog.TextLength;
        txtLog.ScrollToCaret();
    }

    private void SetStatus(string text)
    {
        lblStatus.Text = text;
    }

    private async Task ScanAsync()
    {
        var drive = GetSelectedDrive();
        if (drive == null)
            return;

        Log($"Scanning {drive.Name}...");
        var progress = new Progress<ScanProgress>(p =>
        {
            SetStatus($"{p.Percent}% - {p.Activity}");
            if (!string.IsNullOrWhiteSpace(p.CurrentPath))
            {
                Log($"{p.Activity} {p.CurrentPath}");
            }
        });

        ScanResult result = await _scannerService.ScanAsync(drive, progress, CancellationToken.None);
        Log($"Scan summary: directories scanned={result.DirectoriesScanned}, files scanned={result.FilesScanned}, passed={result.PassedChecks}, failed={result.FailedChecks}, hashed={result.CrcFilesComputed}.");
        if (result.Issues.Count == 0)
        {
            Log("Scan completed. No issues found.");
        }
        else
        {
            Log(result.RequiresFullRebuild ? "Scan recommends full rebuild." : "Scan completed. Repair in place is possible.");
            foreach (var issue in result.Issues)
            {
                Log(issue.ToString());
            }
        }

        SetStatus("Scan complete");
    }

    private async Task BackupAsync()
    {
        var drive = GetSelectedDrive();
        if (drive == null)
            return;

        Directory.CreateDirectory(txtBackupPath.Text);
        var status = new Progress<string>(text =>
        {
            SetStatus(text);
            Log(text);
        });

        _lastBackupFolder = await _backupService.CreateBackupAsync(drive, txtBackupPath.Text, status, CancellationToken.None);
        Log("Backup created: " + _lastBackupFolder);
        SetStatus("Backup complete");
    }

    private async Task RestoreAsync()
    {
        var drive = GetSelectedDrive();
        if (drive == null)
            return;

        using var dialog = new FolderBrowserDialog { Description = "Select a backup folder to restore." };
        if (dialog.ShowDialog(this) != DialogResult.OK)
            return;

        var status = new Progress<string>(text =>
        {
            SetStatus(text);
            Log(text);
        });

        await Task.Run(() => _backupService.RestoreBackup(dialog.SelectedPath, drive, status, CancellationToken.None));
        Log("Restore completed from: " + dialog.SelectedPath);
        SetStatus("Restore complete");
    }

    private async Task RepairAsync()
    {
        var drive = GetSelectedDrive();
        var version = GetSelectedVersion();
        if (drive == null || version == null)
            return;

        string cacheDir = Path.Combine(txtBackupPath.Text, "Downloaded_OS");
        Directory.CreateDirectory(cacheDir);
        SetStatus("Downloading selected OS...");
        Log("Downloading " + version.VersionLabel + "...");
        string zipPath = await _osRepositoryService.DownloadSelectedVersionAsync(version, cacheDir, CancellationToken.None);

        var status = new Progress<string>(text =>
        {
            SetStatus(text);
            Log(text);
        });

        await _systemFileService.ApplyOsZipAsync(zipPath, drive, status, CancellationToken.None);
        Log($"Repair completed using {version.VersionLabel}.");
        SetStatus("Repair complete");
    }

    private async Task FormatOnlyAsync()
    {
        var drive = GetSelectedDrive();
        if (drive == null)
            return;

        int clusterKb = _allocationSizeService.GetBestClusterSizeKb(drive.TotalSize);
        if (MessageBox.Show(this, $"This will erase {drive.Name} and format it as FAT32 using {clusterKb} KB allocation size. Continue?", "Confirm Format", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        var status = new Progress<string>(text =>
        {
            SetStatus(text);
            Log(text);
        });

        await _formatterService.FormatAsync(drive, status, CancellationToken.None);
        SetStatus("Format complete");
        Log("Format complete.");
    }

    private async Task FullRebuildAsync()
    {
        var drive = GetSelectedDrive();
        var version = GetSelectedVersion();
        if (drive == null || version == null)
            return;

        int clusterKb = _allocationSizeService.GetBestClusterSizeKb(drive.TotalSize);
        if (MessageBox.Show(this, $"This will back up, format, reinstall {version.VersionLabel}, recreate OS folders, and restore user content on {drive.Name}. Continue?", "Confirm Full Rebuild", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        if (string.IsNullOrWhiteSpace(_lastBackupFolder) || !Directory.Exists(_lastBackupFolder))
        {
            await BackupAsync();
        }

        string cacheDir = Path.Combine(txtBackupPath.Text, "Downloaded_OS");
        Directory.CreateDirectory(cacheDir);
        string zipPath = await _osRepositoryService.DownloadSelectedVersionAsync(version, cacheDir, CancellationToken.None);

        var status = new Progress<string>(text =>
        {
            SetStatus(text);
            Log(text);
        });

        await _formatterService.FormatAsync(drive, status, CancellationToken.None);
        await _systemFileService.ApplyOsZipAsync(zipPath, drive, status, CancellationToken.None);
        _systemFileService.EnsureRecommendedFolders(drive, status);
        if (!string.IsNullOrWhiteSpace(_lastBackupFolder) && Directory.Exists(_lastBackupFolder))
        {
            _backupService.RestoreBackup(_lastBackupFolder, drive, status, CancellationToken.None);
        }

        Log($"Full rebuild completed on {drive.Name} using {version.VersionLabel}. Allocation size was {clusterKb} KB.");
        SetStatus("Full rebuild complete");
    }

    private async Task FirstTimeSetupAsync()
    {
        var drive = GetSelectedDrive();
        var version = GetSelectedVersion();
        if (drive == null || version == null)
            return;

        int clusterKb = _allocationSizeService.GetBestClusterSizeKb(drive.TotalSize);
        if (MessageBox.Show(this, $"This will erase and prepare {drive.Name} for first-time EverDrive use. The card will be formatted with {clusterKb} KB allocation size, {version.VersionLabel} will be installed, and starter folders will be created. Continue?", "Confirm First-Time Setup", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        string cacheDir = Path.Combine(txtBackupPath.Text, "Downloaded_OS");
        Directory.CreateDirectory(cacheDir);
        string zipPath = await _osRepositoryService.DownloadSelectedVersionAsync(version, cacheDir, CancellationToken.None);

        var status = new Progress<string>(text =>
        {
            SetStatus(text);
            Log(text);
        });

        await _formatterService.FormatAsync(drive, status, CancellationToken.None);
        await _systemFileService.ApplyOsZipAsync(zipPath, drive, status, CancellationToken.None);
        _systemFileService.EnsureRecommendedFolders(drive, status);
        Log($"First-time setup completed on {drive.Name} using {version.VersionLabel}. Allocation size was {clusterKb} KB.");
        SetStatus("First-time setup complete");
    }

    private async void btnRefreshDrives_Click(object? sender, EventArgs e)
    {
        await RunBusyAsync(() =>
        {
            RefreshDrivesInternal();
            SetStatus("Drives refreshed");
            return Task.CompletedTask;
        });
    }

    private async void btnScan_Click(object? sender, EventArgs e)
    {
        await RunBusyAsync(ScanAsync);
    }

    private async void btnPickBackup_Click(object? sender, EventArgs e)
    {
        await RunBusyAsync(() =>
        {
            using var dialog = new FolderBrowserDialog { Description = "Choose the backup root folder." };
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                txtBackupPath.Text = dialog.SelectedPath;
                Log("Backup root set to: " + dialog.SelectedPath);
            }
            return Task.CompletedTask;
        });
    }

    private async void btnBackup_Click(object? sender, EventArgs e)
    {
        await RunBusyAsync(BackupAsync);
    }

    private async void btnRestore_Click(object? sender, EventArgs e)
    {
        await RunBusyAsync(RestoreAsync);
    }

    private async void btnFormat_Click(object? sender, EventArgs e)
    {
        await RunBusyAsync(FormatOnlyAsync);
    }

    private async void btnRepair_Click(object? sender, EventArgs e)
    {
        await RunBusyAsync(RepairAsync);
    }

    private async void btnFullRebuild_Click(object? sender, EventArgs e)
    {
        await RunBusyAsync(FullRebuildAsync);
    }

    private async void btnFirstTimeSetup_Click(object? sender, EventArgs e)
    {
        await RunBusyAsync(FirstTimeSetupAsync);
    }

    private async void btnRefreshVersions_Click(object? sender, EventArgs e)
    {
        await RunBusyAsync(RefreshOsVersionsInternalAsync);
    }

    private void aboutToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        new AboutDialog().ShowDialog(this);
    }

    private void helpToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        new HelpDialog().ShowDialog(this);
    }

    private void cmbOsVersion_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
    }
}
