namespace EverDrive64SdRepair.Dialogs;

public sealed class HelpDialog : Form
{
    public HelpDialog()
    {
        Text = "Help";
        Width = 760;
        Height = 540;
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Color.FromArgb(30, 30, 30);
        ForeColor = Color.Gainsboro;

        var box = new RichTextBox
        {
            ReadOnly = true,
            Dock = DockStyle.Fill,
            BorderStyle = BorderStyle.None,
            BackColor = BackColor,
            ForeColor = ForeColor,
            Text =
                "Instructions\n\n" +
                "1. Refresh drives and select the SD card.\n" +
                "2. Refresh OS versions and choose the version you want to use.\n" +
                "3. Run Scan to inspect partition and system state.\n" +
                "4. Use Backup before formatting or rebuilding.\n" +
                "5. Use Repair to reinstall system files while keeping existing settings when possible.\n" +
                "6. Use Full Rebuild when partition issues are detected and you need backup, format, OS restore, and user-data restore.\n" +
                "7. Use SD Card First-Time Setup for a new card. It formats the card, installs the selected OS, and creates starter folders.\n\n" +
                "Troubleshooting\n\n" +
                "- If formatting fails, run the app as Administrator.\n" +
                "- If no OS versions load, check internet access to krikzz.com.\n" +
                "- If the card is not listed, reinsert it and refresh drives.\n" +
                "- If restore fails, verify the backup folder still contains the backed-up ROM, theme, cheat, save, and ED64 content.\n" +
                "- ROMs should be stored outside the ED64 system folder.\n"
        };

        Controls.Add(box);
    }
}
