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
        private readonly FactorioMonitor _factorioMonitor;


        private DateTime _lastActionTime = DateTime.MinValue;
        private string _lastActionType = "None";
        private System.Windows.Forms.Timer _timerUpdateLastAction;

        // Constructor - this runs when the form is created
        public MainForm()
        {
            // This method is automatically generated and sets up the UI components
            InitializeComponent();

            // Create our Factorio monitor
            _factorioMonitor = new FactorioMonitor();

            // Subscribe to the FactorioClosed event
            _factorioMonitor.FactorioClosed += OnFactorioClosed;

            // Start monitoring for Factorio
            _factorioMonitor.StartMonitoring();

            // Start the game state timer
            timerGameState.Start();

            // Initialize the timer to update the "Last Action" time display
            _timerUpdateLastAction = new System.Windows.Forms.Timer(this.components);
            _timerUpdateLastAction.Interval = 30000; // Update every 30 seconds
            _timerUpdateLastAction.Tick += new EventHandler(OnTimerUpdateLastAction);
            _timerUpdateLastAction.Start();

            // Update the UI with the current save file name
            UpdateSaveFileDisplay();

            // Update game status initially
            UpdateGameStatusDisplay();

            // Initialize last action display
            UpdateLastActionDisplay();
        }

        

        // This method runs when the form is being closed
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Stop the monitoring service
            _factorioMonitor.StopMonitoring();

            timerGameState.Stop();
            _timerUpdateLastAction.Stop();


            // Call the base class method
            base.OnFormClosing(e);
        }

        // Event handler for when Factorio is closed
        private void OnFactorioClosed(object sender, EventArgs e)
        {
            // We need to use Invoke because this event might come from a different thread
            // and we want to update the UI, which must be done on the UI thread
            Invoke(new Action(() => {
                ShowSyncPrompt();
                UpdateGameStatusDisplay();
            }));
        }

        private void OnFactorioOpened(object sender, EventArgs e)
        {
            Invoke(new Action(() => {
                UpdateGameStatusDisplay();
            }));
        }

        // Shows the dialog asking the user if they want to sync their save
        private void ShowSyncPrompt()
        {
            var result = MessageBox.Show(
                "Factorio has been closed. Would you like to upload your save to Google Drive?",
                "Factorio Save Sync",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
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
        private void UpdateSaveFileDisplay()
        {
            // Check if we have a save file selected
            if (string.IsNullOrEmpty(_factorioMonitor.SaveFileName) || _factorioMonitor.SaveFileName == "None")
            {
                lblCurrentSave.Text = "Current Save: None";
                lblLastModified.Text = "Last Modified: --";
                lblSavePath.Text = "Save Path: --";
                return;
            }

            // Update save file name
            lblCurrentSave.Text = $"Current Save: {_factorioMonitor.SaveFileName}";

            // Get the full path to the save file
            string savePath = _factorioMonitor.GetFactorioSavesDirectory();
            lblSavePath.Text = $"Save Path: {savePath}";

            // Check if the file exists and get its last modified time
            try
            {
                if (File.Exists(savePath))
                {
                    DateTime lastModified = File.GetLastWriteTime(savePath);
                    lblLastModified.Text = $"Last Modified: {lastModified.ToString("dd.MM.yyyy HH:mm:ss")}";
                }
                else
                {
                    lblLastModified.Text = "Last Modified: File not found";
                }
            }
            catch (Exception ex)
            {
                lblLastModified.Text = "Last Modified: Error reading file info";
                Console.WriteLine($"Error getting file information: {ex.Message}");
            }
        }

        // Uploads the current save to Google Drive
        private async void UploadSaveToGoogleDrive()
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
                await Task.Delay(2000); // Simulate network delay

                // Record the upload action
                RecordSyncAction("Upload");

                // Update status and UI
                lblStatus.Text = "Status: Upload successful!";

                // Refresh the last modified information
                UpdateSaveFileDisplay();
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

                // TODO: Implement Google Drive integration
                // This is just a placeholder for now
                await Task.Delay(2000); // Simulate network delay

                // Record the download action
                RecordSyncAction("Download");

                lblStatus.Text = "Status: Download successful!";

                // Refresh the information display
                UpdateSaveFileDisplay();
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
        private void OnTimerGameStateTick(object sender, EventArgs e)
        {
            UpdateGameStatusDisplay();
            UpdateLastActionDisplay();
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

            // Update the label
            lblLastAction.Text = $"Last {_lastActionType}: {timeText}";
        }

        // Timer tick handler to update the last action time display
        private void OnTimerUpdateLastAction(object sender, EventArgs e)
        {
            UpdateLastActionDisplay();
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