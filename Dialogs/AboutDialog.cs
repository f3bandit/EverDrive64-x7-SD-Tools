namespace EverDrive64SdRepair.Dialogs;

public sealed class AboutDialog : Form
{
    public AboutDialog()
    {
        Text = "About";
        Width = 500;
        Height = 260;
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Color.FromArgb(30, 30, 30);
        ForeColor = Color.Gainsboro;

        var box = new TextBox
        {
            Multiline = true,
            ReadOnly = true,
            Dock = DockStyle.Fill,
            BorderStyle = BorderStyle.None,
            BackColor = BackColor,
            ForeColor = ForeColor,
            Text = "EverDrive64 SD Repair\r\n\r\nAuthor: f3bandit\r\n\r\nA dark-mode Windows utility for backing up, scanning, rebuilding, formatting, and restoring EverDrive 64 SD cards. Includes bundled fat32format integration and official Krikzz OS version selection."
        };

        Controls.Add(box);
    }
}
