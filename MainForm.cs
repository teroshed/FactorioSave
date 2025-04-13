using FactorioSave.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FactorioSave
{
    public partial class MainForm : Form
    {

        


        // Reference to our Factorio monitoring service
        public readonly FactorioMonitor _factorioMonitor;
        private ApplicationSettings _appSettings;
        private readonly GoogleDriveService _googleDriveService;


        private DateTime _lastActionTime = DateTime.MinValue;
        private DateTime _lastModifiedDrive = DateTime.MinValue;
        private DateTime _lastModifiedLocally = DateTime.MinValue;


        private string _lastActionType = "None";
        private System.Windows.Forms.Timer _timer05Seconds;
        private System.Windows.Forms.Timer _timer30Seconds;

        private string _sharingLink = string.Empty;




        // Constructor - this runs when the form is created
        public MainForm()
        {


        // This method is automatically generated and sets up the UI components
            InitializeComponent();

            _googleDriveService = new GoogleDriveService();
            _googleDriveService.InitializeAsync();

            // Create our Factorio monitor
            _factorioMonitor = new FactorioMonitor();

            // Subscribe to the FactorioClosed event
            _factorioMonitor.FactorioClosed += OnFactorioClosed;

            // Start monitoring for Factorio
            _factorioMonitor.StartMonitoring();


            _appSettings = ApplicationSettings.LoadSettings();

            linkFolderUrl.Text = _appSettings.LastSharedFolderLink;
            _sharingLink = _appSettings.LastSharedFolderLink;
            

            if (!string.IsNullOrEmpty(_appSettings.LastSharedFolderLink))
            {
                _googleDriveService.SetCustomFolderLink(_appSettings.LastSharedFolderLink);
            }


            _timer05Seconds = new System.Windows.Forms.Timer(this.components);
            _timer05Seconds.Interval = 500;
            _timer05Seconds.Tick += new EventHandler(On05SecondsTimer);
            _timer05Seconds.Start();



            // Initialize the timer to update the "Last Action" time display
            _timer30Seconds = new System.Windows.Forms.Timer(this.components);
            _timer30Seconds.Interval = 30000; // Update every 30 seconds
            _timer30Seconds.Tick += new EventHandler(On30SecondTimer);
            _timer30Seconds.Start();



            


            // Update the UI with the current save file name
            UpdateSaveFileDisplay();

            // Update game status initially
            UpdateGameStatusDisplay();

            // Initialize last action display
            UpdateTimeLabels();
           

        }



        private async void UpdateTimeLabels()
        {
            await UpdateLastModifiedLocallyAsync();

            UpdateLastLocalDisplay();

            UpdateLastActionDisplay();

            await UpdateLastModifiedDriveAsync();

            UpdateDriveTimeDisplay();


        }

        // Handle clicking on the sharing link
        private void linkFolderUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_sharingLink))
            {
                try
                {
                    // Open the link in the default browser
                    System.Diagnostics.Process.Start(_sharingLink);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Error opening link: {ex.Message}\n\nYou can manually copy the link and open it in your browser.",
                        "Error Opening Link",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btnCopyLink_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_sharingLink))
            {
                try
                {
                    // Copy the link to clipboard
                    Clipboard.SetText(_sharingLink);
                    lblStatus.Text = "Status: Link copied to clipboard";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Error copying link: {ex.Message}",
                        "Clipboard Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(
                    "No sharing link available. Please generate a link first.",
                    "No Link Available",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            using (var settingsForm = new SettingsForm(_appSettings))
            {
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    // If the folder link has changed, apply it
                    if (!string.IsNullOrEmpty(_appSettings.LastSharedFolderLink))
                    {
                        _googleDriveService.SetCustomFolderLink(_appSettings.LastSharedFolderLink);
                    }
                }
            }
        }

        // Handle edit link button click
        private void btnEditLink_Click(object sender, EventArgs e)
        {
            // Show input dialog to enter custom folder ID
            using (var inputForm = new Form())
            {
                inputForm.Width = 500;
                inputForm.Height = 200;
                inputForm.Text = "Enter Shared Folder Link";
                inputForm.StartPosition = FormStartPosition.CenterParent;
                inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                inputForm.MaximizeBox = false;
                inputForm.MinimizeBox = false;

                var label = new Label()
                {
                    Text = "Enter Google Drive folder link or ID:",
                    AutoSize = true,
                    Location = new Point(20, 20)
                };

                var textBox = new TextBox()
                {
                    Text = _sharingLink,
                    Width = 450,
                    Location = new Point(20, 50)
                };

                var okButton = new Button()
                {
                    Text = "OK",
                    DialogResult = DialogResult.OK,
                    Location = new Point(290, 90)
                };

                var cancelButton = new Button()
                {
                    Text = "Cancel",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(370, 90)
                };

                inputForm.Controls.Add(label);
                inputForm.Controls.Add(textBox);
                inputForm.Controls.Add(okButton);
                inputForm.Controls.Add(cancelButton);

                inputForm.AcceptButton = okButton;
                inputForm.CancelButton = cancelButton;

                var result = inputForm.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(textBox.Text))
                {
                    SetCustomFolderLink(textBox.Text);
                }
            }
        }

        // Handle generate link button click
        private async void btnGenerateLink_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "Status: Generating public sharing link...";

                // Generate a public sharing link
                string link = await _googleDriveService.CreatePublicSharingLinkAsync();

                if (!string.IsNullOrEmpty(link))
                {
                    // Save and display the link
                    _sharingLink = link;
                    linkFolderUrl.Text = link;
                    _appSettings.LastSharedFolderLink = link;
                    _appSettings.SaveSettings();
                    lblStatus.Text = "Status: Public sharing link generated successfully";
                }
                else
                {
                    lblStatus.Text = "Status: Failed to generate public sharing link";
                    MessageBox.Show(
                        "Failed to generate a public sharing link. Please try again later.",
                        "Link Generation Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Status: Error generating public link";
                MessageBox.Show(
                    $"Error generating public link: {ex.Message}",
                    "Link Generation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Set a custom folder link
        private async void SetCustomFolderLink(string link)
        {
            try
            {
                lblStatus.Text = "Status: Validating folder link...";

                // Set the custom folder link in the service
                _googleDriveService.SetCustomFolderLink(link);

                // Validate the folder ID
                bool isValid = await _googleDriveService.ValidateFolderIdAsync(_googleDriveService.GetFolderId());

                if (isValid)
                {
                    // Get the web link for the folder
                    string webLink = await _googleDriveService.GetFolderLinkAsync();

                    if (!string.IsNullOrEmpty(webLink))
                    {
                        _sharingLink = webLink;
                        linkFolderUrl.Text = webLink;
                        lblStatus.Text = "Status: Custom folder link set successfully";
                    }
                    else
                    {
                        lblStatus.Text = "Status: Could not get folder web link";
                    }
                }
                else
                {
                    lblStatus.Text = "Status: Invalid folder ID";
                    MessageBox.Show(
                        "The provided folder ID or link is not valid. Please check and try again.",
                        "Invalid Folder",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Status: Error setting custom folder";
                MessageBox.Show(
                    $"Error setting custom folder: {ex.Message}",
                    "Folder Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Add this to your constructor to initialize sharing
        private async void InitializeSharingAsync()
        {
            try
            {
                // Try to get an existing link
                string link = await _googleDriveService.GetFolderLinkAsync();

                if (!string.IsNullOrEmpty(link))
                {
                    _sharingLink = link;
                    linkFolderUrl.Text = link;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing sharing: {ex.Message}");
            }
        }



        private void btnUploadToDrive_Click(object sender, EventArgs e)
        {
            // Call the existing upload method
            UploadSaveToGoogleDrive();
        }


        // This method runs when the form is being closed
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Stop the monitoring service
            _factorioMonitor.StopMonitoring();

            _timer05Seconds.Stop();
            _timer30Seconds.Stop();


            // Call the base class method
            base.OnFormClosing(e);
        }

        // Event handler for when Factorio is closed
        private void OnFactorioClosed(object sender, EventArgs e)
        {
            Invoke(new Action(() => {
                UpdateGameStatusDisplay();

                switch (_appSettings.CloseAction)
                {
                    case SyncAction.Auto:
                        UploadSaveToGoogleDrive();
                        break;

                    case SyncAction.Prompt:
                        ShowSyncPrompt(false);
                        break;

                    case SyncAction.None:
                    default:
                        // Do nothing
                        break;
                }
            }));
        }

        private void OnFactorioOpened(object sender, EventArgs e)
        {
            Invoke(new Action(() => {
                UpdateGameStatusDisplay();

                switch (_appSettings.OpenAction)
                {
                    case SyncAction.Auto:
                        btnDownloadFromDrive_Click(this, EventArgs.Empty);
                        break;

                    case SyncAction.Prompt:
                        ShowSyncPrompt(true);
                        break;

                    case SyncAction.None:
                    default:
                        // Do nothing
                        break;
                }
            }));
        }

        
        // Shows the dialog asking the user if they want to sync their save
        private void ShowSyncPrompt(bool opened)
        {
            string action = opened ? "download from" : "upload to";

            var result = MessageBox.Show(
                $"Factorio has been {(opened ? "opened" : "closed")}. Would you like to {(opened ? "download" : "upload")} your save {(opened ? "from" : "to")} Google Drive?",
                "Factorio Save Sync",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (opened)
                    _googleDriveService.DownloadSaveAsync(_factorioMonitor.SaveFileName, _factorioMonitor.GetFactorioSavesDirectory());
                else
                    UploadSaveToGoogleDrive();
            }
        }

        // Called when the user selects a different save file
        private void btnSelectSaveFile_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                try
                {
                    // Set the initial directory to the Factorio saves folder (without the filename)
                    string savesDir = Environment.OSVersion.Platform == PlatformID.Win32NT
                        ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Factorio", "saves")
                        : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".factorio", "saves");

                    // Make sure the directory exists, otherwise default to a safe location
                    if (Directory.Exists(savesDir))
                    {
                        dialog.InitialDirectory = savesDir;
                    }
                    else
                    {
                        dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    }
                }
                catch
                {
                    // If there's any error determining the path, default to documents
                    dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }

                dialog.Filter = "Factorio Saves (*.zip)|*.zip";
                dialog.Title = "Select Factorio Save File";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Get just the filename without path
                        string fileName = Path.GetFileName(dialog.FileName);

                        // Update the monitor with the new save file name
                        _factorioMonitor.SaveFileName = fileName;

                        // Update the UI
                        UpdateSaveFileDisplay();

                        // Set a successful status message
                        lblStatus.Text = "Status: Save file selected successfully";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error selecting save file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        lblStatus.Text = "Status: Error selecting save file";
                    }
                }
            }
        }



        // Updates the UI showing the current save file information

        private async void UpdateSaveFileDisplay()
        {
            // Check if we have a save file selected
            if (string.IsNullOrEmpty(_factorioMonitor.SaveFileName) || _factorioMonitor.SaveFileName == "None")
            {
                lblCurrentSave.Text = "Current Save: None";
                lblLastModified.Text = "Last Modified locally: --";
                lblDriveLastModified.Text = "Last Modified on Drive: --";
                lblSavePath.Text = "Save Path: --";
                return;
            }

            // Update save file name
            lblCurrentSave.Text = $"Current Save: {_factorioMonitor.SaveFileName}";

            // Get the full path to the save file
            string savePath = _factorioMonitor.GetFactorioSavesDirectory();
            lblSavePath.Text = $"Save Path: {savePath}";

            // Check if the file exists locally and get its last modified time
            

            // Check Google Drive for the same file's modification time
            try
            {
                // Only attempt to get Drive info if the service is initialized and we have a file name

                //lblDriveLastModified.Text = $"Last Modified (Drive): {_lastModifiedDrive.("yyyy-MM-dd HH:mm:ss")}";
            }
            catch (Exception ex)
            {
                lblDriveLastModified.Text = "Last Modified on Drive: Error reading Drive info";
                System.Diagnostics.Debug.WriteLine($"Error getting Drive file information: {ex.Message}");
            }
        }

        private async void UpdateLastLocalDisplay()
        {
            try
            {
                if (File.Exists(_factorioMonitor.GetFactorioSavesDirectory()))
                {
                    TimeSpan elapsed = DateTime.Now - _lastModifiedLocally;
                    lblLastModified.Text = $"Last Modified locally: {formatTime(elapsed)} / {_lastModifiedLocally.ToString("dd.MM.yyyy HH:MM:ss")}";
                }
                else
                {
                    lblLastModified.Text = "Last Modified locally: File not found";
                }
            }
            catch (Exception ex)
            {
                lblLastModified.Text = "Last Modified locally: Error reading file info";
                System.Diagnostics.Debug.WriteLine($"Error getting local file information: {ex.Message}");
            }
        }

       

        // Uploads the current save to Google Drive
        private async void UploadSaveToGoogleDrive()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Uploading save to google drive..");

                // Check if a save file is selected
                if (string.IsNullOrEmpty(_factorioMonitor.SaveFileName) || _factorioMonitor.SaveFileName == "None")
                {
                    MessageBox.Show(
                        "Please select a save file first.",
                        "No Save Selected",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Check if the save file exists
                string savePath = _factorioMonitor.GetFactorioSavesDirectory();
                if (!File.Exists(savePath))
                {
                    MessageBox.Show(
                        "The selected save file does not exist. Please check the file path or select another save.",
                        "File Not Found",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Update status
                lblStatus.Text = "Status: Uploading to Google Drive...";

                // TODO: Implement Google Drive integration
                // This is just a placeholder for now
                await _googleDriveService.UploadSaveAsync(savePath);  

                // Record the upload action
                RecordSyncAction("Upload");

                // Update status and UI
                lblStatus.Text = "Status: Upload successful!";

                // Refresh the last modified information
                UpdateSaveFileDisplay();
                await UpdateLastModifiedDriveAsync();
                UpdateDriveTimeDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error uploading save: {ex.Message}",
                    "Upload Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                lblStatus.Text = "Status: Upload failed.";
            }
        }
        
        //Downloads the save from drive using info from _factorioMonitor
        private async void downloadSaveFromDrive()
        {
            await _googleDriveService.DownloadSaveAsync(_factorioMonitor.SaveFileName, _factorioMonitor.GetFactorioSavesDirectory());

            // Record the download action
            RecordSyncAction("Download");

            lblStatus.Text = "Status: Download successful!";
            UpdateLastModifiedLocallyAsync();

            // Refresh the information display
            UpdateSaveFileDisplay();
        }


        // Downloads the save from Google Drive
        private async void btnDownloadFromDrive_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if a save file is selected
                if (string.IsNullOrEmpty(_factorioMonitor.SaveFileName) || _factorioMonitor.SaveFileName == "None")
                {
                    MessageBox.Show(
                        "Please select a save file name first.",
                        "No Save Selected",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                lblStatus.Text = "Status: Downloading from Google Drive...";

                downloadSaveFromDrive();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error downloading save: {ex.Message}",
                    "Download Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                lblStatus.Text = "Status: Download failed.";
            }
        }


        

        // Timer tick event handler to update game status
        private void On05SecondsTimer(object sender, EventArgs e)
        {
            UpdateGameStatusDisplay();
            UpdateLastActionDisplay();
            UpdateDriveTimeDisplay();
            UpdateLastLocalDisplay();
            UpdateLastModifiedLocallyAsync();
        }

        // Updates the game status indicator
        private void UpdateGameStatusDisplay()
        {
            bool isRunning = _factorioMonitor.IsFactorioRunning();

            if (isRunning)
            {
                // Update text and color for running state
                lblGameStatus.Text = "Factorio: Running";
                lblGameStatus.ForeColor = Color.DarkGreen;
                panelGameStatus.BackColor = Color.FromArgb(220, 255, 220); // Light green
            }
            else
            {
                // Update text and color for not running state
                lblGameStatus.Text = "Factorio: Not Running";
                lblGameStatus.ForeColor = Color.DarkRed;
                panelGameStatus.BackColor = Color.FromArgb(255, 220, 220); // Light red
            }
        }



        // Updates the last action display with time elapsed
        private void UpdateLastActionDisplay()
        {
            if (_lastActionTime == DateTime.MinValue)
            {
                lblLastAction.Text = "No sync actions yet";
                return;
            }

            // Calculate the time difference
            TimeSpan elapsed = DateTime.Now - _lastActionTime;
            string timeText = formatTime(elapsed);

            // Update the label
            lblLastAction.Text = $"Last {_lastActionType}: {timeText}";
        }

        private void UpdateDriveTimeDisplay()
        {
            if (_lastModifiedDrive == DateTime.MinValue)
            {
                lblDriveLastModified.Text = "Last Modified on Drive: No info";
                return;
            }

            // Calculate the time difference
            TimeSpan elapsed = DateTime.Now - _lastModifiedDrive;
            string timeText = formatTime(elapsed);

            // Update the label
            lblDriveLastModified.Text = $"Last Modified on Drive: {timeText} / {_lastModifiedDrive.ToString("dd.MM.yyyy HH:MM:ss")}";
        }

        private string formatTime(TimeSpan elapsed)
        {
            string timeText;

            // Format the elapsed time in a human-readable way
            if (elapsed.TotalDays > 3)
            {
                // For more than 3 days, show the actual date
                timeText = _lastActionTime.ToString("yyyy-MM-dd HH:mm");
            }
            else if (elapsed.TotalDays >= 1)
            {
                // For 1-3 days
                int days = (int)Math.Floor(elapsed.TotalDays);
                timeText = $"{days} day{(days > 1 ? "s" : "")} ago";
            }
            else if (elapsed.TotalHours >= 1)
            {
                // For hours
                int hours = (int)Math.Floor(elapsed.TotalHours);
                timeText = $"{hours} hour{(hours > 1 ? "s" : "")} ago";
            }
            else if (elapsed.TotalMinutes >= 1)
            {
                // For minutes
                int minutes = (int)Math.Floor(elapsed.TotalMinutes);
                timeText = $"{minutes} minute{(minutes > 1 ? "s" : "")} ago";
            }
            else
            {
                // For seconds
                int seconds = (int)Math.Floor(elapsed.TotalSeconds);
                timeText = $"{seconds} second{(seconds > 1 ? "s" : "")} ago";
            }

            return timeText;
        }

        private async Task UpdateLastModifiedDriveAsync()
        {
            try
            {
                SaveFileInfo info = await _googleDriveService.GetSaveFileInfoAsync(_factorioMonitor.SaveFileName);
                if(info != null && info.ModifiedTimeOffset.HasValue)
                {
                    DateTimeOffset offset = info.ModifiedTimeOffset.Value;
                    _lastModifiedDrive = offset.LocalDateTime; 
            
                    
                    UpdateDriveTimeDisplay();
            
                    
                    System.Diagnostics.Debug.WriteLine($"Drive last modified: {_lastModifiedDrive}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No valid modified time returned from Drive");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating Drive modified time: {ex.Message}");
            }
        }

        private async Task UpdateLastModifiedLocallyAsync()
        {
            _lastModifiedLocally = File.GetLastWriteTime(_factorioMonitor.GetFactorioSavesDirectory());
        }

        // Timer tick handler to update the last action time display
        private async void On30SecondTimer(object sender, EventArgs e)
        {
            UpdateLastModifiedDriveAsync();
        }

        // Records a new sync action
        private void RecordSyncAction(string actionType)
        {
            _lastActionTime = DateTime.Now;
            _lastActionType = actionType;
            UpdateLastActionDisplay();
        }       
        // Timer tick event handler to update game status
        

        // Updates the game status indicator
        
    }
}