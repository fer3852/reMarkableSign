using System.Drawing;
using System.Windows.Forms;

namespace RemarkableSign
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
			this.buttonDownload = new System.Windows.Forms.Button();
			this.labelMonitoring = new System.Windows.Forms.Label();
			this.buttonDrag = new System.Windows.Forms.Button();
			this.buttonSettings = new System.Windows.Forms.Button();
			this.buttonHelp = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// buttonDownload
			// 
			this.buttonDownload.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.buttonDownload.Enabled = false;
			this.buttonDownload.Location = new System.Drawing.Point(16, 39);
			this.buttonDownload.Margin = new System.Windows.Forms.Padding(4);
			this.buttonDownload.Name = "buttonDownload";
			this.buttonDownload.Size = new System.Drawing.Size(341, 315);
			this.buttonDownload.TabIndex = 0;
			this.buttonDownload.Text = "Download signed document";
			this.buttonDownload.UseVisualStyleBackColor = true;
			this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
			// 
			// labelMonitoring
			// 
			this.labelMonitoring.AutoEllipsis = true;
			this.labelMonitoring.Location = new System.Drawing.Point(16, 11);
			this.labelMonitoring.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.labelMonitoring.Name = "labelMonitoring";
			this.labelMonitoring.Size = new System.Drawing.Size(617, 22);
			this.labelMonitoring.TabIndex = 1;
			this.labelMonitoring.Text = "Monitoring: ";
			// 
			// buttonDrag
			// 
			this.buttonDrag.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.buttonDrag.Enabled = false;
			this.buttonDrag.Location = new System.Drawing.Point(364, 39);
			this.buttonDrag.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.buttonDrag.Name = "buttonDrag";
			this.buttonDrag.Size = new System.Drawing.Size(344, 315);
			this.buttonDrag.TabIndex = 5;
			this.buttonDrag.Text = "Drag signed file from here...";
			this.buttonDrag.UseVisualStyleBackColor = true;
			this.buttonDrag.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonDrag_MouseDown);
			// 
			// buttonSettings
			// 
			this.buttonSettings.BackgroundImage = global::RemarkableSign.Properties.Resources.Gear_icon__1_;
			this.buttonSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.buttonSettings.Location = new System.Drawing.Point(677, 5);
			this.buttonSettings.Margin = new System.Windows.Forms.Padding(4);
			this.buttonSettings.Name = "buttonSettings";
			this.buttonSettings.Size = new System.Drawing.Size(28, 28);
			this.buttonSettings.TabIndex = 6;
			this.buttonSettings.UseVisualStyleBackColor = true;
			this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
			// 
			// buttonHelp
			// 
			this.buttonHelp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.buttonHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.139131F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonHelp.Location = new System.Drawing.Point(641, 5);
			this.buttonHelp.Margin = new System.Windows.Forms.Padding(4);
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.Size = new System.Drawing.Size(28, 28);
			this.buttonHelp.TabIndex = 7;
			this.buttonHelp.Text = "?";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
			// 
			// MainWindow
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(720, 361);
			this.Controls.Add(this.buttonHelp);
			this.Controls.Add(this.buttonSettings);
			this.Controls.Add(this.labelMonitoring);
			this.Controls.Add(this.buttonDownload);
			this.Controls.Add(this.buttonDrag);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainWindow";
			this.Text = "reMarkable Sign";
			this.Load += new System.EventHandler(this.MainWindow_Load);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainWindow_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainWindow_DragEnter);
			this.ResumeLayout(false);

        }

        #endregion

        private Button buttonDownload;
        private Label labelMonitoring;
		private Button buttonDrag;
        private Button buttonSettings;
		private Button buttonHelp;
	}
}

