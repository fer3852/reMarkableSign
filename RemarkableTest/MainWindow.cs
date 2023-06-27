using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Text.Json.Nodes;
using System.Collections.Specialized;
using System.Configuration;
using System.Threading;
using PdfiumViewer;
using System.Xml.Linq;

namespace RemarkableSign
{
    /// <summary>
    /// The main window.
    /// </summary>
	public partial class MainWindow : Form
    {
        string lastFilename = "";
        byte[] lastContents = new byte[0];
        FileSystemWatcher watcher = null;
        DataObject dragObject = null;
        DateTime uploadTime = DateTime.Now;
        PdfRenderer pdfRenderer;
        bool hasDownloaded = true;

		public MainWindow()
        {
			Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
			InitializeComponent();
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/octet-stream");
            }
            // Check if this is the first run and proceed accordingly.
            if (ConfigurationManager.AppSettings["monitorDirectory"] == "NotSet" ||
				ConfigurationManager.AppSettings["monitorDirectory"] == null)
            {
                if (!Directory.Exists("Signed Documents"))
                    Directory.CreateDirectory("Signed Documents");
				if (!Directory.Exists("Unsigned Documents"))
					Directory.CreateDirectory("Unsigned Documents");
				if (MessageBox.Show("It seems this is the first time you run Remarkable Sign. It's advisable to configure the app" +
                    " for first use, and set its monitoring directory. Proceed?", "First use", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    FormSettings settingsForm = new FormSettings();
                    settingsForm.ShowDialog();
					SetFileSystemWatcher(ConfigurationManager.AppSettings["monitorDirectory"]);
				} else
                {
                    SetFileSystemWatcher(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads");
                }

            } else {
                if (bool.Parse(ConfigurationManager.AppSettings["monitor"])) {
                    SetFileSystemWatcher(ConfigurationManager.AppSettings["monitorDirectory"]);
                } else {
                    SetFileSystemWatcher("");
				}
			}

		}

        /// <summary>
        /// Initialises watching a directory for file changes.
        /// </summary>
        /// <param name="folder">The directory to watch.</param>
        void SetFileSystemWatcher(string folder)
        {
            // First disable the previous file watcher, if there is one.
            if (watcher != null)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Changed -= OnCreated;
                watcher.Dispose();
            }
            watcher = null;

            // If there is no configured monitoring directory, exit.
            if (ConfigurationManager.AppSettings["monitor"] == null || !bool.Parse(ConfigurationManager.AppSettings["monitor"])) {
                labelMonitoring.Text = "Drag file onto window to upload to your reMarkable.";
                return;
            }

            // Initialise watcher
            watcher = new FileSystemWatcher(folder);
            watcher.IncludeSubdirectories = false;
            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;
            watcher.Changed += OnCreated;
            watcher.Filter = "*.pdf";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
            labelMonitoring.Text = "Monitoring " + folder + " for changes...";
        }

        /// <summary>
        /// Occurs when a file change is detected in the monitored directory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            // Get full path of the changed file.
            string filename = e.FullPath;

            // If filename is the same as the previous uploaded filename, it's 1 of 3 things:
            // - OnCreated was triggered multiple times (which may happen).
            // - The file was uploaded again, e.g. because a correction was made.
            // - External software (e.g. OneDrive) changed a file attribute.
            if (lastFilename == filename) {
                if ((DateTime.Now - uploadTime).TotalSeconds < 3.0) {
                    // Ensure a 3s delay between uploads to prevent multiple
                    // triggers of OnCreated from re-initalising the process.
                    return;
                } else {
                    // It can simply be a file attribute change, e.g. by Cloud
                    // software. In that case, the file contents are the same
                    // and we don't need to re-upload.
                    byte[] newContents = File.ReadAllBytes(filename);
                    // First of all, the length needs to match.
                    if (newContents.Length == lastContents.Length) {
                        // Every byte needs to match as well.
                        bool reUploadNeeded = false;
						for (int i = 0; i < newContents.Length; i++) {
                            // If a byte differs, the content has changed. Reupload.
							if (newContents[i] != lastContents[i]) {
                                reUploadNeeded = true;
								break;
							}
						}
                        if (!reUploadNeeded) {
                            // Files are identical. No need to reupload.
                            return;
                        }
					}
                }
            } 

