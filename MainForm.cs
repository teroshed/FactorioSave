using FactorioSave.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
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
        private bool _hasDriveLocation = false;


        private System.Windows.Forms.Timer _pulseTimer;
        private Dictionary<Button, Color> _originalButtonColors = new Dictionary<Button, Color>();
        private bool _pulseIncreasing = true;
        private int _pulseValue = 0;




        // Constructor - this runs when the form is created
        public MainForm()
        {


        // This method is automatically generated and sets up the UI components
            InitializeComponent();

            //ApplyRoundedCorners();


            _googleDriveService = new GoogleDriveService();
            _googleDriveService.InitializeAsync();

            // Create our Factorio monitor
            _factorioMonitor = new FactorioMonitor();

            // Subscribe to the FactorioClosed event
            _factorioMonitor.FactorioClosed += OnFactorioClosed;

            // Start monitoring for Factorio
            _factorioMonitor.StartMonitoring();


            _appSettings = ApplicationSettings.LoadSettings();


            _sharingLink = _appSettings.LastSharedFolderLink;
            txtFolderUrl.Text = string.IsNullOrEmpty(_sharingLink) ?
                "No sharing link available" : _sharingLink;
            btnOpenLink.Enabled = !string.IsNullOrEmpty(_sharingLink);

            if (!string.IsNullOrEmpty(_appSettings.DriveTargetLocation))
            {
                lblDriveLocation.Text = $"Drive location: {_appSettings.DriveTargetLocation}";

            }

            if (!string.IsNullOrEmpty(_appSettings.LastSharedFolderLink))
            {
                _googleDriveService.SetCustomFolderLink(_appSettings.LastSharedFolderLink);
            }


            if (!string.IsNullOrEmpty(_appSettings.LastSharedFolderLink))
            {
                _googleDriveService.SetCustomFolderLink(_appSettings.LastSharedFolderLink);
            }


            



            


            // Update the UI with the current save file name
            UpdateSaveFileDisplay();

            // Update game status initially
            UpdateGameStatusDisplay();

            // Initialize last action display
            UpdateTimeLabels();

            initializeTimers();



            UpdateButtonStates();






        }

        private void initializeTimers()
        {
            if (this.components == null)
            {
                this.components = new System.ComponentModel.Container();
            }
            _pulseTimer = new System.Windows.Forms.Timer(this.components);
            _pulseTimer.Interval = 50; // 50ms for smooth animation
            _pulseTimer.Tick += OnPulseTimerTick;

            _timer05Seconds = new System.Windows.Forms.Timer(this.components);
            _timer05Seconds.Interval = 500;
            _timer05Seconds.Tick += new EventHandler(On05SecondsTimer);
            _timer05Seconds.Start();



            // Initialize the timer to update the "Last Action" time display
            _timer30Seconds = new System.Windows.Forms.Timer(this.components);
            _timer30Seconds.Interval = 30000; // Update every 30 seconds
            _timer30Seconds.Tick += new EventHandler(On30SecondTimer);
            _timer30Seconds.Start();

            
        }

        // Apply rounded corners to all controls
        private void ApplyRoundedCorners()
        {
            // Apply to all panels
            ApplyRoundCorners(panelHeader, 10);
            ApplyRoundCorners(panelSaveInfo, 15);
            ApplyRoundCorners(panelSharing, 15);
            ApplyRoundCorners(panelGameStatus, 15);
            ApplyRoundCorners(panelLastAction, 15);

            // Apply to all buttons
            ApplyRoundCorners(btnSelectSaveFile, 20);
            ApplyRoundCorners(btnSearchSave, 20);
            ApplyRoundCorners(btnUploadToDrive, 20);
            ApplyRoundCorners(btnDownloadFromDrive, 20);
            ApplyRoundCorners(btnSettings, 10);
            ApplyRoundCorners(btnCopyLink, 15);
            ApplyRoundCorners(btnEditLink, 15);
            ApplyRoundCorners(btnGenerateLink, 15);
        }

        // Helper method to apply rounded corners to a control
        private void ApplyRoundCorners(Control control, int radius)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(control.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(control.Width - radius, control.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, control.Height - radius, radius, radius, 90, 90);
                path.CloseAllFigures();

                control.Region = new Region(path);
            }

            // Make sure the form repaints properly
            control.Paint += (sender, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            };
        }


        private void OnPulseTimerTick(object sender, EventArgs e)
        {
            // Pulse all buttons in the dictionary
            foreach (var btn in _originalButtonColors.Keys.ToList())
            {
                // Calculate pulse amount (0-40)
                if (_pulseIncreasing)
                {
                    _pulseValue += 2;
                    if (_pulseValue >= 40) _pulseIncreasing = false;
                }
                else
                {
                    _pulseValue -= 2;
                    if (_pulseValue <= 0) _pulseIncreasing = true;
                }

                // Get base color
                var baseColor = _originalButtonColors[btn];

                // Apply pulse
                int r = Math.Min(255, baseColor.R + _pulseValue);
                int g = Math.Min(255, baseColor.G + _pulseValue);
                int b = Math.Min(255, baseColor.B + _pulseValue);

                btn.BackColor = Color.FromArgb(r, g, b);
            }
        }

        private void StartButtonPulse(Button button)
        {
            // Save original color if not already saved
            if (!_originalButtonColors.ContainsKey(button))
            {
                _originalButtonColors[button] = button.BackColor;
            }

            // Make sure timer is running
            if (!_pulseTimer.Enabled)
            {
                _pulseTimer.Start();
            }
        }

        private void StopButtonPulse(Button button)
        {
            // If button is being pulsed, remove it and restore original color
            if (_originalButtonColors.ContainsKey(button))
            {
                button.BackColor = _originalButtonColors[button];
                _originalButtonColors.Remove(button);
            }

            // If no more buttons to pulse, stop the timer
            if (_originalButtonColors.Count == 0 && _pulseTimer.Enabled)
            {
                _pulseTimer.Stop();
            }
        }



        private async void UpdateTimeLabels()
        {
            await UpdateLastModifiedLocallyAsync();

            UpdateLastLocalDisplay();

            UpdateLastActionDisplay();

            await UpdateLastModifiedDriveAsync();

            UpdateDriveTimeDisplay();


        }

        // Method to check if a Drive location exists and update button states
        private void UpdateButtonStates()
        {
            // Check if we have a valid drive location and link
            _hasDriveLocation = !string.IsNullOrEmpty(_appSettings.DriveTargetLocation) ||
                                !string.IsNullOrEmpty(_googleDriveService.GetFolderId());

            bool hasLink = !string.IsNullOrEmpty(_sharingLink);

            // Disable download button if we don't have a Drive location
            btnDownloadFromDrive.Enabled = _hasDriveLocation;

            // If no Drive location or link, highlight the search and edit buttons
            if (!_hasDriveLocation || !hasLink)
            {
                // Make Search button green and animated if no Drive location
                if (!_hasDriveLocation)
                {
                    btnSearchSave.BackColor = Color.FromArgb(92, 184, 92); // Green
                    StartButtonPulse(btnSearchSave);
                }
                else
                {
                    btnSearchSave.BackColor = Color.FromArgb(240, 173, 78); // Yellow/amber
                    StopButtonPulse(btnSearchSave);
                }

                // Make Edit Link button green and animated if no link
                if (!hasLink)
                {
                    btnEditLink.BackColor = Color.FromArgb(92, 184, 92); // Green
                    StartButtonPulse(btnEditLink);
                }
                else
                {
                    btnEditLink.BackColor = Color.FromArgb(224, 224, 224); // Original color
                    StopButtonPulse(btnEditLink);
                }
            }
            else
            {
                // If we have both, restore original colors and stop animation
                btnSearchSave.BackColor = Color.FromArgb(240, 173, 78);
                btnEditLink.BackColor = Color.FromArgb(224, 224, 224);
                StopButtonPulse(btnSearchSave);
                StopButtonPulse(btnEditLink);
            }
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

        // Update btnGenerateLink_Click method:
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
                    txtFolderUrl.Text = link;
                    btnOpenLink.Enabled = true; // Enable the Open button
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
        // Update SetCustomFolderLink method:
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
                        txtFolderUrl.Text = webLink;
                        btnOpenLink.Enabled = true; // Enable the Open button
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
                    lblLastModified.Text = $"Last Modified locally: {FactorioMonitor.FormatTime(elapsed)} / {_lastModifiedLocally.ToString("dd.MM.yyyy HH:MM:ss")}";
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
        private async Task UploadSaveToGoogleDrive()
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
            string timeText = FactorioMonitor.FormatTime(elapsed);

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
            string timeText = FactorioMonitor.FormatTime(elapsed);

            // Update the label
            lblDriveLastModified.Text = $"Last Modified on Drive: {timeText} / {_lastModifiedDrive.ToString("dd.MM.yyyy HH:MM:ss")}";
        }

        /**
        public static string FormatTime(TimeSpan elapsed)
        {
            string timeText;

            // Format the elapsed time in a human-readable way
            if (elapsed.TotalDays > 3)
            {
                // For more than 3 days, show the actual date
                timeText = elapsed.ToString("dd.MM.yyyy HH:mm");
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
        } */


        // Handles the Open button click to open the link in a browser
        /// </summary>
        private void btnOpenLink_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_sharingLink))
            {
                try
                {
                    // Open the link in the default browser
                    System.Diagnostics.Process.Start(new ProcessStartInfo
                    {
                        FileName = _sharingLink,
                        UseShellExecute = true
                    });
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



        /// <summary>
        /// Handles the text box click event to enable editing
        /// </summary>
        private void txtFolderUrl_Click(object sender, EventArgs e)
        {
            // Only enable editing if there is a valid link
            if (!string.IsNullOrEmpty(_sharingLink) && _sharingLink != "No sharing link available")
            {
                txtFolderUrl.ReadOnly = false;
                txtFolderUrl.SelectAll();
            }
        }

        /// <summary>
        /// Searches for the current save file on Google Drive and offers organization options
        /// </summary>
        private async void btnSearchSave_Click(object sender, EventArgs e)
        {
            try
            {
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

                lblStatus.Text = "Status: Searching for save on Google Drive...";

                // Search for the file in Drive
                var saveFiles = await _googleDriveService.GetSaveFilesListAsync();
                var existingSave = saveFiles.FirstOrDefault(f => f.Name == _factorioMonitor.SaveFileName);

                if (existingSave != null)
                {
                    // Save found on Drive - get detailed location info
                    var locationInfo = await _googleDriveService.GetFileLocationAsync(existingSave.Id);

                    // Format the modified time if available
                    string modifiedText = locationInfo.ModifiedTime.HasValue
                        ? locationInfo.ModifiedTime.Value.ToString("dd.MM.yyyy - HH:mm:ss")
                        : "Unknown";

                    // Show the file location in the UI with owner and last modified time
                    lblDriveLocation.Text = $"Drive Location: {locationInfo.Path}";

                    // Create a message with all the details
                    string message =
                        $"Save file '{_factorioMonitor.SaveFileName}' found on Google Drive.\n\n" +
                        $"Location: {locationInfo.Path}\n" +
                        $"Owner: {locationInfo.Owner}\n" +
                        $"Last Modified: {modifiedText}";

                    MessageBox.Show(
                        message,
                        "Save Found",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    // The file already exists, so we set the current location as the target
                    _appSettings.DriveTargetLocation = locationInfo.Path;
                    _appSettings.SaveSettings();

                    // Update button states
                    UpdateButtonStates();
                }
                else
                {
                    // Save not found - show organization options dialog
                    ShowSaveOrganizationDialog();
                }

                lblStatus.Text = "Status: Ready";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Status: Error searching for save";
                MessageBox.Show(
                    $"Error searching for save: {ex.Message}",
                    "Search Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Shows a dialog to let the user choose how to organize saves on Drive
        /// </summary>
        private void ShowSaveOrganizationDialog()
        {
            using (var organizerForm = new Form())
            {
                organizerForm.Text = "Save Organization Options";
                organizerForm.Width = 450;
                organizerForm.Height = 250;
                organizerForm.StartPosition = FormStartPosition.CenterParent;
                organizerForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                organizerForm.MaximizeBox = false;
                organizerForm.MinimizeBox = false;

                // Apply rounded corners to the form
                using (System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    int radius = 15;
                    path.AddArc(0, 0, radius, radius, 180, 90);
                    path.AddArc(organizerForm.Width - radius, 0, radius, radius, 270, 90);
                    path.AddArc(organizerForm.Width - radius, organizerForm.Height - radius, radius, radius, 0, 90);
                    path.AddArc(0, organizerForm.Height - radius, radius, radius, 90, 90);
                    path.CloseAllFigures();

                    organizerForm.Region = new Region(path);
                }

                // Information label
                var lblInfo = new Label
                {
                    Text = $"Save file '{_factorioMonitor.SaveFileName}' was not found on Google Drive.",
                    AutoSize = true,
                    Location = new Point(20, 20)
                };

                // Create folder checkbox
                var chkCreateFolder = new CheckBox
                {
                    Text = "Create a FactorioSaves folder on Drive",
                    Checked = true,
                    AutoSize = true,
                    Location = new Point(20, 60)
                };

                // Upload save checkbox
                var chkUploadSave = new CheckBox
                {
                    Text = "Upload save file now",
                    Checked = false,
                    AutoSize = true,
                    Location = new Point(20, 90)
                };

                // Location preview label
                var lblPreview = new Label
                {
                    Text = $"Target location: /FactorioSaves/{_factorioMonitor.SaveFileName}",
                    AutoSize = true,
                    Location = new Point(20, 130)
                };

                // Update preview when checkbox changes
                chkCreateFolder.CheckedChanged += (s, e) =>
                {
                    lblPreview.Text = chkCreateFolder.Checked
                        ? $"Target location: /FactorioSaves/{_factorioMonitor.SaveFileName}"
                        : $"Target location: /{_factorioMonitor.SaveFileName}";
                };

                // OK button with rounded corners
                var btnOK = new Button
                {
                    Text = "OK",
                    DialogResult = DialogResult.OK,
                    Size = new Size(100, 35),
                    Location = new Point(330, 170),
                    BackColor = Color.FromArgb(92, 184, 92),
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.White
                };

                // Apply rounded corners to the OK button
                using (System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    int radius = 10;
                    path.AddArc(0, 0, radius, radius, 180, 90);
                    path.AddArc(btnOK.Width - radius, 0, radius, radius, 270, 90);
                    path.AddArc(btnOK.Width - radius, btnOK.Height - radius, radius, radius, 0, 90);
                    path.AddArc(0, btnOK.Height - radius, radius, radius, 90, 90);
                    path.CloseAllFigures();

                    btnOK.Region = new Region(path);
                }

                // Cancel button with rounded corners
                var btnCancel = new Button
                {
                    Text = "Cancel",
                    DialogResult = DialogResult.Cancel,
                    Size = new Size(100, 35),
                    Location = new Point(220, 170)
                };

                // Apply rounded corners to the Cancel button
                using (System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    int radius = 10;
                    path.AddArc(0, 0, radius, radius, 180, 90);
                    path.AddArc(btnCancel.Width - radius, 0, radius, radius, 270, 90);
                    path.AddArc(btnCancel.Width - radius, btnCancel.Height - radius, radius, radius, 0, 90);
                    path.AddArc(0, btnCancel.Height - radius, radius, radius, 90, 90);
                    path.CloseAllFigures();

                    btnCancel.Region = new Region(path);
                }

                // Add controls to form
                organizerForm.Controls.Add(lblInfo);
                organizerForm.Controls.Add(chkCreateFolder);
                organizerForm.Controls.Add(chkUploadSave);
                organizerForm.Controls.Add(lblPreview);
                organizerForm.Controls.Add(btnOK);
                organizerForm.Controls.Add(btnCancel);

                organizerForm.AcceptButton = btnOK;
                organizerForm.CancelButton = btnCancel;

                // Show dialog and process result
                if (organizerForm.ShowDialog() == DialogResult.OK)
                {
                    // Set the target location based on user choice
                    string targetLocation = chkCreateFolder.Checked
                        ? $"/FactorioSaves/{_factorioMonitor.SaveFileName}"
                        : $"/{_factorioMonitor.SaveFileName}";

                    // Save the target location
                    _appSettings.DriveTargetLocation = targetLocation;
                    _appSettings.SaveSettings();

                    // Update the location display
                    lblDriveLocation.Text = $"Drive Location: {targetLocation}";

                    // Update button states now that we have a location
                    UpdateButtonStates();

                    // Upload if requested
                    if (chkUploadSave.Checked)
                    {
                        // Use Task.Run to avoid UI freezing during upload
                        Task.Run(() =>
                        {
                            this.Invoke(async () =>
                            {
                                try
                                {
                                    // Ensure folder exists if needed
                                    if (chkCreateFolder.Checked)
                                    {
                                        await _googleDriveService.EnsureFactorioFolderExistsAsync();
                                    }

                                    // Upload the save
                                    await UploadSaveToGoogleDrive();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(
                                        $"Error during upload: {ex.Message}",
                                        "Upload Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                                }
                            });
                        });
                    }
                }
            }
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

        private void lblGameStatus_Click(object sender, EventArgs e)
        {

        }
        // Timer tick event handler to update game status


        // Updates the game status indicator

    }
}