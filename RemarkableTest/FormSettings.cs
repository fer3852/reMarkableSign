using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;
using System.IO;
using Microsoft.Win32;

namespace RemarkableSign
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["monitorDirectory"] == "NotSet"||
				ConfigurationManager.AppSettings["monitorDirectory"] == null)
            {
                textBoxFileSystem.Text = Directory.GetCurrentDirectory() + @"\Unsigned Documents";
				textBoxRemarkableIP.Text = "10.11.99.1";
				textBoxUsername.Text = "root";
				textBoxPassword.Text = "**********";
				checkBoxAutodelete.Checked = true;
				checkBoxMonitor.Checked = true;
				checkBoxStartup.Checked = false;
				numericUpDownTimeout.Value = 3000;
			} else {
                textBoxFileSystem.Text = ConfigurationManager.AppSettings["monitorDirectory"];
				textBoxRemarkableIP.Text = ConfigurationManager.AppSettings["remarkableIP"];
				textBoxUsername.Text = ConfigurationManager.AppSettings["username"];
				textBoxPassword.Text = ConfigurationManager.AppSettings["password"];
				checkBoxAutodelete.Checked = bool.Parse(ConfigurationManager.AppSettings["autodelete"]);
				checkBoxMonitor.Checked = bool.Parse(ConfigurationManager.AppSettings["monitor"]);
				RegistryKey rkApp = Registry.CurrentUser.OpenSubKey(
				@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
				checkBoxStartup.Checked = rkApp.GetValue("reMarkable Sign") != null;
				numericUpDownTimeout.Value = int.Parse(ConfigurationManager.AppSettings["timeout"]);
			}
        }

        private void buttonApplyClose_Click(object sender, EventArgs e)
        {
			try {
				var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
				var settings = configFile.AppSettings.Settings;
				//See Settings->Help->Copyrights and licenses
				if (checkBoxAutodelete.Checked && textBoxPassword.Text== "**********") {
					MessageBox.Show("ReMarkable Sign is configured to automatically" +
						" delete documents from the reMarkable after retrieving them, but the" +
						" ssh password is set to its default value **********. Either set" +
						" the password or disable automatic removal.\n\nYou can" +
						" find the password on your reMarkable:" +
						" Settings -> Help -> Copyrights and licenses", "Error", MessageBoxButtons.OK,
						MessageBoxIcon.Exclamation);
					return;
				}

				if (settings["monitor"] == null) {
					settings.Add("monitor", checkBoxMonitor.Checked.ToString());
				} else {
					settings["monitor"].Value = checkBoxMonitor.Checked.ToString();
				}
				if (settings["monitorDirectory"] == null) {
					settings.Add("monitorDirectory", textBoxFileSystem.Text);
				} else {
					settings["monitorDirectory"].Value = textBoxFileSystem.Text;
				}
				if (settings["remarkableIP"] == null) {
					settings.Add("remarkableIP", textBoxRemarkableIP.Text);
				} else {
					settings["remarkableIP"].Value = textBoxRemarkableIP.Text;
				}
				if (settings["username"] == null) {
					settings.Add("username", textBoxUsername.Text);
				} else {
					settings["username"].Value = textBoxUsername.Text;
				}
				if (settings["password"] == null) {
					settings.Add("password", textBoxPassword.Text);
				} else {
					settings["password"].Value = textBoxPassword.Text;
				}
				if (settings["autodelete"] == null) {
					settings.Add("autodelete", checkBoxAutodelete.Checked.ToString());
				} else {
					settings["autodelete"].Value = checkBoxAutodelete.Checked.ToString();
				}
				if (settings["timeout"] == null) {
					settings.Add("timeout", numericUpDownTimeout.Value.ToString());
				} else {
					settings["timeout"].Value = numericUpDownTimeout.Value.ToString();
				}
				if (checkBoxStartup.Checked) {
					RegistryKey rkApp = Registry.CurrentUser.OpenSubKey(
					@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

					//Path to launch shortcut
					string startPath = Directory.GetCurrentDirectory() + "\\reMarkableSign.exe";

					rkApp.SetValue("reMarkable Sign", startPath);
				} else {
					RegistryKey rkApp = Registry.CurrentUser.OpenSubKey(
					@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

					//Path to launch shortcut
					string startPath = Directory.GetCurrentDirectory() + "\\reMarkableSign.exe";
					if (rkApp.GetValue("reMarkable Sign") != null) {
						rkApp.DeleteValue("reMarkable Sign");
					}
				}
				configFile.Save(ConfigurationSaveMode.Modified);
				ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
			} catch (ConfigurationErrorsException) {
				Console.WriteLine("Error writing app settings");
			}
			Close();
        }

		private void checkBoxAutodelete_CheckedChanged(object sender, EventArgs e) {
			if (checkBoxAutodelete.Checked == true) {
				textBoxUsername.Enabled = true;
				textBoxPassword.Enabled = true;
			} else {
				textBoxUsername.Enabled = false;
				textBoxPassword.Enabled = false;
			}
		}

		private void folderBrowserDialogMonitorDirectory_HelpRequest(object sender, EventArgs e) {

		}

		private void buttonBrowse_Click(object sender, EventArgs e) {
			if (folderBrowserDialogMonitorDirectory.ShowDialog() == DialogResult.OK) {
				textBoxFileSystem.Text = folderBrowserDialogMonitorDirectory.SelectedPath;
			}
		}

		private void checkBoxMonitor_CheckedChanged(object sender, EventArgs e) {
			if (checkBoxMonitor.Checked) {
				textBoxFileSystem.Enabled = true;
				buttonBrowse.Enabled = true;
			} else {
				textBoxFileSystem.Enabled = false;
				buttonBrowse.Enabled = false;
			}
		}

		private void buttonDeleteFiles_Click(object sender, EventArgs e) {
			SshConnector.DeleteAllFiles(
				textBoxRemarkableIP.Text,
				textBoxUsername.Text,
				textBoxPassword.Text);
		}
	}
}
