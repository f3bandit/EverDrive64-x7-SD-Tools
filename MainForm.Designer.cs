namespace EverDrive64SdRepair;

partial class MainForm
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        menuStrip1 = new MenuStrip();
        helpToolStripMenuItem = new ToolStripMenuItem();
        aboutToolStripMenuItem = new ToolStripMenuItem();
        grpSdCard = new GroupBox();
        btnRefreshDrives = new Button();
        btnScan = new Button();
        cmbDrives = new ComboBox();
        lblDrive = new Label();
        grpOsVersion = new GroupBox();
        btnRefreshVersions = new Button();
        cmbOsVersion = new ComboBox();
        lblOsVersion = new Label();
        grpBackup = new GroupBox();
        btnRestore = new Button();
        btnBackup = new Button();
        btnPickBackup = new Button();
        txtBackupPath = new TextBox();
        grpActions = new GroupBox();
        btnFullRebuild = new Button();
        btnRepair = new Button();
        btnFormat = new Button();
        grpFirstTimeSetup = new GroupBox();
        btnFirstTimeSetup = new Button();
        lblSetupInfo = new Label();
        txtLog = new RichTextBox();
        statusStrip1 = new StatusStrip();
        lblStatus = new ToolStripStatusLabel();
        menuStrip1.SuspendLayout();
        grpSdCard.SuspendLayout();
        grpOsVersion.SuspendLayout();
        grpBackup.SuspendLayout();
        grpActions.SuspendLayout();
        grpFirstTimeSetup.SuspendLayout();
        statusStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // menuStrip1
        // 
        menuStrip1.Items.AddRange(new ToolStripItem[] { helpToolStripMenuItem, aboutToolStripMenuItem });
        menuStrip1.Location = new Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new Size(994, 24);
        menuStrip1.TabIndex = 0;
        // 
        // helpToolStripMenuItem
        // 
        helpToolStripMenuItem.Name = "helpToolStripMenuItem";
        helpToolStripMenuItem.Size = new Size(44, 20);
        helpToolStripMenuItem.Text = "Help";
        helpToolStripMenuItem.Click += helpToolStripMenuItem_Click;
        // 
        // aboutToolStripMenuItem
        // 
        aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
        aboutToolStripMenuItem.Size = new Size(52, 20);
        aboutToolStripMenuItem.Text = "About";
        aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
        // 
        // grpSdCard
        // 
        grpSdCard.Controls.Add(btnRefreshDrives);
        grpSdCard.Controls.Add(btnScan);
        grpSdCard.Controls.Add(cmbDrives);
        grpSdCard.Controls.Add(lblDrive);
        grpSdCard.ForeColor = Color.White;
        grpSdCard.Location = new Point(12, 36);
        grpSdCard.Name = "grpSdCard";
        grpSdCard.Size = new Size(255, 150);
        grpSdCard.TabIndex = 1;
        grpSdCard.TabStop = false;
        grpSdCard.Text = "SD Card";
        // 
        // btnRefreshDrives
        // 
        btnRefreshDrives.BackColor = Color.FromArgb(70, 70, 70);
        btnRefreshDrives.FlatStyle = FlatStyle.Flat;
        btnRefreshDrives.Location = new Point(18, 81);
        btnRefreshDrives.Name = "btnRefreshDrives";
        btnRefreshDrives.Size = new Size(221, 25);
        btnRefreshDrives.TabIndex = 3;
        btnRefreshDrives.Text = "Refresh Drives";
        btnRefreshDrives.UseVisualStyleBackColor = false;
        btnRefreshDrives.Click += btnRefreshDrives_Click;
        // 
        // btnScan
        // 
        btnScan.BackColor = Color.FromArgb(70, 70, 70);
        btnScan.FlatStyle = FlatStyle.Flat;
        btnScan.Location = new Point(18, 112);
        btnScan.Name = "btnScan";
        btnScan.Size = new Size(221, 25);
        btnScan.TabIndex = 2;
        btnScan.Text = "Scan Card";
        btnScan.UseVisualStyleBackColor = false;
        btnScan.Click += btnScan_Click;
        // 
        // cmbDrives
        // 
        cmbDrives.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbDrives.FormattingEnabled = true;
        cmbDrives.Items.AddRange(new object[] { "E:\\", "F:\\", "G:\\" });
        cmbDrives.Location = new Point(18, 52);
        cmbDrives.Name = "cmbDrives";
        cmbDrives.Size = new Size(221, 23);
        cmbDrives.TabIndex = 1;
        // 
        // lblDrive
        // 
        lblDrive.AutoSize = true;
        lblDrive.Location = new Point(18, 30);
        lblDrive.Name = "lblDrive";
        lblDrive.Size = new Size(72, 15);
        lblDrive.TabIndex = 0;
        lblDrive.Text = "Target drive:";
        // 
        // grpOsVersion
        // 
        grpOsVersion.Controls.Add(btnRefreshVersions);
        grpOsVersion.Controls.Add(cmbOsVersion);
        grpOsVersion.Controls.Add(lblOsVersion);
        grpOsVersion.ForeColor = Color.White;
        grpOsVersion.Location = new Point(279, 36);
        grpOsVersion.Name = "grpOsVersion";
        grpOsVersion.Size = new Size(230, 150);
        grpOsVersion.TabIndex = 2;
        grpOsVersion.TabStop = false;
        grpOsVersion.Text = "OS Version";
        // 
        // btnRefreshVersions
        // 
        btnRefreshVersions.BackColor = Color.FromArgb(70, 70, 70);
        btnRefreshVersions.FlatStyle = FlatStyle.Flat;
        btnRefreshVersions.Location = new Point(15, 91);
        btnRefreshVersions.Name = "btnRefreshVersions";
        btnRefreshVersions.Size = new Size(198, 25);
        btnRefreshVersions.TabIndex = 2;
        btnRefreshVersions.Text = "Refresh Versions";
        btnRefreshVersions.UseVisualStyleBackColor = false;
        btnRefreshVersions.Click += btnRefreshVersions_Click;
        // 
        // cmbOsVersion
        // 
        cmbOsVersion.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbOsVersion.FormattingEnabled = true;
        cmbOsVersion.Location = new Point(15, 52);
        cmbOsVersion.Name = "cmbOsVersion";
        cmbOsVersion.Size = new Size(198, 23);
        cmbOsVersion.TabIndex = 1;
        // 
        // lblOsVersion
        // 
        lblOsVersion.AutoSize = true;
        lblOsVersion.Location = new Point(15, 30);
        lblOsVersion.Name = "lblOsVersion";
        lblOsVersion.Size = new Size(82, 15);
        lblOsVersion.TabIndex = 0;
        lblOsVersion.Text = "Select version:";
        // 
        // grpBackup
        // 
        grpBackup.Controls.Add(btnRestore);
        grpBackup.Controls.Add(btnBackup);
        grpBackup.Controls.Add(btnPickBackup);
        grpBackup.Controls.Add(txtBackupPath);
        grpBackup.ForeColor = Color.White;
        grpBackup.Location = new Point(521, 36);
        grpBackup.Name = "grpBackup";
        grpBackup.Size = new Size(230, 150);
        grpBackup.TabIndex = 3;
        grpBackup.TabStop = false;
        grpBackup.Text = "Backup";
        // 
        // btnRestore
        // 
        btnRestore.BackColor = Color.FromArgb(70, 70, 70);
        btnRestore.FlatStyle = FlatStyle.Flat;
        btnRestore.Location = new Point(10, 114);
        btnRestore.Name = "btnRestore";
        btnRestore.Size = new Size(210, 25);
        btnRestore.TabIndex = 3;
        btnRestore.Text = "Restore";
        btnRestore.UseVisualStyleBackColor = false;
        btnRestore.Click += btnRestore_Click;
        // 
        // btnBackup
        // 
        btnBackup.BackColor = Color.FromArgb(70, 70, 70);
        btnBackup.FlatStyle = FlatStyle.Flat;
        btnBackup.Location = new Point(10, 83);
        btnBackup.Name = "btnBackup";
        btnBackup.Size = new Size(210, 25);
        btnBackup.TabIndex = 2;
        btnBackup.Text = "Backup";
        btnBackup.UseVisualStyleBackColor = false;
        btnBackup.Click += btnBackup_Click;
        // 
        // btnPickBackup
        // 
        btnPickBackup.BackColor = Color.FromArgb(70, 70, 70);
        btnPickBackup.FlatStyle = FlatStyle.Flat;
        btnPickBackup.Location = new Point(10, 52);
        btnPickBackup.Name = "btnPickBackup";
        btnPickBackup.Size = new Size(210, 25);
        btnPickBackup.TabIndex = 1;
        btnPickBackup.Text = "Choose Backup Folder";
        btnPickBackup.UseVisualStyleBackColor = false;
        btnPickBackup.Click += btnPickBackup_Click;
        // 
        // txtBackupPath
        // 
        txtBackupPath.Location = new Point(10, 22);
        txtBackupPath.Name = "txtBackupPath";
        txtBackupPath.Size = new Size(210, 23);
        txtBackupPath.TabIndex = 0;
        // 
        // grpActions
        // 
        grpActions.Controls.Add(btnFullRebuild);
        grpActions.Controls.Add(btnRepair);
        grpActions.Controls.Add(btnFormat);
        grpActions.ForeColor = Color.White;
        grpActions.Location = new Point(763, 36);
        grpActions.Name = "grpActions";
        grpActions.Size = new Size(220, 150);
        grpActions.TabIndex = 4;
        grpActions.TabStop = false;
        grpActions.Text = "Actions";
        // 
        // btnFullRebuild
        // 
        btnFullRebuild.BackColor = Color.FromArgb(70, 70, 70);
        btnFullRebuild.FlatStyle = FlatStyle.Flat;
        btnFullRebuild.Location = new Point(12, 89);
        btnFullRebuild.Name = "btnFullRebuild";
        btnFullRebuild.Size = new Size(196, 25);
        btnFullRebuild.TabIndex = 2;
        btnFullRebuild.Text = "Full Rebuild";
        btnFullRebuild.UseVisualStyleBackColor = false;
        btnFullRebuild.Click += btnFullRebuild_Click;
        // 
        // btnRepair
        // 
        btnRepair.BackColor = Color.FromArgb(70, 70, 70);
        btnRepair.FlatStyle = FlatStyle.Flat;
        btnRepair.Location = new Point(12, 58);
        btnRepair.Name = "btnRepair";
        btnRepair.Size = new Size(196, 25);
        btnRepair.TabIndex = 1;
        btnRepair.Text = "Repair In Place";
        btnRepair.UseVisualStyleBackColor = false;
        btnRepair.Click += btnRepair_Click;
        // 
        // btnFormat
        // 
        btnFormat.BackColor = Color.FromArgb(70, 70, 70);
        btnFormat.FlatStyle = FlatStyle.Flat;
        btnFormat.Location = new Point(12, 27);
        btnFormat.Name = "btnFormat";
        btnFormat.Size = new Size(196, 25);
        btnFormat.TabIndex = 0;
        btnFormat.Text = "Format SD Card";
        btnFormat.UseVisualStyleBackColor = false;
        btnFormat.Click += btnFormat_Click;
        // 
        // grpFirstTimeSetup
        // 
        grpFirstTimeSetup.Controls.Add(btnFirstTimeSetup);
        grpFirstTimeSetup.Controls.Add(lblSetupInfo);
        grpFirstTimeSetup.ForeColor = Color.White;
        grpFirstTimeSetup.Location = new Point(763, 198);
        grpFirstTimeSetup.Name = "grpFirstTimeSetup";
        grpFirstTimeSetup.Size = new Size(220, 143);
        grpFirstTimeSetup.TabIndex = 5;
        grpFirstTimeSetup.TabStop = false;
        grpFirstTimeSetup.Text = "SD Card First-Time Setup";
        // 
        // btnFirstTimeSetup
        // 
        btnFirstTimeSetup.BackColor = Color.FromArgb(70, 70, 70);
        btnFirstTimeSetup.FlatStyle = FlatStyle.Flat;
        btnFirstTimeSetup.Location = new Point(12, 109);
        btnFirstTimeSetup.Name = "btnFirstTimeSetup";
        btnFirstTimeSetup.Size = new Size(196, 25);
        btnFirstTimeSetup.TabIndex = 1;
        btnFirstTimeSetup.Text = "Run First-Time Setup";
        btnFirstTimeSetup.UseVisualStyleBackColor = false;
        btnFirstTimeSetup.Click += btnFirstTimeSetup_Click;
        // 
        // lblSetupInfo
        // 
        lblSetupInfo.Location = new Point(12, 27);
        lblSetupInfo.Name = "lblSetupInfo";
        lblSetupInfo.Size = new Size(196, 70);
        lblSetupInfo.TabIndex = 0;
        lblSetupInfo.Text = "Formats the card, installs the selected OS, and creates starter folders for ROMs, cheats, themes, and system data.";
        // 
        // txtLog
        // 
        txtLog.BackColor = Color.FromArgb(24, 24, 24);
        txtLog.ForeColor = Color.White;
        txtLog.Location = new Point(12, 198);
        txtLog.Name = "txtLog";
        txtLog.Size = new Size(739, 220);
        txtLog.TabIndex = 6;
        txtLog.Text = "";
        // 
        // statusStrip1
        // 
        statusStrip1.Items.AddRange(new ToolStripItem[] { lblStatus });
        statusStrip1.Location = new Point(0, 428);
        statusStrip1.Name = "statusStrip1";
        statusStrip1.Size = new Size(994, 22);
        statusStrip1.TabIndex = 7;
        // 
        // lblStatus
        // 
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(39, 17);
        lblStatus.Text = "Ready";
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(32, 32, 32);
        ClientSize = new Size(994, 450);
        Controls.Add(statusStrip1);
        Controls.Add(txtLog);
        Controls.Add(grpFirstTimeSetup);
        Controls.Add(grpActions);
        Controls.Add(grpBackup);
        Controls.Add(grpOsVersion);
        Controls.Add(grpSdCard);
        Controls.Add(menuStrip1);
        ForeColor = Color.White;
        MainMenuStrip = menuStrip1;
        MinimumSize = new Size(1010, 489);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "EverDrive64 SD Repair";
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        grpSdCard.ResumeLayout(false);
        grpSdCard.PerformLayout();
        grpOsVersion.ResumeLayout(false);
        grpOsVersion.PerformLayout();
        grpBackup.ResumeLayout(false);
        grpBackup.PerformLayout();
        grpActions.ResumeLayout(false);
        grpFirstTimeSetup.ResumeLayout(false);
        statusStrip1.ResumeLayout(false);
        statusStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private MenuStrip menuStrip1;
    private ToolStripMenuItem helpToolStripMenuItem;
    private ToolStripMenuItem aboutToolStripMenuItem;
    private GroupBox grpSdCard;
    private Button btnRefreshDrives;
    private Button btnScan;
    private ComboBox cmbDrives;
    private Label lblDrive;
    private GroupBox grpOsVersion;
    private Button btnRefreshVersions;
    private ComboBox cmbOsVersion;
    private Label lblOsVersion;
    private GroupBox grpBackup;
    private Button btnRestore;
    private Button btnBackup;
    private Button btnPickBackup;
    private TextBox txtBackupPath;
    private GroupBox grpActions;
    private Button btnFullRebuild;
    private Button btnRepair;
    private Button btnFormat;
    private GroupBox grpFirstTimeSetup;
    private Button btnFirstTimeSetup;
    private Label lblSetupInfo;
    private RichTextBox txtLog;
    private StatusStrip statusStrip1;
    private ToolStripStatusLabel lblStatus;
}
