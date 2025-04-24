using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FactorioSave
{
    public partial class SetupWizard : Form
    {

        private readonly GoogleDriveService _googleDriveService;
        private readonly FactorioMonitor _factorioMonitor;



        
        private System.Windows.Forms.Timer _timer05Seconds;


        private CancellationTokenSource _cancellationToken;
        public event EventHandler ClipboardChanged;
        private bool _isMonitoring = false;

        public int CurrentStep = 0;

        // Collected settings
        private string selectedSaveFile;
        private string savePath;
        private bool createNewFolder = true;
        private string folderLink;
        private bool sharePublicly = true;
        private SyncAction openAction = SyncAction.Auto;
        private SyncAction closeAction = SyncAction.Auto;
        private SyncDirection preferredDirection = SyncDirection.Auto;

        private Font _buttonFont;
        private string _lastCheckedLink = string.Empty;

        

        public SetupWizard(GoogleDriveService googleDriveService, FactorioMonitor factorioMonitor)
        {
            _googleDriveService = googleDriveService;
            _factorioMonitor = factorioMonitor;
            _buttonFont = new Font("Segoe UI", 12);
            _buttonFont = new Font(_buttonFont, FontStyle.Bold);




            InitializeComponent();
            InitializeWizardSteps();


            InitializeTimers();

            StartMonitoringClipboard();


        }

        private void StartMonitoringClipboard()
        {
            if (_isMonitoring)
                return;

            _isMonitoring = true;


            _cancellationToken = new CancellationTokenSource();

            Task.Run(() => MonitorClipboard(_cancellationToken.Token));



        }


        private async Task MonitorClipboard(CancellationToken cancellationToken)
        {

            try
            {
                System.Diagnostics.Debug.WriteLine($"Getting clipboard ? {GetClipboardAsync()}");

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting clipboard {e}");

            }
            string clipboardValueWas = await GetClipboardAsync();

            while (!cancellationToken.IsCancellationRequested)
            {
                //System.Diagnostics.Debug.WriteLine($"Clipboard was {clipboardValueWas}, is: {Clipboard.GetText()}");

                if (await GetClipboardAsync() != clipboardValueWas)
                {
                    OnClipboardChange();
                    System.Diagnostics.Debug.WriteLine("Clipboard changed :)");
                }
                clipboardValueWas = await GetClipboardAsync();
                await Task.Delay(500, cancellationToken).ConfigureAwait(false);

            }
        }

        private async Task<string> GetClipboardAsync()
        {
            try 
            {
                string clipboardValue = "";
                await Task.Run(() =>
                {
                    try
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            clipboardValue = Clipboard.GetText();
                        });
                    }
                    catch (Exception e)
                    {
                        

                    }
                });
                return clipboardValue;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting clipboard {e}");
                return string.Empty;
            }
            
        }



        private void OnClipboardChange()
        {
            Invoke(new Action(() => {
                if(CurrentStep == 2)
                {
                    CheckClipboardForDriveLink();
                }
            }));
        }

        private async void InitializeTimers()
        {
            if (this.components == null)
            {
                this.components = new System.ComponentModel.Container();
            }

            _timer05Seconds = new System.Windows.Forms.Timer(this.components);
            _timer05Seconds.Interval = 500;
            _timer05Seconds.Tick += new EventHandler(On05SecondsTimer);
            _timer05Seconds.Start();
        }

        private void On05SecondsTimer(object sender, EventArgs e)
        {
        }



        private void InitializeWizardSteps()
        {
            // Create all wizard step panels
            InitializeWelcomeStep();
            InitializeSaveSelectionStep();
            InitializeDriveSetupStep();
            InitializeSyncSettingsStep();
            InitializeCompleteStep();

            // Add all steps to an array for easy navigation
            wizardSteps = new Panel[] {
                panelWelcome,
                panelSaveSelection,
                panelDriveSetup,
                panelSyncSettings,
                panelComplete
            };

            // Add the first step to the main panel
            panelMain.Controls.Add(wizardSteps[0]);

            // Navigate to the first step
            NavigateToStep(0);
        }

        

        

        private void LoadSaveFiles()
        {
            try
            {
                // Determine Factorio saves directory
                string savesDir = FactorioMonitor.GetFactorioSaveFolder();

                // Check if the directory exists
                if (!Directory.Exists(savesDir))
                {
                    cboSaveFiles.Items.Add("No save files found");
                    cboSaveFiles.SelectedIndex = 0;
                    return;
                }

                // Load all .zip files in the saves directory
                string[] saveFiles = Directory.GetFiles(savesDir, "*.zip");

                if (saveFiles.Length == 0)
                {
                    cboSaveFiles.Items.Add("No save files found");
                    cboSaveFiles.SelectedIndex = 0;
                    return;
                }
                
                


                // Add each save file to the combo box
                for (int i = 0; i < saveFiles.Length; i++)
                {

                    cboSaveFiles.Items.Add(Path.GetFileName(saveFiles[i]));
                    System.Diagnostics.Debug.WriteLine($"file: {saveFiles[i]}");
                    if (saveFiles[i].EndsWith("sayvGameTolyanSpAge.zip"))
                    {
                        System.Diagnostics.Debug.WriteLine($"Yes: {saveFiles[i]}");
                        cboSaveFiles.SelectedIndex = i;
                        selectedSaveFile = cboSaveFiles.SelectedItem.ToString();
                        savePath = Path.Combine(savesDir, selectedSaveFile);
                        lblSavePath.Text = $"Save location: {savePath}";
                        return;
                    }
                    
                }

                
                // Select the first item
                if (cboSaveFiles.Items.Count > 0)
                {
                    
                    cboSaveFiles.SelectedIndex = 0;

                    // Update the save path
                    selectedSaveFile = cboSaveFiles.SelectedItem.ToString();
                    savePath = Path.Combine(savesDir, selectedSaveFile);
                    lblSavePath.Text = $"Save location: {savePath}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error loading save files: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void NavigateToStep(int step)
        {
            // Validate step index
            if (step < 0 || step >= wizardSteps.Length)
                return;

            CurrentStep = step;
            // Hide all steps
            foreach (var panel in wizardSteps)
            {
                if (panel != null)
                {
                    panel.Visible = false;
                    panelMain.Controls.Remove(panel);
                }
                
            }

            // Show the requested step
            if (wizardSteps[CurrentStep] != null)
            {
                CurrentStep = step;
                panelMain.Controls.Add(wizardSteps[CurrentStep]);
                wizardSteps[CurrentStep].Visible = true;
            }
            

            // Update navigation buttons
            btnBack.Enabled = CurrentStep > 0;

            if (CurrentStep == wizardSteps.Length - 1)
            {
                btnNext.Text = "Finish";
                UpdateSummary();
            }
            else
            {
                btnNext.Text = "Next ▶";
            }

            // Update the title
            switch (CurrentStep)
            {
                case 0:
                    lblTitle.Text = "Welcome to Factorio Save Sync Setup";
                    break;
                case 1:
                    lblTitle.Text = "Step 1: Select Save File";
                    break;
                case 2:
                    lblTitle.Text = "Step 2: Configure Google Drive";
                    CheckClipboardForDriveLink();
                    break;
                case 3:
                    lblTitle.Text = "Step 3: Configure Sync Settings";
                    break;
                case 4:
                    lblTitle.Text = "Setup Complete";
                    break;
            }
        }


        private void UpdateSummary()
        {
            // Build a summary of the configuration
            string summary = "Setup Summary:\r\n\r\n";

            // Save file info
            summary += $"• Save file: {(string.IsNullOrEmpty(selectedSaveFile) ? "Not selected" : selectedSaveFile)}\r\n";

            // Google Drive info
            if (createNewFolder)
            {
                summary += "• Google Drive: New 'FactorioSaves' folder will be created\r\n";
                summary += $"• Public sharing: {(sharePublicly ? "Enabled" : "Disabled")}\r\n";
            }
            else
            {
                summary += $"• Google Drive: Using existing folder\r\n";
            }

            // Sync settings
            string autoSyncStatus = "";
            if (chkAutoSyncOnOpen.Checked && chkAutoSyncOnClose.Checked)
                autoSyncStatus = "Enabled on open and close";
            else if (chkAutoSyncOnOpen.Checked)
                autoSyncStatus = "Enabled on open only";
            else if (chkAutoSyncOnClose.Checked)
                autoSyncStatus = "Enabled on close only";
            else
                autoSyncStatus = "Disabled";

            summary += $"• Auto-sync: {autoSyncStatus}\r\n";

            // Sync direction
            string direction = "Auto";
            if (radPreferLocal.Checked)
                direction = "Always upload local version";
            else if (radPreferDrive.Checked)
                direction = "Always download Drive version";

            summary += $"• Sync direction: {direction}";

            // Update the summary label
            lblSetupSummary.Text = summary;
        }

        public void CheckClipboardForDriveLink(bool validate = false)
        {
            if (Clipboard.ContainsText())
            {
                string copiedText = Clipboard.GetText();

                DriveLinkResult driveLinkResult = SetupWizard.IsDriveLink(copiedText);

                if (driveLinkResult != DriveLinkResult.INVALID)
                {
                    

                    radioNewFolder.Checked = false;
                    radioExistingFolder.Checked = true;

                    if (driveLinkResult == DriveLinkResult.FILE)
                    {
                        System.Diagnostics.Debug.WriteLine("Drive file link in clipboard");

                        txtFolderLink.Text = copiedText;
                        return;
                    }
                    System.Diagnostics.Debug.WriteLine("Drive folder in clipboard");
                    txtFolderLink.Text = copiedText;

                    ValidateLink();




                    return;
                }
                System.Diagnostics.Debug.WriteLine("Drive link isn't in clipboard");
                if(txtFolderLink != null)
                    txtFolderLink.Text = String.Empty;
                if(radioNewFolder != null)
                    radioNewFolder.Checked = true;
                if(radioExistingFolder != null)
                radioExistingFolder.Checked = false;



            }
        }

        public static DriveLinkResult IsDriveLink(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return DriveLinkResult.INVALID;

            // Check if it's from drive.google.com
            if (!text.StartsWith("https://drive.google.com/"))
                return DriveLinkResult.INVALID;

            // Check for file pattern
            bool isFile = text.Contains("/file/d/") &&
                         (text.Contains("/view?") || text.Contains("?usp="));

            // Check for folder pattern
            bool isFolder = text.Contains("/drive/folders/") &&
                           text.Contains("?usp=");

            // Validate ID format (typically 25+ characters)
            string idPattern = @"(file\/d\/|drive\/folders\/)([a-zA-Z0-9_-]{25,})";
            var match = Regex.Match(text, idPattern);

            bool hasValidId = match.Success;
            
            if (hasValidId)
            {
                if (isFolder)
                    return DriveLinkResult.FOLDER;
                return DriveLinkResult.FILE; 
            }
            

            return DriveLinkResult.INVALID;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            NavigateToStep(CurrentStep - 1);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            // Collect information from the current step
            switch (CurrentStep)
            {
                case 0: // Welcome
                    // Nothing to collect
                    break;

                case 1: // Save Selection
                    // Validate save selection
                    if (cboSaveFiles.SelectedItem == null || cboSaveFiles.SelectedItem.ToString() == "No save files found")
                    {
                        MessageBox.Show(
                            "Please select a save file or browse for one.",
                            "Missing Information",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return;
                    }

                    selectedSaveFile = cboSaveFiles.SelectedItem.ToString();
                    break;

                case 2: // Drive Setup
                    // Collect Google Drive settings
                    createNewFolder = radioNewFolder.Checked;
                    sharePublicly = chkSharePublic.Checked;

                    if (!createNewFolder)
                    {
                        // Validate the folder link if using existing folder
                        if (string.IsNullOrWhiteSpace(txtFolderLink.Text))
                        {
                            MessageBox.Show(
                                "Please enter a Google Drive folder link or ID.",
                                "Missing Information",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            return;
                        }

                        folderLink = txtFolderLink.Text;
                    }
                    break;

                case 3: // Sync Settings
                    // Collect sync settings
                    openAction = chkAutoSyncOnOpen.Checked ? SyncAction.Auto : SyncAction.Prompt;
                    closeAction = chkAutoSyncOnClose.Checked ? SyncAction.Auto : SyncAction.Prompt;

                    if (radPreferNewest.Checked)
                        preferredDirection = SyncDirection.Auto;
                    else if (radPreferLocal.Checked)
                        preferredDirection = SyncDirection.Upload;
                    else if (radPreferDrive.Checked)
                        preferredDirection = SyncDirection.Download;
                    break;

                case 4: // Complete
                    // Apply all settings
                    ApplySettings();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
            }

            // Move to the next step
            NavigateToStep(CurrentStep + 1);
        }

        private void btnBrowseSave_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                try
                {
                    // Set the initial directory to the Factorio saves folder
                    string savesDir = _factorioMonitor.GetSavePath();

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
                        // Get the filename
                        selectedSaveFile = Path.GetFileName(dialog.FileName);
                        savePath = dialog.FileName;

                        // Update UI
                        if (!cboSaveFiles.Items.Contains(selectedSaveFile))
                        {
                            cboSaveFiles.Items.Add(selectedSaveFile);
                        }
                        cboSaveFiles.SelectedItem = selectedSaveFile;

                        lblSavePath.Text = $"Save location: {savePath}";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            $"Error selecting save file: {ex.Message}",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void radioFolderOption_CheckedChanged(object sender, EventArgs e)
        {
            RadioChanged();
        }

        private void RadioChanged()
        {
            // Enable/disable folder link controls based on selected option
            txtFolderLink.Enabled = radioExistingFolder.Checked;
            btnValidateLink.Enabled = radioExistingFolder.Checked;
        }

        private void btnValidateLink_Click(object sender, EventArgs e)
        {
            ValidateLink();

            
        }

        private async void ValidateLink()
        {
            
            try
            {
                string text = txtFolderLink.Text;
                if(_lastCheckedLink == text)
                {
                    
                    return;
                }
                _lastCheckedLink = text;

                if (string.IsNullOrWhiteSpace(text))
                {
                    lblLinkStatus.Text = "Please enter a folder link or ID";
                    lblLinkStatus.ForeColor = Color.Red;
                    return;
                }


                // Extract folder ID
                string folderId = _googleDriveService.ExtractFolderIdFromLink(text);
                if (string.IsNullOrEmpty(folderId))
                {
                    lblLinkStatus.Text = "Invalid folder link format";
                    lblLinkStatus.ForeColor = Color.Red;
                    return;
                }

                lblLinkStatus.Text = "Validating...";
                lblLinkStatus.ForeColor = Color.Gray;

                // Check if it's valid
                var linkStatus = await _googleDriveService.CheckFolderAccessAsync(folderId);

                // Display result
                lblLinkStatus.Text = linkStatus.Message;
                lblLinkStatus.ForeColor = linkStatus.GetStatusColor();

                folderLink = txtFolderLink.Text;
            }
            catch (Exception ex)
            {
                lblLinkStatus.Text = $"Error: {ex.Message}";
                lblLinkStatus.ForeColor = Color.Red;
            }
        }

        private async void ApplySettings()
        {
            try
            {
                // Check if user is logged in to Google Drive before proceeding
                if (! await _googleDriveService.IsLoggedInFn())
                {
                    MessageBox.Show(
                        "You need to log in to your Google account before completing the setup. Please log in and try again.",
                        "Login Required",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                // 1. Set the selected save file
                _factorioMonitor.SaveFileName = selectedSaveFile;

                // 2. Set up Google Drive
                if (createNewFolder)
                {
                    // Ensure the folder exists
                    await _googleDriveService.EnsureFactorioFolderExistsAsync();

                    // If sharing is enabled, create a public link
                    if (sharePublicly)
                    {
                        string link = await _googleDriveService.CreatePublicSharingLinkAsync();
                        folderLink = link;
                    }
                }
                else
                {
                    // Set the custom folder link
                    _googleDriveService.SetCustomFolderLink(folderLink);
                }

                // 3. Configure application settings
                var appSettings = ApplicationSettings.LoadSettings();
                appSettings.OpenAction = openAction;
                appSettings.CloseAction = closeAction;
                appSettings.LastSharedFolderLink = folderLink;

                // Set the target location
                string fileName = Path.GetFileName(selectedSaveFile);
                appSettings.DriveTargetLocation = $"/FactorioSaves/{fileName}";

                // Save settings
                appSettings.SaveSettings();

                // Close the dialog with success
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error applying settings: {ex.Message}",
                    "Setup Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        public enum DriveLinkResult {
            INVALID,
            FILE,
            FOLDER

        }



    }
}
