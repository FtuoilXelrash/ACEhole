namespace ACEhole
{
    partial class LogViewerForm
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

            this.toolbarPanel    = new System.Windows.Forms.Panel();
            this.logTypeLabel    = new System.Windows.Forms.Label();
            this.logTypeCombo    = new System.Windows.Forms.ComboBox();
            this.prevButton      = new System.Windows.Forms.Button();
            this.dateLabel       = new System.Windows.Forms.Label();
            this.nextButton      = new System.Windows.Forms.Button();
            this.saveLogButton   = new System.Windows.Forms.Button();
            this.deleteLogButton = new System.Windows.Forms.Button();
            this.logRTB          = new System.Windows.Forms.RichTextBox();

            this.toolbarPanel.SuspendLayout();
            this.SuspendLayout();

            // toolbarPanel
            this.toolbarPanel.Dock    = System.Windows.Forms.DockStyle.Top;
            this.toolbarPanel.Height  = 36;
            this.toolbarPanel.Padding = new System.Windows.Forms.Padding(4, 4, 4, 0);

            // logTypeLabel
            this.logTypeLabel.Text     = "Log Type:";
            this.logTypeLabel.Location = new System.Drawing.Point(6, 8);
            this.logTypeLabel.AutoSize = true;

            // logTypeCombo
            this.logTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.logTypeCombo.Items.AddRange(new object[] { "Application", "Console" });
            this.logTypeCombo.Location = new System.Drawing.Point(70, 5);
            this.logTypeCombo.Size     = new System.Drawing.Size(120, 24);
            this.logTypeCombo.SelectedIndexChanged += new System.EventHandler(this.logTypeCombo_SelectedIndexChanged);

            // prevButton
            this.prevButton.Text     = "◄";
            this.prevButton.Location = new System.Drawing.Point(200, 5);
            this.prevButton.Size     = new System.Drawing.Size(30, 24);
            this.prevButton.Click   += new System.EventHandler(this.prevButton_Click);

            // dateLabel
            this.dateLabel.Text     = "";
            this.dateLabel.Location = new System.Drawing.Point(234, 8);
            this.dateLabel.Size     = new System.Drawing.Size(220, 20);
            this.dateLabel.Font     = new System.Drawing.Font("Consolas", 9f);

            // nextButton
            this.nextButton.Text     = "►";
            this.nextButton.Location = new System.Drawing.Point(458, 5);
            this.nextButton.Size     = new System.Drawing.Size(30, 24);
            this.nextButton.Click   += new System.EventHandler(this.nextButton_Click);

            // saveLogButton
            this.saveLogButton.Text     = "Save Log";
            this.saveLogButton.Location = new System.Drawing.Point(500, 5);
            this.saveLogButton.Size     = new System.Drawing.Size(75, 24);
            this.saveLogButton.Click   += new System.EventHandler(this.saveLogButton_Click);

            // deleteLogButton
            this.deleteLogButton.Text     = "Delete Log";
            this.deleteLogButton.Location = new System.Drawing.Point(580, 5);
            this.deleteLogButton.Size     = new System.Drawing.Size(80, 24);
            this.deleteLogButton.Click   += new System.EventHandler(this.deleteLogButton_Click);

            this.toolbarPanel.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.logTypeLabel, this.logTypeCombo, this.prevButton, this.dateLabel,
                this.nextButton, this.saveLogButton, this.deleteLogButton });

            // logRTB
            this.logRTB.Dock        = System.Windows.Forms.DockStyle.Fill;
            this.logRTB.BackColor   = System.Drawing.Color.FromArgb(30, 30, 30);
            this.logRTB.ForeColor   = System.Drawing.Color.FromArgb(212, 212, 212);
            this.logRTB.Font        = new System.Drawing.Font("Consolas", 9f);
            this.logRTB.ReadOnly    = true;
            this.logRTB.ScrollBars  = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.logRTB.WordWrap    = false;

            // LogViewerForm
            this.ClientSize    = new System.Drawing.Size(800, 560);
            this.Text          = "ACEhole - Log Viewer";
            this.Font          = new System.Drawing.Font("Segoe UI", 9f);
            this.Shown        += new System.EventHandler(this.LogViewerForm_Shown);
            this.FormClosing  += new System.Windows.Forms.FormClosingEventHandler(this.LogViewerForm_FormClosing);

            this.Controls.Add(this.logRTB);
            this.Controls.Add(this.toolbarPanel);

            this.toolbarPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel       toolbarPanel;
        private System.Windows.Forms.Label       logTypeLabel;
        private System.Windows.Forms.ComboBox    logTypeCombo;
        private System.Windows.Forms.Button      prevButton;
        private System.Windows.Forms.Label       dateLabel;
        private System.Windows.Forms.Button      nextButton;
        private System.Windows.Forms.Button      saveLogButton;
        private System.Windows.Forms.Button      deleteLogButton;
        private System.Windows.Forms.RichTextBox logRTB;
    }
}
