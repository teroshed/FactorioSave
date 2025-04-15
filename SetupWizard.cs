    using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FactorioSave
{
    public partial class SetupWizard : Form
    {

        private readonly GoogleDriveService _googleDriveService;
        private readonly FactorioMonitor _factorioMonitor;

       

        // Collected settings
        private string selectedSaveFile;
        private string savePath;
        private bool createNewFolder = true;
        private string folderLink;
        private bool sharePublicly = true;
        private SyncAction openAction = SyncAction.Auto;
        private SyncAction closeAction = SyncAction.Auto;
        private SyncDirection preferredDirection = SyncDirection.Auto;

        public SetupWizard(GoogleDriveService googleDriveService, FactorioMonitor factorioMonitor)
        {
            _googleDriveService = googleDriveService;
            _factorioMonitor = factorioMonitor;

            InitializeComponent();
            InitializeWizardSteps();
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

        private void InitializeWelcomeStep()
        {
            panelWelcome = new Panel();
            panelWelcome.Dock = DockStyle.Fill;
            panelWelcome.Padding = new Padding(20, 80, 20, 20);

            lblWelcomeTitle = new Label();
            lblWelcomeTitle.AutoSize = false;
            lblWelcomeTitle.Size = new Size(600, 40);
            lblWelcomeTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblWelcomeTitle.ForeColor = Color.FromArgb(41, 50, 65);
            lblWelcomeTitle.Text = "Welcome to Factorio Save Sync";
            lblWelcomeTitle.TextAlign = ContentAlignment.MiddleCenter;

            lblWelcomeText = new Label();
            lblWelcomeText.AutoSize = false;
            lblWelcomeText.Size = new Size(600, 200);
            lblWelcomeText.Font = new Font("Segoe UI", 12F);
            lblWelcomeText.ForeColor = Color.FromArgb(80, 80, 80);
            lblWelcomeText.Text =
                "This wizard will help you set up Factorio Save Sync to easily share your save files with friends.\r\n\r\n" +
                "Here's what we'll do:\r\n" +
                "• Select which Factorio save file to sync\r\n" +
                "• Set up Google Drive for sharing\r\n" +
                "• Configure sync settings\r\n\r\n" +
                "Click Next to get started!";
            lblWelcomeText.TextAlign = ContentAlignment.TopCenter;

            panelWelcome.Controls.Add(lblWelcomeTitle);
            panelWelcome.Controls.Add(lblWelcomeText);

            // Position controls
            lblWelcomeTitle.Location = new Point((panelWelcome.Width - lblWelcomeTitle.Width) / 2, 80);
            lblWelcomeText.Location = new Point((panelWelcome.Width - lblWelcomeText.Width) / 2, 140);
        }

        private void InitializeSaveSelectionStep()
        {
            panelSaveSelection = new Panel();
            panelSaveSelection.Dock = DockStyle.Fill;
            panelSaveSelection.Padding = new Padding(20, 80, 20, 20);

            lblSaveSelectionTitle = new Label();
            lblSaveSelectionTitle.AutoSize = false;
            lblSaveSelectionTitle.Size = new Size(600, 40);
            lblSaveSelectionTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblSaveSelectionTitle.ForeColor = Color.FromArgb(41, 50, 65);
            lblSaveSelectionTitle.Text = "Select Your Factorio Save File";

            lblSaveSelectionText = new Label();
            lblSaveSelectionText.AutoSize = false;
            lblSaveSelectionText.Size = new Size(600, 60);
            lblSaveSelectionText.Font = new Font("Segoe UI", 11F);
            lblSaveSelectionText.ForeColor = Color.FromArgb(80, 80, 80);
            lblSaveSelectionText.Text =
                "Choose the Factorio save file you want to sync with Google Drive. " +
                "This will be the save file that is automatically uploaded and downloaded.";

            cboSaveFiles = new ComboBox();
            cboSaveFiles.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSaveFiles.Font = new Font("Segoe UI", 12F);
            cboSaveFiles.Size = new Size(450, 30);

            btnBrowseSave = new Button();
            btnBrowseSave.Text = "Browse...";
            btnBrowseSave.Font = new Font("Segoe UI", 10F);
            btnBrowseSave.Size = new Size(100, 30);
            btnBrowseSave.Click += btnBrowseSave_Click;

            lblSavePath = new Label();
            lblSavePath.AutoSize = false;
            lblSavePath.Size = new Size(600, 60);
            lblSavePath.Font = new Font("Segoe UI", 9F);
            lblSavePath.ForeColor = Color.FromArgb(100, 100, 100);
            lblSavePath.Text = "Save location: Not selected";

            // Add controls
            panelSaveSelection.Controls.Add(lblSaveSelectionTitle);
            panelSaveSelection.Controls.Add(lblSaveSelectionText);
            panelSaveSelection.Controls.Add(cboSaveFiles);
            panelSaveSelection.Controls.Add(btnBrowseSave);
            panelSaveSelection.Controls.Add(lblSavePath);

            // Position controls
            lblSaveSelectionTitle.Location = new Point(20, 80);
            lblSaveSelectionText.Location = new Point(20, 130);
            cboSaveFiles.Location = new Point(20, 200);
            btnBrowseSave.Location = new Point(480, 200);
            lblSavePath.Location = new Point(20, 240);

            // Load existing save files
            LoadSaveFiles();
        }

        private void InitializeDriveSetupStep()
        {
            panelDriveSetup = new Panel();
            panelDriveSetup.Dock = DockStyle.Fill;
            panelDriveSetup.Padding = new Padding(20, 80, 20, 20);

            lblDriveSetupTitle = new Label();
            lblDriveSetupTitle.AutoSize = false;
            lblDriveSetupTitle.Size = new Size(600, 40);
            lblDriveSetupTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblDriveSetupTitle.ForeColor = Color.FromArgb(41, 50, 65);
            lblDriveSetupTitle.Text = "Set Up Google Drive Sharing";

            lblDriveSetupText = new Label();
            lblDriveSetupText.AutoSize = false;
            lblDriveSetupText.Size = new Size(600, 60);
            lblDriveSetupText.Font = new Font("Segoe UI", 11F);
            lblDriveSetupText.ForeColor = Color.FromArgb(80, 80, 80);
            lblDriveSetupText.Text =
                "Choose how you want to store your save files on Google Drive. " +
                "You can create a new folder or use an existing shared folder.";

            radNewFolder = new RadioButton();
            radNewFolder.Text = "Create a new 'FactorioSaves' folder on my Google Drive";
            radNewFolder.Font = new Font("Segoe UI", 11F);
            radNewFolder.AutoSize = true;
            radNewFolder.Checked = true;
            radNewFolder.CheckedChanged += radioFolderOption_CheckedChanged;

            radioExistingFolder = new RadioButton();
            radioExistingFolder.Text = "Use an existing folder (paste Google Drive folder link or ID)";
            radioExistingFolder.Font = new Font("Segoe UI", 11F);
            radioExistingFolder.AutoSize = true;
            radioExistingFolder.CheckedChanged += radioFolderOption_CheckedChanged;

            txtFolderLink = new TextBox();
            txtFolderLink.Font = new Font("Segoe UI", 11F);
            txtFolderLink.Size = new Size(450, 30);
            txtFolderLink.Enabled = false;

            btnValidateLink = new Button();
            btnValidateLink.Text = "Validate";
            btnValidateLink.Font = new Font("Segoe UI", 10F);
            btnValidateLink.Size = new Size(100, 30);
            btnValidateLink.Enabled = false;
            btnValidateLink.Click += btnValidateLink_Click;

            lblLinkStatus = new Label();
            lblLinkStatus.AutoSize = false;
            lblLinkStatus.Size = new Size(600, 30);
            lblLinkStatus.Font = new Font("Segoe UI", 9F);
            lblLinkStatus.ForeColor = Color.Gray;
            lblLinkStatus.Text = "Enter a folder link to validate";

            chkSharePublic = new CheckBox();
            chkSharePublic.Text = "Make this folder publicly accessible (allows other players to access saves)";
            chkSharePublic.Font = new Font("Segoe UI", 11F);
            chkSharePublic.AutoSize = true;
            chkSharePublic.Checked = true;

            // Add controls
            panelDriveSetup.Controls.Add(lblDriveSetupTitle);
            panelDriveSetup.Controls.Add(lblDriveSetupText);
            panelDriveSetup.Controls.Add(radNewFolder);
            panelDriveSetup.Controls.Add(radioExistingFolder);
            panelDriveSetup.Controls.Add(txtFolderLink);
            panelDriveSetup.Controls.Add(btnValidateLink);
            panelDriveSetup.Controls.Add(lblLinkStatus);
            panelDriveSetup.Controls.Add(chkSharePublic);

            // Position controls
            lblDriveSetupTitle.Location = new Point(20, 80);
            lblDriveSetupText.Location = new Point(20, 130);
            radNewFolder.Location = new Point(20, 190);
            radioExistingFolder.Location = new Point(20, 220);
            txtFolderLink.Location = new Point(40, 250);
            btnValidateLink.Location = new Point(500, 250);
            lblLinkStatus.Location = new Point(40, 280);
            chkSharePublic.Location = new Point(20, 310);
        }

        private void InitializeSyncSettingsStep()
        {
            panelSyncSettings = new Panel();
            panelSyncSettings.Dock = DockStyle.Fill;
            panelSyncSettings.Padding = new Padding(20, 80, 20, 20);

            lblSyncSettingsTitle = new Label();
            lblSyncSettingsTitle.AutoSize = false;
            lblSyncSettingsTitle.Size = new Size(600, 40);
            lblSyncSettingsTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblSyncSettingsTitle.ForeColor = Color.FromArgb(41, 50, 65);
            lblSyncSettingsTitle.Text = "Configure Sync Settings";

            lblSyncSettingsText = new Label();
            lblSyncSettingsText.AutoSize = false;
            lblSyncSettingsText.Size = new Size(600, 40);
            lblSyncSettingsText.Font = new Font("Segoe UI", 11F);
            lblSyncSettingsText.ForeColor = Color.FromArgb(80, 80, 80);
            lblSyncSettingsText.Text = "Choose how you want Factorio Save Sync to behave:";

            chkAutoSyncOnOpen = new CheckBox();
            chkAutoSyncOnOpen.Text = "Automatically download save when Factorio starts";
            chkAutoSyncOnOpen.Font = new Font("Segoe UI", 11F);
            chkAutoSyncOnOpen.AutoSize = true;
            chkAutoSyncOnOpen.Checked = true;

            chkAutoSyncOnClose = new CheckBox();
            chkAutoSyncOnClose.Text = "Automatically upload save when Factorio closes";
            chkAutoSyncOnClose.Font = new Font("Segoe UI", 11F);
            chkAutoSyncOnClose.AutoSize = true;
            chkAutoSyncOnClose.Checked = true;

            var syncDirectionLabel = new Label();
            syncDirectionLabel.Text = "When manually syncing, prefer:";
            syncDirectionLabel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            syncDirectionLabel.AutoSize = true;

            radPreferNewest = new RadioButton();
            radPreferNewest.Text = "Newest version (Smart sync)";
            radPreferNewest.Font = new Font("Segoe UI", 11F);
            radPreferNewest.AutoSize = true;
            radPreferNewest.Checked = true;

            radPreferLocal = new RadioButton();
            radPreferLocal.Text = "Local version (Always upload)";
            radPreferLocal.Font = new Font("Segoe UI", 11F);
            radPreferLocal.AutoSize = true;

            radPreferDrive = new RadioButton();
            radPreferDrive.Text = "Google Drive version (Always download)";
            radPreferDrive.Font = new Font("Segoe UI", 11F);
            radPreferDrive.AutoSize = true;

            // Add controls
            panelSyncSettings.Controls.Add(lblSyncSettingsTitle);
            panelSyncSettings.Controls.Add(lblSyncSettingsText);
            panelSyncSettings.Controls.Add(chkAutoSyncOnOpen);
            panelSyncSettings.Controls.Add(chkAutoSyncOnClose);
            panelSyncSettings.Controls.Add(syncDirectionLabel);
            panelSyncSettings.Controls.Add(radPreferNewest);
            panelSyncSettings.Controls.Add(radPreferLocal);
            panelSyncSettings.Controls.Add(radPreferDrive);

            // Position controls
            lblSyncSettingsTitle.Location = new Point(20, 80);
            lblSyncSettingsText.Location = new Point(20, 130);
            chkAutoSyncOnOpen.Location = new Point(20, 180);
            chkAutoSyncOnClose.Location = new Point(20, 210);
            syncDirectionLabel.Location = new Point(20, 250);
            radPreferNewest.Location = new Point(40, 280);
            radPreferLocal.Location = new Point(40, 310);
            radPreferDrive.Location = new Point(40, 340);
        }

        private void InitializeCompleteStep()
        {
            panelComplete = new Panel();
            panelComplete.Dock = DockStyle.Fill;
            panelComplete.Padding = new Padding(20, 80, 20, 20);

            lblCompleteTitle = new Label();
            lblCompleteTitle.AutoSize = false;
            lblCompleteTitle.Size = new Size(600, 40);
            lblCompleteTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblCompleteTitle.ForeColor = Color.FromArgb(41, 50, 65);
            lblCompleteTitle.Text = "Setup Complete!";

            lblCompleteText = new Label();
            lblCompleteText.AutoSize = false;
            lblCompleteText.Size = new Size(600, 60);
            lblCompleteText.Font = new Font("Segoe UI", 11F);
            lblCompleteText.ForeColor = Color.FromArgb(80, 80, 80);
            lblCompleteText.Text =
                "Factorio Save Sync is now configured and ready to use. " +
                "Your save files will be automatically synchronized according to your settings.";

            lblSetupSummary = new Label();
            lblSetupSummary.AutoSize = false;
            lblSetupSummary.Size = new Size(600, 200);
            lblSetupSummary.Font = new Font("Segoe UI", 10F);
            lblSetupSummary.ForeColor = Color.FromArgb(60, 60, 60);
            lblSetupSummary.Text = "Setup Summary:\r\n\r\n" +
                "• Save file: Not selected\r\n" +
                "• Google Drive: New folder will be created\r\n" +
                "• Auto-sync: Enabled on open and close\r\n" +
                "• Sync direction: Smart (newest version)";

            // Add controls
            panelComplete.Controls.Add(lblCompleteTitle);
            panelComplete.Controls.Add(lblCompleteText);
            panelComplete.Controls.Add(lblSetupSummary);

            // Position controls
            lblCompleteTitle.Location = new Point(20, 80);
            lblCompleteText.Location = new Point(20, 130);
            lblSetupSummary.Location = new Point(20, 200);
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
                foreach (string file in saveFiles)
                {
                    cboSaveFiles.Items.Add(Path.GetFileName(file));
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

            // Hide all steps
            foreach (var panel in wizardSteps)
            {
                panel.Visible = false;
                panelMain.Controls.Remove(panel);
            }

            // Show the requested step
            currentStep = step;
            panelMain.Controls.Add(wizardSteps[currentStep]);
            wizardSteps[currentStep].Visible = true;

            // Update navigation buttons
            btnBack.Enabled = currentStep > 0;

            if (currentStep == wizardSteps.Length - 1)
            {
                btnNext.Text = "Finish";
                UpdateSummary();
            }
            else
            {
                btnNext.Text = "Next ▶";
            }

            // Update the title
            switch (currentStep)
            {
                case 0:
                    lblTitle.Text = "Welcome to Factorio Save Sync Setup";
                    break;
                case 1:
                    lblTitle.Text = "Step 1: Select Save File";
                    break;
                case 2:
                    lblTitle.Text = "Step 2: Configure Google Drive";
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

        private void btnBack_Click(object sender, EventArgs e)
        {
            NavigateToStep(currentStep - 1);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            // Collect information from the current step
            switch (currentStep)
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
                    createNewFolder = radNewFolder.Checked;
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
            NavigateToStep(currentStep + 1);
        }

        private void btnBrowseSave_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                try
                {
                    // Set the initial directory to the Factorio saves folder
                    string savesDir = FactorioMonitor.GetFactorioSaveFolder();

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
            // Enable/disable folder link controls based on selected option
            txtFolderLink.Enabled = radioExistingFolder.Checked;
            btnValidateLink.Enabled = radioExistingFolder.Checked;
        }

        private async void btnValidateLink_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtFolderLink.Text))
                {
                    lblLinkStatus.Text = "Please enter a folder link or ID";
                    lblLinkStatus.ForeColor = Color.Red;
                    return;
                }

                // Extract folder ID
                string folderId = _googleDriveService.ExtractFolderIdFromLink(txtFolderLink.Text);
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



    }
}
