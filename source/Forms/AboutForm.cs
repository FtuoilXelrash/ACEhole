using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace ACEhole
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();

            var ver   = Assembly.GetExecutingAssembly().GetName().Version;
            var built = System.IO.File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location);

            versionLabel.Text   = $"Version {ver.Major}.{ver.Minor}.{ver.Build}";
            buildDateLabel.Text = $"Built: {built:yyyy-MM-dd HH:mm}";
            authorLabel.Text    = "Author: Ftuoil Xelrash";
            platformLabel.Text  = $"Platform: {Environment.OSVersion.VersionString}  |  .NET {Environment.Version}";
        }

        private void storeLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try { Process.Start(new ProcessStartInfo("ms-windows-store://pdp/?productid=TODO") { UseShellExecute = true }); }
            catch { }
        }

        private void closeButton_Click(object sender, EventArgs e) => Close();
    }
}
