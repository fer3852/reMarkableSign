namespace RemarkableSign
{
    partial class FormSettings
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
			this.buttonApplyClose = new System.Windows.Forms.Button();
			this.textBoxFileSystem = new System.Windows.Forms.TextBox();
			this.textBoxRemarkableIP = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.checkBoxAutodelete = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textBoxUsername = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textBoxPassword = new System.Windows.Forms.TextBox();
			this.checkBoxMonitor = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.buttonBrowse = new System.Windows.Forms.Button();
			this.folderBrowserDialogMonitorDirectory = new System.Windows.Forms.FolderBrowserDialog();
			this.checkBoxStartup = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.numericUpDownTimeout = new System.Windows.Forms.NumericUpDown();
			this.buttonDeleteFiles = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeout)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonApplyClose
			// 
			this.buttonApplyClose.Location = new System.Drawing.Point(3, 205);
			this.buttonApplyClose.Margin = new System.Windows.Forms.Padding(4);
			this.buttonApplyClose.Name = "buttonApplyClose";
			this.buttonApplyClose.Size = new System.Drawing.Size(606, 28);
			this.buttonApplyClose.TabIndex = 9;
			this.buttonApplyClose.Text = "Apply and close";
			this.buttonApplyClose.UseVisualStyleBackColor = true;
			this.buttonApplyClose.Click += new System.EventHandler(this.buttonApplyClose_Click);
			// 
			// textBoxFileSystem
			// 
			this.textBoxFileSystem.Location = new System.Drawing.Point(141, 67);
			this.textBoxFileSystem.Margin = new System.Windows.Forms.Padding(4);
			this.textBoxFileSystem.Name = "textBoxFileSystem";
			this.textBoxFileSystem.Size = new System.Drawing.Size(385, 22);
			this.textBoxFileSystem.TabIndex = 2;
			// 
			// textBoxRemarkableIP
			// 
			this.textBoxRemarkableIP.Location = new System.Drawing.Point(197, 99);
			this.textBoxRemarkableIP.Name = "textBoxRemarkableIP";
			this.textBoxRemarkableIP.Size = new System.Drawing.Size(224, 22);
			this.textBoxRemarkableIP.TabIndex = 3;
			this.textBoxRemarkableIP.Text = "10.11.99.1";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 102);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(187, 16);
			this.label2.TabIndex = 8;
			this.label2.Text = "ReMarkable network address:";
			// 
			// checkBoxAutodelete
			// 
			this.checkBoxAutodelete.AutoSize = true;
			this.checkBoxAutodelete.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBoxAutodelete.Checked = true;
			this.checkBoxAutodelete.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxAutodelete.Location = new System.Drawing.Point(3, 124);
			this.checkBoxAutodelete.Name = "checkBoxAutodelete";
			this.checkBoxAutodelete.Size = new System.Drawing.Size(353, 20);
			this.checkBoxAutodelete.TabIndex = 5;
			this.checkBoxAutodelete.Text = "Automatically delete downloaded files from reMarkable";
			this.checkBoxAutodelete.UseVisualStyleBackColor = true;
			this.checkBoxAutodelete.CheckedChanged += new System.EventHandler(this.checkBoxAutodelete_CheckedChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 153);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(148, 16);
			this.label3.TabIndex = 10;
			this.label3.Text = "ReMarkable username:";
			// 
			// textBoxUsername
			// 
			this.textBoxUsername.Location = new System.Drawing.Point(158, 150);
			this.textBoxUsername.Name = "textBoxUsername";
			this.textBoxUsername.Size = new System.Drawing.Size(142, 22);
			this.textBoxUsername.TabIndex = 6;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(307, 153);
			this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(70, 16);
			this.label4.TabIndex = 12;
			this.label4.Text = "Password:";
			// 
			// textBoxPassword
			// 
			this.textBoxPassword.Location = new System.Drawing.Point(384, 150);
			this.textBoxPassword.Name = "textBoxPassword";
			this.textBoxPassword.Size = new System.Drawing.Size(225, 22);
			this.textBoxPassword.TabIndex = 7;
			// 
			// checkBoxMonitor
			// 
			this.checkBoxMonitor.AutoSize = true;
			this.checkBoxMonitor.Checked = true;
			this.checkBoxMonitor.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxMonitor.Location = new System.Drawing.Point(6, 69);
			this.checkBoxMonitor.Name = "checkBoxMonitor";
			this.checkBoxMonitor.Size = new System.Drawing.Size(128, 20);
			this.checkBoxMonitor.TabIndex = 1;
			this.checkBoxMonitor.Text = "Monitor directory:";
			this.checkBoxMonitor.UseVisualStyleBackColor = true;
			this.checkBoxMonitor.CheckedChanged += new System.EventHandler(this.checkBoxMonitor_CheckedChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(3, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(603, 57);
			this.label1.TabIndex = 15;
			this.label1.Text = resources.GetString("label1.Text");
			// 
			// buttonBrowse
			// 
			this.buttonBrowse.Location = new System.Drawing.Point(534, 66);
			this.buttonBrowse.Name = "buttonBrowse";
			this.buttonBrowse.Size = new System.Drawing.Size(75, 23);
			this.buttonBrowse.TabIndex = 16;
			this.buttonBrowse.Text = "Browse...";
			this.buttonBrowse.UseVisualStyleBackColor = true;
			this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
			// 
			// folderBrowserDialogMonitorDirectory
			// 
			this.folderBrowserDialogMonitorDirectory.RootFolder = System.Environment.SpecialFolder.MyComputer;
			this.folderBrowserDialogMonitorDirectory.HelpRequest += new System.EventHandler(this.folderBrowserDialogMonitorDirectory_HelpRequest);
			// 
			// checkBoxStartup
			// 
			this.checkBoxStartup.AutoSize = true;
			this.checkBoxStartup.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBoxStartup.Location = new System.Drawing.Point(3, 179);
			this.checkBoxStartup.Name = "checkBoxStartup";
			this.checkBoxStartup.Size = new System.Drawing.Size(163, 20);
			this.checkBoxStartup.TabIndex = 8;
			this.checkBoxStartup.Text = "Run at Windows Logon";
			this.checkBoxStartup.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(179, 180);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(177, 16);
			this.label5.TabIndex = 18;
			this.label5.Text = "(Note: may require a reboot.)";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(427, 102);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(88, 16);
			this.label6.TabIndex = 19;
			this.label6.Text = "Timeout (ms):";
			// 
			// numericUpDownTimeout
			// 
			this.numericUpDownTimeout.Increment = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.numericUpDownTimeout.Location = new System.Drawing.Point(524, 100);
			this.numericUpDownTimeout.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
			this.numericUpDownTimeout.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.numericUpDownTimeout.Name = "numericUpDownTimeout";
			this.numericUpDownTimeout.Size = new System.Drawing.Size(85, 22);
			this.numericUpDownTimeout.TabIndex = 4;
			this.numericUpDownTimeout.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			// 
			// buttonDeleteFiles
			// 
			this.buttonDeleteFiles.Location = new System.Drawing.Point(384, 124);
			this.buttonDeleteFiles.Name = "buttonDeleteFiles";
			this.buttonDeleteFiles.Size = new System.Drawing.Size(225, 23);
			this.buttonDeleteFiles.TabIndex = 20;
			this.buttonDeleteFiles.Text = "Delete all files";
			this.buttonDeleteFiles.UseVisualStyleBackColor = true;
			this.buttonDeleteFiles.Click += new System.EventHandler(this.buttonDeleteFiles_Click);
			// 
			// FormSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(619, 237);
			this.Controls.Add(this.buttonDeleteFiles);
			this.Controls.Add(this.numericUpDownTimeout);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.checkBoxStartup);
			this.Controls.Add(this.buttonBrowse);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBoxPassword);
			this.Controls.Add(this.textBoxUsername);
			this.Controls.Add(this.textBoxRemarkableIP);
			this.Controls.Add(this.textBoxFileSystem);
			this.Controls.Add(this.buttonApplyClose);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.checkBoxMonitor);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.checkBoxAutodelete);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label2);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormSettings";
			this.ShowInTaskbar = false;
			this.Text = "Remarkable Sign Settings";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.FormSettings_Load);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeout)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonApplyClose;
        private System.Windows.Forms.TextBox textBoxFileSystem;
		private System.Windows.Forms.TextBox textBoxRemarkableIP;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox checkBoxAutodelete;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBoxUsername;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBoxPassword;
		private System.Windows.Forms.CheckBox checkBoxMonitor;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonBrowse;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogMonitorDirectory;
		private System.Windows.Forms.CheckBox checkBoxStartup;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.NumericUpDown numericUpDownTimeout;
		private System.Windows.Forms.Button buttonDeleteFiles;
	}
}