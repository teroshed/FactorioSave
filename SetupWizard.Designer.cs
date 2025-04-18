using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FactorioSave
{
    partial class SetupWizard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private Panel panelMain;
        private Panel panelButtons;
        private Button btnBack;
        private Button btnNext;
        private Button btnCancel;
        private Label lblTitle;
        private PictureBox picIcon;

        // Wizard step controls
        private Panel[] wizardSteps;

        // Step 1: Welcome
        private Panel panelWelcome;
        private Label lblWelcomeTitle;
        private Label lblWelcomeText;

        // Step 2: Save Selection
        private Panel panelSaveSelection;
        private Label lblSaveSelectionTitle;
        private Label lblSaveSelectionText;
        private ComboBox cboSaveFiles;
        private Button btnBrowseSave;
        private Label lblSavePath;

        // Step 3: Drive Setup
        private Panel panelDriveSetup;
        private Label lblDriveSetupTitle;
        private Label lblDriveSetupText;
        private RadioButton radioNewFolder;
        private RadioButton radioExistingFolder;
        private TextBox txtFolderLink;
        private Button btnValidateLink;
        private Label lblLinkStatus;
        private CheckBox chkSharePublic;

        // Step 4: Sync Settings
        private Panel panelSyncSettings;
        private Label lblSyncSettingsTitle;
        private Label lblSyncSettingsText;
        private CheckBox chkAutoSyncOnOpen;
        private CheckBox chkAutoSyncOnClose;
        private RadioButton radPreferNewest;
        private RadioButton radPreferLocal;
        private RadioButton radPreferDrive;

        // Step 5: Completion
        private Panel panelComplete;
        private Label lblCompleteTitle;
        private Label lblCompleteText;
        private Label lblSetupSummary;

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


        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Text = "Factorio Save Sync Setup";
            this.ClientSize = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Main panel
            panelMain = new Panel();
            panelMain.Dock = DockStyle.Fill;
            panelMain.Padding = new Padding(20);

            // Button panel
            panelButtons = new Panel();
            panelButtons.Height = 60;
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.BackColor = Color.FromArgb(240, 240, 240);
            panelButtons.Padding = new Padding(20, 10, 20, 10);

            btnBack = new Button();
            btnBack.Text = "◀ Back";
            btnBack.Size = new Size(100, 40);
            btnBack.Location = new Point(20, 10);
            btnBack.Enabled = false;
            btnBack.Click += btnBack_Click;
            btnBack.Font = _buttonFont;

            btnNext = new Button();
            btnNext.Text = "Next ▶";
            btnNext.Size = new Size(100, 40);
            btnNext.Location = new Point(580, 10);
            btnNext.BackColor = Color.FromArgb(92, 184, 92); // Green
            btnNext.ForeColor = Color.White;
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.Click += btnNext_Click;
            btnNext.Font = _buttonFont;


            btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Size = new Size(100, 40);
            btnCancel.Location = new Point(470, 10);
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Font = _buttonFont;

            // Title and icon at the top
            lblTitle = new Label();
            lblTitle.AutoSize = false;
            lblTitle.Size = new Size(500, 30);
            lblTitle.Location = new Point(80, 20);
            lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(41, 50, 65);
            lblTitle.Text = "Welcome to Factorio Save Sync Setup";

            picIcon = new PictureBox();
            picIcon.Size = new Size(48, 48);
            picIcon.Location = new Point(20, 20);
            picIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            picIcon.Image = SystemIcons.Information.ToBitmap();

            // Add controls
            panelButtons.Controls.Add(btnBack);
            panelButtons.Controls.Add(btnNext);
            panelButtons.Controls.Add(btnCancel);

            panelMain.Controls.Add(lblTitle);
            panelMain.Controls.Add(picIcon);

            this.Controls.Add(panelMain);
            this.Controls.Add(panelButtons);

            

            this.CancelButton = btnCancel;

            InitializeWelcomeStep();
           
        }

        private void InitializeWelcomeStep()
        {


            panelWelcome = new Panel();
            panelWelcome.Dock = DockStyle.Fill;
            panelWelcome.Padding = new Padding(20, 80, 20, 20);

            lblWelcomeTitle = new Label();
            lblWelcomeTitle.AutoSize = false;
            lblWelcomeTitle.Size = new Size(600, 40);
            lblWelcomeTitle.Location = new Point(100, 20);
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
            lblWelcomeTitle.Location = new Point(10, 80);
            lblWelcomeText.Location = new Point(10, 140);
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

            //write to debug console the selcted item
            System.Diagnostics.Debug.WriteLine($"Selected item: {cboSaveFiles.SelectedItem}");


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

        private async void InitializeDriveSetupStep()
        {
            //Check if the user is logged in to Google Drive
            if (!await _googleDriveService.IsLoggedIn())
            {
                MessageBox.Show("You must be logged in to Google Drive to continue.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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

            radioNewFolder = new RadioButton();
            radioNewFolder.Text = "Create a new 'FactorioSaves' folder on my Google Drive";
            radioNewFolder.Font = new Font("Segoe UI", 11F);
            radioNewFolder.AutoSize = true;
            radioNewFolder.Checked = true;


            radioExistingFolder = new RadioButton();
            radioExistingFolder.Text = "Use an existing folder (paste Google Drive folder link or ID)";
            radioExistingFolder.Font = new Font("Segoe UI", 11F);
            radioExistingFolder.AutoSize = true;

            radioNewFolder.CheckedChanged += radioFolderOption_CheckedChanged;

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
            panelDriveSetup.Controls.Add(radioNewFolder);
            panelDriveSetup.Controls.Add(radioExistingFolder);
            panelDriveSetup.Controls.Add(txtFolderLink);
            panelDriveSetup.Controls.Add(btnValidateLink);
            panelDriveSetup.Controls.Add(lblLinkStatus);
            panelDriveSetup.Controls.Add(chkSharePublic);

            // Position controls
            lblDriveSetupTitle.Location = new Point(20, 80);
            lblDriveSetupText.Location = new Point(20, 130);
            radioNewFolder.Location = new Point(20, 190);
            radioExistingFolder.Location = new Point(20, 220);
            txtFolderLink.Location = new Point(40, 250);
            btnValidateLink.Location = new Point(500, 250);
            lblLinkStatus.Location = new Point(40, 280);
            chkSharePublic.Location = new Point(20, 310);

            CheckClipboardForDriveLink();
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

    }
}