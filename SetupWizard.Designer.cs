using System.Drawing;
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
        private int currentStep = 0;

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
        private RadioButton radNewFolder;
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

            btnNext = new Button();
            btnNext.Text = "Next ▶";
            btnNext.Size = new Size(100, 40);
            btnNext.Location = new Point(580, 10);
            btnNext.BackColor = Color.FromArgb(92, 184, 92); // Green
            btnNext.ForeColor = Color.White;
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.Click += btnNext_Click;

            btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Size = new Size(100, 40);
            btnCancel.Location = new Point(470, 10);
            btnCancel.DialogResult = DialogResult.Cancel;

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
        }

    }
}