            // Pop up application window.
            this.Invoke((Action)delegate { BringToFront(); });

            // Initialise the upload process.
			Upload(ConfigurationManager.AppSettings["remarkableIP"] + "/upload", filename);
            uploadTime = DateTime.Now;
        }

        /// <summary>
        /// Brings focus to the application.
        /// </summary>
        new void BringToFront() {
                this.WindowState = FormWindowState.Minimized;
                this.Show();
                this.WindowState = FormWindowState.Normal;
		}

        /// <summary>
        /// Occurs when the user starts dragging a .pdf file onto the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		void MainWindow_DragEnter(object sender, DragEventArgs e)
        {
            // Set the drag/drop icon.
			if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
		}

        /// <summary>
        /// Occurs when a user drags a .pdf file onto the window. Initialises the upload process.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		void MainWindow_DragDrop(object sender, DragEventArgs e) {
            // Check whether the file comes from our own application; ignore if
            // it does.
			if (e.Data.GetData("DragSource") != this) {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0) {
                    if (files[0].Substring(files[0].Length - 3) == "pdf") {
                        Upload(ConfigurationManager.AppSettings["remarkableIP"] + "/upload", files[0]);
                    } else {
                        MessageBox.Show("Error: only .pdf files supported!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }

        private void MainWindow_Load(object sender, EventArgs e) {

		}

        /// <summary>
        /// Gets an image representation of the first page of a .pdf document.
        /// </summary>
        /// <param name="filename">The filename of the pdf document to create an image for.</param>
        /// <param name="size">The resolution in pixels of the maximum dimension of the image.</param>
        /// <returns></returns>
        Image GetPdfImage(string filename, int size = 256) {
            try {
                PdfDocument pdfdoc = PdfDocument.Load(filename);
                SizeF s = pdfdoc.PageSizes[0];
                int width, height;
                if (s.Width > s.Height) {
                    width = size;
                    height = (int)(size * s.Height / ((float)s.Width));
                } else {
					height = size;
					width = (int)(size * s.Width / ((float)s.Height));
				}
				Image img = pdfdoc.Render(0, width, height, 1, 1, false);
				pdfdoc.Dispose();
                return img;
			} catch {
                // If for some reason we weren't able to get a valid bitmap, return the
                // default pdf icon embedded in the .exe.
				return new Bitmap(RemarkableSign.Properties.Resources.icon_pdf);
			}
		}
        
        /// <summary>
        /// Uploads a .pdf document to the reMarkable.
        /// </summary>
        /// <param name="actionUrl">The url of the reMarkable upload page, usually https://10.11.99.1/upload</param>
        /// <param name="filename">The filename of the .pdf file to upload.</param>
        /// <param name="forceContinue">Upload even if the last file wasn't downloaded yet, without throwing a warning.</param>
        private void Upload(string actionUrl, string filename, bool forceContinue=false)
        {
            // Check if we already downloaded the previous file.
            if (!hasDownloaded && !forceContinue) {
                // If the user hasn't downloaded the previous file, ask if he wants to
                // continue.
                if (bool.Parse(ConfigurationManager.AppSettings["autodelete"])) {
                    // We should delete the previously uploaded file.

                    if (MessageBox.Show("You didn't download the previously uploaded file " +
                        Path.GetFileName(filename) + " yet. Do you want to continue? " +
                        "WARNING: continuing will remove the previously uploaded file!",
                        "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) ==
                        DialogResult.Yes) {
                        // Obtain the id of the previous file.
                        string previousid;
                        try {
                            previousid = GetLastUploadID();
                        } catch {
							// We couldn't get the ID. Probably a connection error. Ask the user if he
							// wants to retry.
							if (MessageBox.Show("Error: couldn't establish a connection to the reMarkable. Make sure it's connected and switched on.\n\n" +
								"If the reMarkable still doesn't connect after waiting a few seconds, check on the reMarkable if " +
								"\"Settings -> Storage -> USB web interface\" is switched on.", "Error", MessageBoxButtons.RetryCancel,
								MessageBoxIcon.Error) == DialogResult.Retry) {
								Upload(actionUrl, filename, forceContinue);
								return;
							}
							return;
						}

						// First upload the new file (this because deleting a file results
						// in a nasty reset of the service on the reMarkable, whereas
						// uploading a file doesn't. Set forceContinue=true to prevent
                        // the same questions the next time round, and return.
						Upload(actionUrl, filename, true);

						// After uploading, delete the previous file.
						try {
							SshConnector.DeleteFile(previousid,
								ConfigurationManager.AppSettings["remarkableIP"],
								ConfigurationManager.AppSettings["username"],
								ConfigurationManager.AppSettings["password"]);
						} catch {
							// Delete failed. Just show a warning. 
							MessageBox.Show("Warning: couldn't automatically delete the " +
                                "previously uploaded file from the reMarkable! Remember to " +
                                "delete it manually to comply with data and privacy " +
                                "regulations!",
								"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						}
                        return;
					}
				} else {
					// No need to delete the previously uploaded file.
					if (MessageBox.Show("You didn't download the previously uploaded file " + Path.GetFileName(filename) +
					    " yet. Do you want to continue?", "Warning",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) ==
                        DialogResult.Yes) {
						// Upload the new file. Set forceContinue=true to prevent
						// the same questions the next time round, and return.
						Upload(actionUrl, filename, true);
                        return;
					}
				}
			}

            // Reset lastFilename to ensure we can retry immediately if anything should fail.
            lastFilename = "";
            lastContents = new byte[0];

            // Enable the upload button, disable the download button.
            this.Invoke((Action)delegate {
                buttonDrag.Enabled = false;
                buttonDrag.BackgroundImage = null;
				buttonDownload.Enabled = false;
            });
            
            // Store current time to measure timeouts later on.
            DateTime actionTime = DateTime.Now;

            byte[] data;
            try
            {
                // Read file data
                data = File.ReadAllBytes(filename);

			} catch (Exception ex)
            {
                // It probably fails because the file is locked (i.e. still being written by
                // the filesystem). Try again every 250ms, up to 3s.
                if ((DateTime.Now - actionTime).TotalSeconds < 3.0) {
                    Thread.Sleep(250);
					Upload(actionUrl, filename, true);
					return;
                } else {
                    // Something else. Show error and ask if the user wants to retry.
                    if (MessageBox.Show("Error: " + ex.Message + (ex.InnerException == null ? "" : "\n\n" + ex.InnerException.Message), "Error", MessageBoxButtons.RetryCancel,
                        MessageBoxIcon.Error) == DialogResult.Retry) {
                        Upload(actionUrl, filename, true);
                        return;
                    } else {
                        return;
                    }
                }
            }

            // Generate parameters of the HTTP post.
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
			postParameters.Add("filename", Path.GetFileName(filename));
            postParameters.Add("fileformat", "pdf");
            postParameters.Add("file", new FormUpload.FileParameter(data, Path.GetFileName(filename), "application/pdf"));

            // Create request and receive response
            string postURL = "http://" + ConfigurationManager.AppSettings["remarkableIP"] +
                "/upload";
            string userAgent = "Someone";
            HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(postURL, userAgent, postParameters, false,
				int.Parse(ConfigurationManager.AppSettings["timeout"]));

            // Check for valid response.
            if (webResponse == null || !(webResponse.StatusCode == HttpStatusCode.OK || webResponse.StatusCode == HttpStatusCode.Accepted || webResponse.StatusCode == HttpStatusCode.Created)) {
				// No successful response. Ask if the user wants to try again.

				if (MessageBox.Show("Error: couldn't establish a connection to the reMarkable. Make sure it's connected and switched on.\n\n" +
					"If the reMarkable still doesn't connect after waiting a few seconds, check on the reMarkable if " +
					"\"Settings -> Storage -> USB web interface\" is switched on.", "Error", MessageBoxButtons.RetryCancel,
					MessageBoxIcon.Error) == DialogResult.Retry) {
                    Upload(actionUrl, filename);
                    return;
                } else {
                    return;
                }
            }

            // Process response.
            StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
            string fullResponse = responseReader.ReadToEnd();
            webResponse.Close();

            // Now we have a confirmation that the file has uploaded, update lastFileName.
            lastFilename = filename;
			lastContents = File.ReadAllBytes(filename);

			// We just uploaded a new file, so hasDownloaded is now false.
			hasDownloaded = false;

            // Enable the download button (being GUI, it has to be invoked).
            this.Invoke((Action)delegate {
                buttonDownload.Enabled = true;
				labelMonitoring.Text = "Uploaded " + filename + "!";
			});
        }

        /// <summary>
        /// Automatically uploads the downloaded file to TopDesk (needs an API key)
        /// !!!! STILL WIP !!!!
        /// </summary>
        /// <param name="filename"></param>
        /// <exception cref="NullReferenceException"></exception>
        private void UploadToTopdesk(string filename)
        {
            // Check for the proper filename format.
            if (filename.Length < 12)//minimal filename C2303000.pdf
            {
                MessageBox.Show("Error: filename is not of the format \"CXXXXXXXX*.pdf\". Cannot upload automatically. Upload manually.",
                    "Error",MessageBoxButtons.OK, MessageBoxIcon.Error); return;
            }
            if (!Char.IsLetter(filename[0]))
            {
                MessageBox.Show("Error: filename is not of the format \"CXXXXXXXX*.pdf\". Cannot upload automatically. Upload manually.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
            }
            for (int i = 1; i < 8; i++)
            {
                if (!Char.IsNumber(filename[i]))
                {
                    MessageBox.Show("Error: filename is not of the format \"CXXXXXXXX*.pdf\". Cannot upload automatically. Upload manually.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
                }
            }
            int firstNonNumericChar = -1;
            for (int i = 1; i < filename.Length; i++)
            {
                if (!Char.IsNumber(filename[i]))
                {
                    firstNonNumericChar = i;
                    break;
                }
            }
            string changenumber = filename.Substring(0, firstNonNumericChar);

            //get id
            HttpWebRequest request = WebRequest.Create("https://tue-test.topdesk.net/tas/api/operatorChanges?query=number==" +
                changenumber) as HttpWebRequest;

            if (request == null)
            {
                throw new NullReferenceException("request is not a http request");
            }

            // Set up the request properties.
            request.Method = "GET";
            request.CookieContainer = new CookieContainer();

            //TEST FER TODO
            request.PreAuthenticate = true;
            var username = "apiusername";
            var password = "apipassword";
            string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                                           .GetBytes(username + ":" + password));
            request.Headers.Add("Authorization", "Basic " + encoded);


            string html;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            Console.WriteLine(html);
            JsonNode document = JsonNode.Parse(html);

            JsonNode root = document.Root;

            //TODO if list length not 0
            string changeId = root["results"][0]["id"].ToString();
            
            // Read file data
            byte[] data = File.ReadAllBytes(filename);

            // Generate post objects
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("filename", Path.GetFileName(filename));
            postParameters.Add("fileformat", "pdf");
            postParameters.Add("file", new FormUpload.FileParameter(data, Path.GetFileName(filename), "application/pdf"));

            // Create request and receive response
            string postURL = "https://tue-test.topdesk.net/tas/api/operatorChanges/" + changeId +"/attachments";
            string userAgent = "Someone";
            HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(postURL, userAgent, postParameters, true);

            // Process response
            StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
            string fullResponse = responseReader.ReadToEnd();
            webResponse.Close();

            lastFilename = filename;
            lastContents = File.ReadAllBytes(filename);

		}

        /// <summary>
        /// Gets the internal id of the last uploaded file. Needed to access the file on the reMarkable.
        /// </summary>
        /// <returns>A string representation of the internal id of the last modified file with a filename equal to this.lastFilename.</returns>
        /// <exception cref="NullReferenceException"></exception>
        private string GetLastUploadID() {
            string contents;

            // Initialise request.
            HttpWebRequest request = WebRequest.Create("http://" +
                ConfigurationManager.AppSettings["remarkableIP"] +
                "/documents/") as HttpWebRequest;

            if (request == null) {
                throw new NullReferenceException("request is not a http request");
            }
            // Set up the request properties.
            request.Method = "GET";
            request.CookieContainer = new CookieContainer();
            request.Timeout = int.Parse(ConfigurationManager.AppSettings["timeout"]);

            // Do the request. Note this may throw an exception, so exception handling
            // is required in the calling function!
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream)) {
                contents = reader.ReadToEnd();
            }

            // The response (should) consists of a JSON file. Parse it.
            JsonNode document = JsonNode.Parse(contents);

            // Store some values for easier usage.
            JsonNode root = document.Root;
            JsonArray docArray = root.AsArray();
            string fname = Path.GetFileName(this.lastFilename);
            string id = "";

            // In case of more files with the same (Windows-)filename, we need to pick the
            // most recent one. Initialise a value to check for this.
            DateTime lastModified = DateTime.MinValue;

            // The filename on the reMarkable sometimes doesn't include the extension,
            // for some reason (and sometimes it does).
            string fnameWithoutExt = fname.Substring(0, fname.Length - 4);

            // Find the latest-modified file with the correct filename.
            foreach (JsonNode node in docArray) {
                if (node["VissibleName"] is JsonNode fnNode) {
                    if (fnNode.ToString() == fnameWithoutExt ||
                        fnNode.ToString() == fname) {
                        if (node["ID"] is JsonNode idNode && node["ModifiedClient"] is JsonNode timeNode) {
                            DateTime dt = DateTime.Parse(timeNode.ToString());
                            if (DateTime.Compare(lastModified, dt) < 0) {//we've found a newer file with this filename
                                lastModified = dt;
                                id = idNode.ToString();
                            }
                        }
                    }
                }
            }

            // ID now contains the remarkable file ID that we need to download the file
            // (or an empty string if it couldn't be found).
            return id;
        }

        /// <summary>
        /// Occurs when the download button is clicked. Initialises the download process.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDownload_Click(object sender, EventArgs e) {
            string id;

            // Reset the application directory as if auto-started at logon, this would be
            // different than expected.
			Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            // Obtain the file ID the reMarkable uses.
			try {
                id = GetLastUploadID();
            } catch {
                // We couldn't get the ID. Probably a connection error. Ask the user if he
                // wants to retry.
				if (MessageBox.Show("Error: couldn't establish a connection to the reMarkable. Make sure it's connected and switched on.\n\n" +
					"If the reMarkable still doesn't connect after waiting a few seconds, check on the reMarkable if " +
					"\"Settings -> Storage -> USB web interface\" is switched on.", "Error", MessageBoxButtons.RetryCancel,
					MessageBoxIcon.Error) == DialogResult.Retry) {
					buttonDownload_Click(new object(), new EventArgs());
					return;
				}
				return;
			}

			// To store the downloaded file, retrieve its filename without path.
			string fname = Path.GetFileName(this.lastFilename);

			// Check if we successfully retrieved an ID.
			if (id != "") {
				using (var client = new WebClient()) {
                    // Try to download the file using the id we retrieved earlier.
                    try {
                        client.DownloadFile("http://" + ConfigurationManager.AppSettings["remarkableIP"] +
                            "/download/" + id + "/placeholder", "Signed Documents\\" + fname);
                    } catch (Exception ex) {
                        // Download failed. Ask if user wants to retry.
						if (MessageBox.Show("Error: couldn't download file from the reMarkable:\n" + ex.Message +
                            "\n" + ex.InnerException.Message, "Error", MessageBoxButtons.RetryCancel,
							MessageBoxIcon.Error) == DialogResult.Retry) {
							buttonDownload_Click(sender, e);
							return;
						}
					}

                    // Initialize the wrapper used to drag from the application.
					dragObject = new System.Windows.Forms.DataObject();
					StringCollection filePaths = new StringCollection();
                    string dlFileName = Directory.GetCurrentDirectory() +
                        "\\Signed Documents\\" + fname;
					filePaths.Add(dlFileName);
					dragObject.SetFileDropList(filePaths);
                    // Set the DragSource attribute, which we need to determine whether
                    // it's a new file we dragged or not.
					dragObject.SetData("DragSource", this);

					// Enable the drag button and set its icon, disable the Download button.
					buttonDrag.Enabled = true;
                    buttonDrag.BackgroundImage= GetPdfImage(dlFileName,buttonDrag.Height - 16);
                    buttonDownload.Enabled = false;
                    hasDownloaded = true;
                    labelMonitoring.Text = "Downloaded " + fname + "!";

                    // Delete downloaded file from the reMarkable, if configured.
					if (bool.Parse(ConfigurationManager.AppSettings["autodelete"])) {
                        //Read downloaded file to ensure deleting it from the ReMarkable is OK.
                        try {
                            byte[] data = File.ReadAllBytes("Signed Documents\\" + fname);
                            if (data.Length > 0) {
                                // There's data. Assume all the HTTP error checking/correcting
                                // have done their job and the download is correct (pretty
                                // safe bet - no need to parse the pdf). Delete the file from
                                // the reMarkable.
                                try {
                                    SshConnector.DeleteFile(id,
                                        ConfigurationManager.AppSettings["remarkableIP"],
                                        ConfigurationManager.AppSettings["username"],
                                        ConfigurationManager.AppSettings["password"]);
								} catch {
									// Delete failed. Just show a warning. 
									MessageBox.Show("Warning: couldn't automatically delete file " +
										fname + " from ReMarkable! Remember to delete it manually to " +
										"comply with data and privacy regulations!",
										"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
								}
							} else {
                                throw new Exception();
							}
                        } catch {
							// Reading downloaded file failed. Ask if user wants to retry.
							if (MessageBox.Show("Error: couldn't read downloaded file! Check" +
								"if download was successful!", "Error",
								MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) ==
								DialogResult.Retry) {
								buttonDownload_Click(sender, e);
							} else {
								MessageBox.Show("Warning: didn't automatically delete " +
									"file " + fname + " from ReMarkable! Remember to " +
									"delete it manually to comply with data and " +
									"privacy regulations!", "Warning",
									MessageBoxButtons.OK, MessageBoxIcon.Warning);
							}
							return;
						}
                    }
				}
            } else {
                //No file with the same filename as we uploaded!
                MessageBox.Show("Error: couldn't find file " + fname + 
                    "! Download manually through a browser (default http://10.11.99.1).",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

            return;
        }

        /// <summary>
        /// Occurs when the user starts dragging the downloaded pdf file. Initialises the dragging process.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void buttonDrag_MouseDown(object sender, EventArgs e) {
            if (dragObject != null) {
                DoDragDrop(dragObject, DragDropEffects.Copy);
            }
            
		}

        /// <summary>
        /// Occurs when the user presses the settings button. Opens the settings window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSettings_Click(object sender, EventArgs e)
        {
			FormSettings settingsForm = new FormSettings();
            settingsForm.ShowDialog();
            SetFileSystemWatcher(ConfigurationManager.AppSettings["monitorDirectory"]);

		}

        /// <summary>
        /// Shows a message box with some basic help about how to upload and
        /// download files to/from the reMarkable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void buttonHelp_Click(object sender, EventArgs e) {
			MessageBox.Show("To upload a file to the reMarkable, you can either " +
                "drag-and-drop the file onto this window, or save it inside " +
                "the following directory:\n" + ConfigurationManager.
                AppSettings["monitorDirectory"] + "\nYou can change this directory " +
                "in the settings.\nNote that only .pdf files " +
                "are supported.\n\nOnce uploaded to the reMarkable, the button " +
                "on the left will become available. You can now sign the file. " +
                "Once finished, click the left button to download it. The downloaded " +
                "file is automatically saved in the \"Signed Documents\" folder in " +
                "the installation directory of reMarkable Sign.\n\nThe button " +
                "on the right now shows a preview of the .pdf file. Drag-and-drop " +
                "the file straight from the right button into the application you " +
                "want to upload it.", "Help",
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	}
}
