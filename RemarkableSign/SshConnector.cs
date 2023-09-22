using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Renci.SshNet;
using System.Threading;

namespace RemarkableSign
{
	internal static class SshConnector
	{
		/// <summary>
		/// Deletes all files from the reMarkable.
		/// </summary>
		/// <param name="ip">The IP address of the reMarkable.</param>
		/// <param name="usr">The username to use.</param>
		/// <param name="pwd">The password of the provided username.</param>
		public static void DeleteAllFiles(string ip, string usr, string pwd) {
			try {
				using (var client = new SshClient(ip, usr, pwd)) {
					// Connect to the client.
					client.Connect();

					// Simply remove all files in the main folder.
					SshCommand commandRm = client.RunCommand("rm -r /home/root/.local/share/remarkable/xochitl/*");
					commandRm = client.RunCommand("systemctl restart xochitl");

					// We can disconnect now.
					client.Disconnect();
				}
			} catch {
				// Something went wrong. Likely we used wrong credentials.
				MessageBox.Show("Error: couldn't establish ssh connection to " + ip +
					". Check stored login credentials in the settings.",
					"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// Delete a single file from the reMarkable.
		/// </summary>
		/// <param name="id">The file ID as used by the reMarkable.</param>
		/// <param name="ip">The IP address of the reMarkable.</param>
		/// <param name="usr">The username to use.</param>
		/// <param name="pwd">The password of the provided username.</param>
		public static void DeleteFile(string id, string ip, string usr, string pwd) {
			// Deleting files can take a while because the whole reMarkable GUI thread has
			// to restart (who at reMarkable came up with the horrible idea to combine GUI
			// and functionality?!?). Therefore use a separate thread.
			Thread t = new Thread(new ThreadStart(delegate () {
				try {
					using (var client = new SshClient(ip, usr, pwd)) {
						// Connect to the client.
						client.Connect();

						// There are a couple different files, all filenames of which are
						// based on the file ID. Delete all these files.
						SshCommand commandRm = client.RunCommand("rm \"/home/root/.local/share/remarkable/xochitl/" + id + ".pdf\"");
						commandRm = client.RunCommand("rm \"/home/root/.local/share/remarkable/xochitl/" + id + ".pagedata\"");
						commandRm = client.RunCommand("rm \"/home/root/.local/share/remarkable/xochitl/" + id + ".metadata\"");
						commandRm = client.RunCommand("rm \"/home/root/.local/share/remarkable/xochitl/" + id + ".local\"");
						commandRm = client.RunCommand("rm \"/home/root/.local/share/remarkable/xochitl/" + id + ".content\"");
						
						// Also remove the following directories.
						commandRm = client.RunCommand("rm -r \"/home/root/.local/share/remarkable/xochitl/" + id + "\"");
						commandRm = client.RunCommand("rm -r \"/home/root/.local/share/remarkable/xochitl/" + id + ".thumbnails\"");

						// The reMarkable keeps a "tombstone" file of all removed files.
						// Create one as well (not sure what this is used for).
						commandRm = client.RunCommand("echo `date` >> \"/home/root/.local/share/remarkable/xochitl/" + id + ".tombstone\"");
						commandRm = client.RunCommand("systemctl restart xochitl");

						// We can disconnect now.
						client.Disconnect();
					}
				} catch (Exception ex) {
					// Something went wrong. Likely we used wrong credentials.
					MessageBox.Show("Error: couldn't establish ssh connection to 10.11.99.1! " +
						"Check stored login credentials in the settings.\n\nError details: " + ex.Message,
						"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					// We need to show a warning here for privacy issues etc.
					MessageBox.Show("Warning: couldn't automatically delete the " +
						"previously uploaded file from the reMarkable! Remember to " +
						"delete it manually to comply with data and privacy regulations!",
						"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
			}));
			t.Start();
		}
	}
}
