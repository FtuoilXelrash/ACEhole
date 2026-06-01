namespace ACEhole
{
    partial class AboutForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.aboutImage     = new System.Windows.Forms.PictureBox();
            this.titleLabel     = new System.Windows.Forms.Label();
            this.versionLabel   = new System.Windows.Forms.Label();
            this.buildDateLabel = new System.Windows.Forms.Label();
            this.authorLabel    = new System.Windows.Forms.Label();
            this.platformLabel  = new System.Windows.Forms.Label();
            this.descLabel      = new System.Windows.Forms.Label();
            this.storeLink      = new System.Windows.Forms.LinkLabel();
            this.closeButton    = new System.Windows.Forms.Button();
            this.separatorPanel = new System.Windows.Forms.Panel();

            ((System.ComponentModel.ISupportInitialize)(this.aboutImage)).BeginInit();
            this.SuspendLayout();

            // aboutImage
            this.aboutImage.Location  = new System.Drawing.Point(0, 0);
            this.aboutImage.Size      = new System.Drawing.Size(460, 90);
            this.aboutImage.SizeMode  = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.aboutImage.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            try
            {
                var stream = System.Reflection.Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("ACEhole.resources.images.ACEhole_about.png");
                if (stream != null) this.aboutImage.Image = System.Drawing.Image.FromStream(stream);
            }
            catch { }

            // titleLabel
            this.titleLabel.Text      = "ACEhole";
            this.titleLabel.Font      = new System.Drawing.Font("Segoe UI", 16f, System.Drawing.FontStyle.Bold);
            this.titleLabel.Location  = new System.Drawing.Point(20, 102);
            this.titleLabel.Size      = new System.Drawing.Size(420, 32);
            this.titleLabel.ForeColor = System.Drawing.Color.FromArgb(33, 33, 33);

            // descLabel
            this.descLabel.Text      = "ACE Asheron's Call Server Manager";
            this.descLabel.Font      = new System.Drawing.Font("Segoe UI", 9f);
            this.descLabel.Location  = new System.Drawing.Point(22, 136);
            this.descLabel.Size      = new System.Drawing.Size(420, 20);
            this.descLabel.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);

            // separatorPanel
            this.separatorPanel.BackColor = System.Drawing.Color.FromArgb(200, 200, 200);
            this.separatorPanel.Location  = new System.Drawing.Point(20, 162);
            this.separatorPanel.Size      = new System.Drawing.Size(420, 1);

            // versionLabel
            this.versionLabel.Text     = "Version 0.0.1";
            this.versionLabel.Font     = new System.Drawing.Font("Segoe UI", 9f);
            this.versionLabel.Location = new System.Drawing.Point(20, 172);
            this.versionLabel.Size     = new System.Drawing.Size(420, 20);

            // buildDateLabel
            this.buildDateLabel.Text     = "Built: —";
            this.buildDateLabel.Font     = new System.Drawing.Font("Segoe UI", 9f);
            this.buildDateLabel.Location = new System.Drawing.Point(20, 194);
            this.buildDateLabel.Size     = new System.Drawing.Size(420, 20);

            // authorLabel
            this.authorLabel.Text     = "Author: Ftuoil Xelrash";
            this.authorLabel.Font     = new System.Drawing.Font("Segoe UI", 9f);
            this.authorLabel.Location = new System.Drawing.Point(20, 216);
            this.authorLabel.Size     = new System.Drawing.Size(420, 20);

            // platformLabel
            this.platformLabel.Text      = "Platform:";
            this.platformLabel.Font      = new System.Drawing.Font("Segoe UI", 9f);
            this.platformLabel.Location  = new System.Drawing.Point(20, 238);
            this.platformLabel.Size      = new System.Drawing.Size(420, 20);
            this.platformLabel.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);

            // storeLink
            this.storeLink.Text      = "Microsoft Store Page";
            this.storeLink.Font      = new System.Drawing.Font("Segoe UI", 9f);
            this.storeLink.Location  = new System.Drawing.Point(20, 266);
            this.storeLink.Size      = new System.Drawing.Size(200, 20);
            this.storeLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.storeLink_LinkClicked);

            // closeButton
            this.closeButton.Text     = "Close";
            this.closeButton.Font     = new System.Drawing.Font("Segoe UI", 9f);
            this.closeButton.Location = new System.Drawing.Point(355, 300);
            this.closeButton.Size     = new System.Drawing.Size(85, 28);
            this.closeButton.Click   += new System.EventHandler(this.closeButton_Click);

            // AboutForm
            this.ClientSize      = new System.Drawing.Size(460, 346);
            this.Text            = "About ACEhole";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.StartPosition   = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Font            = new System.Drawing.Font("Segoe UI", 9f);

            this.Controls.Add(this.aboutImage);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.descLabel);
            this.Controls.Add(this.separatorPanel);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.buildDateLabel);
            this.Controls.Add(this.authorLabel);
            this.Controls.Add(this.platformLabel);
            this.Controls.Add(this.storeLink);
            this.Controls.Add(this.closeButton);

            ((System.ComponentModel.ISupportInitialize)(this.aboutImage)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.PictureBox aboutImage;
        private System.Windows.Forms.Label      titleLabel;
        private System.Windows.Forms.Label      descLabel;
        private System.Windows.Forms.Panel      separatorPanel;
        private System.Windows.Forms.Label      versionLabel;
        private System.Windows.Forms.Label      buildDateLabel;
        private System.Windows.Forms.Label      authorLabel;
        private System.Windows.Forms.Label      platformLabel;
        private System.Windows.Forms.LinkLabel  storeLink;
        private System.Windows.Forms.Button     closeButton;
    }
}
