using System;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;

namespace FactorioSave
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // UI Components
        private Label lblTitle;
        private Label lblCurrentSave;
        private Label lblLastModified;
        private Label lblSavePath;
        private Button btnSelectSaveFile;
        private Button btnDownloadFromDrive;
        private Button btnUploadToDrive;
        private Label lblStatus;
        private Panel panelHeader;
        private Panel panelSaveInfo;
        private Panel panelGameStatus;
        private Label lblGameStatus;
        private Button btnSettings;
        private Label lblDriveLocation;
        private Button btnSearchSave;
        private Label lblGameTimeTitle;
        private Label lblLastPlayed;
        private Label lblTotalPlayTime;
        private Button btnSync;
        private ComboBox selectDirection;
        private Label lblLinkAccessStatus;
        private Label lblLastPlayedDuration;
        private Label lblStatusResult;
        private Label lblSessionLength;


        private Label panelLastAction;
        private Label lblLastAction;
        private Label lblDriveLastModified;

        // Sharing UI components
        private Panel panelSharing;
        private Label lblSharingTitle;
        private TextBox txtFolderUrl;
        private Button btnOpenLink;
        private Button btnCopyLink;
        private Button btnEditLink;
        private Button btnGenerateLink;


        //Simplified UI 
        private Panel panelSimplified;
        private Button btnLargeSync;
        private Button btnWizard;
        private Button btnMoreDetails;
        private Label lblSimpleStatus;
        private Label lblCurrentSaveSimple;
        private Label lblLastSyncSimple;
        private Panel panelStatusBar;
        private CheckBox chkAutoSync;



        private bool _isSimplifiedMode = true;

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
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnSettings = new System.Windows.Forms.Button();
            this.panelSaveInfo = new System.Windows.Forms.Panel();
            this.lblCurrentSave = new System.Windows.Forms.Label();
            this.lblLastModified = new System.Windows.Forms.Label();
            this.lblDriveLastModified = new System.Windows.Forms.Label();
            this.lblDriveLocation = new System.Windows.Forms.Label();
            this.lblSavePath = new System.Windows.Forms.Label();
            this.lblGameTimeTitle = new System.Windows.Forms.Label();
            this.lblLastPlayed = new System.Windows.Forms.Label();
            this.lblTotalPlayTime = new System.Windows.Forms.Label();
            this.lblLastPlayedDuration = new System.Windows.Forms.Label();
            this.btnSelectSaveFile = new System.Windows.Forms.Button();
            this.btnSearchSave = new System.Windows.Forms.Button();
            this.btnUploadToDrive = new System.Windows.Forms.Button();
            this.btnDownloadFromDrive = new System.Windows.Forms.Button();
            this.panelSharing = new System.Windows.Forms.Panel();
            this.btnSync = new System.Windows.Forms.Button();
            this.btnOpenLink = new System.Windows.Forms.Button();
            this.lblSharingTitle = new System.Windows.Forms.Label();
            this.btnCopyLink = new System.Windows.Forms.Button();
            this.btnEditLink = new System.Windows.Forms.Button();
            this.btnGenerateLink = new System.Windows.Forms.Button();
            this.txtFolderUrl = new System.Windows.Forms.TextBox();
            this.lblLinkAccessStatus = new System.Windows.Forms.Label();
            this.panelGameStatus = new System.Windows.Forms.Panel();
            this.lblSessionLength = new System.Windows.Forms.Label();
            this.lblStatusResult = new System.Windows.Forms.Label();
            this.lblGameStatus = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.panelLastAction = new System.Windows.Forms.Label();
            this.lblLastAction = new System.Windows.Forms.Label();
            this.selectDirection = new System.Windows.Forms.ComboBox();
            this.panelHeader.SuspendLayout();
            this.panelSaveInfo.SuspendLayout();
            this.panelSharing.SuspendLayout();
            this.panelGameStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(50)))), ((int)(((byte)(65)))));
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Controls.Add(this.btnSettings);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(854, 70);
            this.panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(277, 41);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Factorio Save Sync";
            // 
            // btnSettings
            // 
            this.btnSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSettings.ForeColor = System.Drawing.Color.White;
            this.btnSettings.Location = new System.Drawing.Point(707, 20);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(120, 40);
            this.btnSettings.TabIndex = 1;
            this.btnSettings.Text = "⚙️ Settings";
            this.btnSettings.UseVisualStyleBackColor = false;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // panelSaveInfo
            // 
            this.panelSaveInfo.BackColor = System.Drawing.Color.White;
            this.panelSaveInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSaveInfo.Controls.Add(this.lblCurrentSave);
            this.panelSaveInfo.Controls.Add(this.lblLastModified);
            this.panelSaveInfo.Controls.Add(this.lblDriveLastModified);
            this.panelSaveInfo.Controls.Add(this.lblDriveLocation);
            this.panelSaveInfo.Controls.Add(this.lblSavePath);
            this.panelSaveInfo.Controls.Add(this.lblGameTimeTitle);
            this.panelSaveInfo.Controls.Add(this.lblLastPlayed);
            this.panelSaveInfo.Controls.Add(this.lblTotalPlayTime);
            this.panelSaveInfo.Controls.Add(this.lblLastPlayedDuration);
            this.panelSaveInfo.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelSaveInfo.Location = new System.Drawing.Point(20, 76);
            this.panelSaveInfo.Name = "panelSaveInfo";
            this.panelSaveInfo.Size = new System.Drawing.Size(812, 252);
            this.panelSaveInfo.TabIndex = 1;
            // 
            // lblCurrentSave
            // 
            this.lblCurrentSave.AutoSize = true;
            this.lblCurrentSave.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblCurrentSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(50)))), ((int)(((byte)(65)))));
            this.lblCurrentSave.Location = new System.Drawing.Point(10, 10);
            this.lblCurrentSave.Name = "lblCurrentSave";
            this.lblCurrentSave.Size = new System.Drawing.Size(186, 25);
            this.lblCurrentSave.TabIndex = 0;
            this.lblCurrentSave.Text = "Current Save: None";
            // 
            // lblLastModified
            // 
            this.lblLastModified.AutoSize = true;
            this.lblLastModified.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblLastModified.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblLastModified.Location = new System.Drawing.Point(12, 44);
            this.lblLastModified.Name = "lblLastModified";
            this.lblLastModified.Size = new System.Drawing.Size(193, 23);
            this.lblLastModified.TabIndex = 1;
            this.lblLastModified.Text = "Last Modified (Local): --";
            this.lblLastModified.Click += new System.EventHandler(this.lblLastModified_Click);
            // 
            // lblDriveLastModified
            // 
            this.lblDriveLastModified.AutoSize = true;
            this.lblDriveLastModified.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblDriveLastModified.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.lblDriveLastModified.Location = new System.Drawing.Point(12, 71);
            this.lblDriveLastModified.Name = "lblDriveLastModified";
            this.lblDriveLastModified.Size = new System.Drawing.Size(194, 23);
            this.lblDriveLastModified.TabIndex = 2;
            this.lblDriveLastModified.Text = "Last Modified (Drive): --";
            // 
            // lblDriveLocation
            // 
            this.lblDriveLocation.AutoSize = true;
            this.lblDriveLocation.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDriveLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.lblDriveLocation.Location = new System.Drawing.Point(12, 98);
            this.lblDriveLocation.Name = "lblDriveLocation";
            this.lblDriveLocation.Size = new System.Drawing.Size(251, 23);
            this.lblDriveLocation.TabIndex = 3;
            this.lblDriveLocation.Text = "Drive Location: Not determined";
            // 
            // lblSavePath
            // 
            this.lblSavePath.AutoSize = true;
            this.lblSavePath.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblSavePath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblSavePath.Location = new System.Drawing.Point(12, 125);
            this.lblSavePath.MaximumSize = new System.Drawing.Size(800, 50);
            this.lblSavePath.Name = "lblSavePath";
            this.lblSavePath.Size = new System.Drawing.Size(109, 23);
            this.lblSavePath.TabIndex = 4;
            this.lblSavePath.Text = "Save Path: --";
            this.lblSavePath.Click += new System.EventHandler(this.lblSavePath_Click);
            // 
            // lblGameTimeTitle
            // 
            this.lblGameTimeTitle.AutoSize = true;
            this.lblGameTimeTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblGameTimeTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(50)))), ((int)(((byte)(65)))));
            this.lblGameTimeTitle.Location = new System.Drawing.Point(10, 10);
            this.lblGameTimeTitle.Name = "lblGameTimeTitle";
            this.lblGameTimeTitle.Size = new System.Drawing.Size(131, 23);
            this.lblGameTimeTitle.TabIndex = 0;
            this.lblGameTimeTitle.Text = "Game Sessions:";
            // 
            // lblLastPlayed
            // 
            this.lblLastPlayed.AutoSize = true;
            this.lblLastPlayed.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblLastPlayed.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblLastPlayed.Location = new System.Drawing.Point(12, 152);
            this.lblLastPlayed.Name = "lblLastPlayed";
            this.lblLastPlayed.Size = new System.Drawing.Size(163, 23);
            this.lblLastPlayed.TabIndex = 1;
            this.lblLastPlayed.Text = "Last played: No info";
            this.lblLastPlayed.Click += new System.EventHandler(this.lblLastPlayed_Click);
            // 
            // lblTotalPlayTime
            // 
            this.lblTotalPlayTime.AutoSize = true;
            this.lblTotalPlayTime.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblTotalPlayTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblTotalPlayTime.Location = new System.Drawing.Point(12, 179);
            this.lblTotalPlayTime.Name = "lblTotalPlayTime";
            this.lblTotalPlayTime.Size = new System.Drawing.Size(216, 23);
            this.lblTotalPlayTime.TabIndex = 3;
            this.lblTotalPlayTime.Text = "Total play time: 0 hrs 0 min";
            // 
            // lblLastPlayedDuration
            // 
            this.lblLastPlayedDuration.AutoSize = true;
            this.lblLastPlayedDuration.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblLastPlayedDuration.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblLastPlayedDuration.Location = new System.Drawing.Point(12, 206);
            this.lblLastPlayedDuration.Name = "lblLastPlayedDuration";
            this.lblLastPlayedDuration.Size = new System.Drawing.Size(208, 23);
            this.lblLastPlayedDuration.TabIndex = 1;
            this.lblLastPlayedDuration.Text = "Last run duration: No info";
            this.lblLastPlayedDuration.Click += new System.EventHandler(this.lblLastPlayedDuration_Click);
            // 
            // btnSelectSaveFile
            // 
            this.btnSelectSaveFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(139)))), ((int)(((byte)(202)))));
            this.btnSelectSaveFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectSaveFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectSaveFile.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnSelectSaveFile.ForeColor = System.Drawing.Color.White;
            this.btnSelectSaveFile.Location = new System.Drawing.Point(187, 132);
            this.btnSelectSaveFile.Name = "btnSelectSaveFile";
            this.btnSelectSaveFile.Size = new System.Drawing.Size(169, 39);
            this.btnSelectSaveFile.TabIndex = 2;
            this.btnSelectSaveFile.Text = "Select Save File";
            this.btnSelectSaveFile.UseVisualStyleBackColor = false;
            this.btnSelectSaveFile.Click += new System.EventHandler(this.btnSelectSaveFile_Click);
            // 
            // btnSearchSave
            // 
            this.btnSearchSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(139)))), ((int)(((byte)(202)))));
            this.btnSearchSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearchSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchSave.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnSearchSave.ForeColor = System.Drawing.Color.White;
            this.btnSearchSave.Location = new System.Drawing.Point(11, 132);
            this.btnSearchSave.Name = "btnSearchSave";
            this.btnSearchSave.Size = new System.Drawing.Size(170, 40);
            this.btnSearchSave.TabIndex = 3;
            this.btnSearchSave.Text = "Search on Drive";
            this.btnSearchSave.UseVisualStyleBackColor = false;
            this.btnSearchSave.Click += new System.EventHandler(this.btnSearchSave_Click);

            // 
            // btnUploadToDrive
            // 
            this.btnUploadToDrive.Location = new System.Drawing.Point(0, 0);
            this.btnUploadToDrive.Name = "btnUploadToDrive";
            this.btnUploadToDrive.Size = new System.Drawing.Size(75, 23);
            this.btnUploadToDrive.TabIndex = 4;
            this.btnUploadToDrive.Visible = false;
            // 
            // btnDownloadFromDrive
            // 
            this.btnDownloadFromDrive.Location = new System.Drawing.Point(0, 0);
            this.btnDownloadFromDrive.Name = "btnDownloadFromDrive";
            this.btnDownloadFromDrive.Size = new System.Drawing.Size(75, 23);
            this.btnDownloadFromDrive.TabIndex = 5;
            this.btnDownloadFromDrive.Visible = false;

            // 
            // panelSharing
            // 
            this.panelSharing.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.panelSharing.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSharing.Controls.Add(this.btnGenerateLink);
            this.panelSharing.Controls.Add(this.btnSync);
            this.panelSharing.Controls.Add(this.btnCopyLink);
            this.panelSharing.Controls.Add(this.btnOpenLink);
            this.panelSharing.Controls.Add(this.btnSelectSaveFile);
            this.panelSharing.Controls.Add(this.lblSharingTitle);
            this.panelSharing.Controls.Add(this.btnEditLink);
            this.panelSharing.Controls.Add(this.txtFolderUrl);
            this.panelSharing.Controls.Add(this.btnSearchSave);
            this.panelSharing.Controls.Add(this.lblLinkAccessStatus);
            this.panelSharing.Location = new System.Drawing.Point(20, 366);
            this.panelSharing.Name = "panelSharing";
            this.panelSharing.Size = new System.Drawing.Size(807, 190);
            this.panelSharing.TabIndex = 6;
            // 
            // btnSync
            // 
            this.btnSync.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(105)))), ((int)(((byte)(87)))));
            this.btnSync.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSync.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSync.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnSync.ForeColor = System.Drawing.Color.White;
            this.btnSync.Location = new System.Drawing.Point(582, 86);
            this.btnSync.Name = "btnSync";
            this.btnSync.Size = new System.Drawing.Size(200, 91);
            this.btnSync.TabIndex = 11;
            this.btnSync.Text = "⟲ Sync";
            this.btnSync.UseVisualStyleBackColor = false;
            this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
            // 
            // btnOpenLink
            // 
            this.btnOpenLink.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnOpenLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenLink.Enabled = false;
            this.btnOpenLink.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenLink.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnOpenLink.ForeColor = System.Drawing.Color.White;
            this.btnOpenLink.Location = new System.Drawing.Point(171, 76);
            this.btnOpenLink.Name = "btnOpenLink";
            this.btnOpenLink.Size = new System.Drawing.Size(105, 33);
            this.btnOpenLink.TabIndex = 6;
            this.btnOpenLink.Text = "🌐 Open";
            this.btnOpenLink.UseVisualStyleBackColor = false;
            this.btnOpenLink.Visible = false;
            this.btnOpenLink.Click += new System.EventHandler(this.btnOpenLink_Click);
            // 
            // lblSharingTitle
            // 
            this.lblSharingTitle.AutoSize = true;
            this.lblSharingTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblSharingTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(50)))), ((int)(((byte)(65)))));
            this.lblSharingTitle.Location = new System.Drawing.Point(6, 6);
            this.lblSharingTitle.Name = "lblSharingTitle";
            this.lblSharingTitle.Size = new System.Drawing.Size(184, 25);
            this.lblSharingTitle.TabIndex = 0;
            this.lblSharingTitle.Text = "Share With Friends:";
            // 
            // btnCopyLink
            // 
            this.btnCopyLink.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnCopyLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCopyLink.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopyLink.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnCopyLink.ForeColor = System.Drawing.Color.Black;
            this.btnCopyLink.Location = new System.Drawing.Point(199, 3);
            this.btnCopyLink.Name = "btnCopyLink";
            this.btnCopyLink.Size = new System.Drawing.Size(105, 30);
            this.btnCopyLink.TabIndex = 2;
            this.btnCopyLink.Text = "Copy";
            this.btnCopyLink.UseVisualStyleBackColor = false;
            this.btnCopyLink.Click += new System.EventHandler(this.btnCopyLink_Click);
            // 
            // btnEditLink
            // 
            this.btnEditLink.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(184)))), ((int)(((byte)(92)))));
            this.btnEditLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditLink.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditLink.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnEditLink.ForeColor = System.Drawing.Color.White;
            this.btnEditLink.Location = new System.Drawing.Point(362, 129);
            this.btnEditLink.Name = "btnEditLink";
            this.btnEditLink.Size = new System.Drawing.Size(168, 42);
            this.btnEditLink.TabIndex = 3;
            this.btnEditLink.Text = "Edit Link";
            this.btnEditLink.UseVisualStyleBackColor = false;
            this.btnEditLink.Click += new System.EventHandler(this.btnEditLink_Click);
            // 
            // btnGenerateLink
            // 
            this.btnGenerateLink.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(139)))), ((int)(((byte)(202)))));
            this.btnGenerateLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGenerateLink.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerateLink.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnGenerateLink.ForeColor = System.Drawing.Color.White;
            this.btnGenerateLink.Location = new System.Drawing.Point(650, 34);
            this.btnGenerateLink.Name = "btnGenerateLink";
            this.btnGenerateLink.Size = new System.Drawing.Size(132, 36);
            this.btnGenerateLink.TabIndex = 4;
            this.btnGenerateLink.Text = "Generate Link";
            this.btnGenerateLink.UseVisualStyleBackColor = false;
            this.btnGenerateLink.Click += new System.EventHandler(this.btnGenerateLink_Click);
            // 
            // txtFolderUrl
            // 
            this.txtFolderUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFolderUrl.BackColor = System.Drawing.Color.White;
            this.txtFolderUrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFolderUrl.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtFolderUrl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.txtFolderUrl.Location = new System.Drawing.Point(10, 34);
            this.txtFolderUrl.Name = "txtFolderUrl";
            this.txtFolderUrl.ReadOnly = true;
            this.txtFolderUrl.Size = new System.Drawing.Size(772, 39);
            this.txtFolderUrl.TabIndex = 5;
            this.txtFolderUrl.Text = "No sharing link available";
            this.txtFolderUrl.Click += new System.EventHandler(this.txtFolderUrl_Click);
            // 
            // lblLinkAccessStatus
            // 
            this.lblLinkAccessStatus.AutoSize = true;
            this.lblLinkAccessStatus.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblLinkAccessStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblLinkAccessStatus.Location = new System.Drawing.Point(7, 76);
            this.lblLinkAccessStatus.Name = "lblLinkAccessStatus";
            this.lblLinkAccessStatus.Size = new System.Drawing.Size(158, 19);
            this.lblLinkAccessStatus.TabIndex = 13;
            this.lblLinkAccessStatus.Text = "Link status: Not checked";
            // 
            // panelGameStatus
            // 
            this.panelGameStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelGameStatus.Controls.Add(this.lblSessionLength);
            this.panelGameStatus.Controls.Add(this.lblStatusResult);
            this.panelGameStatus.Controls.Add(this.lblGameStatus);
            this.panelGameStatus.Controls.Add(this.lblStatus);
            this.panelGameStatus.Location = new System.Drawing.Point(502, 562);
            this.panelGameStatus.Name = "panelGameStatus";
            this.panelGameStatus.Size = new System.Drawing.Size(325, 70);
            this.panelGameStatus.TabIndex = 7;
            this.panelGameStatus.Paint += new System.Windows.Forms.PaintEventHandler(this.panelGameStatus_Paint);
            // 
            // lblSessionLength
            // 
            this.lblSessionLength.AutoSize = true;
            this.lblSessionLength.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSessionLength.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblSessionLength.Location = new System.Drawing.Point(12, 30);
            this.lblSessionLength.Name = "lblSessionLength";
            this.lblSessionLength.Size = new System.Drawing.Size(136, 23);
            this.lblSessionLength.TabIndex = 11;
            this.lblSessionLength.Text = "Current session:";
            this.lblSessionLength.Visible = false;
            // 
            // lblStatusResult
            // 
            this.lblStatusResult.AutoSize = true;
            this.lblStatusResult.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatusResult.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblStatusResult.Location = new System.Drawing.Point(60, 10);
            this.lblStatusResult.Name = "lblStatusResult";
            this.lblStatusResult.Size = new System.Drawing.Size(50, 20);
            this.lblStatusResult.TabIndex = 10;
            this.lblStatusResult.Text = "Ready";
            // 
            // lblGameStatus
            // 
            this.lblGameStatus.AutoSize = true;
            this.lblGameStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblGameStatus.ForeColor = System.Drawing.Color.DarkRed;
            this.lblGameStatus.Location = new System.Drawing.Point(116, 7);
            this.lblGameStatus.Name = "lblGameStatus";
            this.lblGameStatus.Size = new System.Drawing.Size(186, 23);
            this.lblGameStatus.TabIndex = 0;
            this.lblGameStatus.Text = "Factorio: Not Running";
            this.lblGameStatus.Click += new System.EventHandler(this.lblGameStatus_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblStatus.Location = new System.Drawing.Point(12, 10);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(52, 20);
            this.lblStatus.TabIndex = 9;
            this.lblStatus.Text = "Status:";
            // 
            // panelLastAction
            // 
            this.panelLastAction.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelLastAction.Location = new System.Drawing.Point(340, 496);
            this.panelLastAction.Name = "panelLastAction";
            this.panelLastAction.Size = new System.Drawing.Size(320, 40);
            this.panelLastAction.TabIndex = 8;
            // 
            // lblLastAction
            // 
            this.lblLastAction.AutoSize = true;
            this.lblLastAction.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLastAction.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblLastAction.Location = new System.Drawing.Point(12, 609);
            this.lblLastAction.Name = "lblLastAction";
            this.lblLastAction.Size = new System.Drawing.Size(158, 23);
            this.lblLastAction.TabIndex = 0;
            this.lblLastAction.Text = "No sync actions yet";
            this.lblLastAction.Click += new System.EventHandler(this.lblLastAction_Click);
            // 
            // selectDirection
            // 
            this.selectDirection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.selectDirection.Cursor = System.Windows.Forms.Cursors.Hand;
            this.selectDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectDirection.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.selectDirection.ForeColor = System.Drawing.Color.Black;
            this.selectDirection.FormattingEnabled = true;
            this.selectDirection.Items.AddRange(new object[] {
            "Upload",
            "Download",
            "Auto"});
            this.selectDirection.Location = new System.Drawing.Point(609, 480);
            this.selectDirection.Name = "selectDirection";
            this.selectDirection.Size = new System.Drawing.Size(194, 28);
            this.selectDirection.TabIndex = 12;
            this.selectDirection.Click += new System.EventHandler(this.btnSelectSync_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(854, 641);
            this.Controls.Add(this.lblLastAction);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelSaveInfo);
            this.Controls.Add(this.btnUploadToDrive);
            this.Controls.Add(this.btnDownloadFromDrive);
            this.Controls.Add(this.panelSharing);
            this.Controls.Add(this.selectDirection);
            this.Controls.Add(this.panelLastAction);
            this.Controls.Add(this.panelGameStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Factorio Save Sync";
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelSaveInfo.ResumeLayout(false);
            this.panelSaveInfo.PerformLayout();
            this.panelSharing.ResumeLayout(false);
            this.panelSharing.PerformLayout();
            this.panelGameStatus.ResumeLayout(false);
            this.panelGameStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();


            Task.Run(async () =>
            {
                TogglePanels(false);
            });



        }
        

        /// <summary>
        /// Sets the value of Visible of all panels to the value of the parameter
        /// </summary> 
        public void TogglePanels(bool visible)
        {
            this.panelHeader.Visible = visible;
            this.panelSaveInfo.Visible = visible;
            this.panelSharing.Visible = visible;
            this.panelGameStatus.Visible = visible;
            this.panelLastAction.Visible = visible;
        }
        

        
        /// <summary>
        /// Creates the simplified view components and initializes them
        /// </summary>
        private void InitializeSimplifiedView()
        {
            // Main panel for simplified view
            panelSimplified = new Panel();
            panelSimplified.Dock = DockStyle.Fill;
            panelSimplified.BackColor = Color.WhiteSmoke;
            panelSimplified.Padding = new Padding(20);

            // Large sync button
            btnLargeSync = new Button();
            btnLargeSync.Size = new Size(300, 200);
            btnLargeSync.Location = new Point((this.ClientSize.Width - 300) / 2, 120);
            btnLargeSync.Text = "⟲ SYNC";
            btnLargeSync.Font = new Font("Segoe UI", 36F, FontStyle.Bold);
            btnLargeSync.FlatStyle = FlatStyle.Flat;
            btnLargeSync.BackColor = Color.FromArgb(92, 184, 92); // Green
            btnLargeSync.ForeColor = Color.White;
            btnLargeSync.Cursor = Cursors.Hand;
            btnLargeSync.Click += btnLargeSync_Click;

            // Wizard button
            btnWizard = new Button();
            btnWizard.Size = new Size(140, 60);
            btnWizard.Location = new Point(20, 340);
            btnWizard.Text = "✨ Setup Wizard";
            btnWizard.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnWizard.FlatStyle = FlatStyle.Flat;
            btnWizard.BackColor = Color.FromArgb(66, 139, 202); // Blue
            btnWizard.ForeColor = Color.White;
            btnWizard.Cursor = Cursors.Hand;
            btnWizard.Click += btnWizard_Click;

            // More Details button
            btnMoreDetails = new Button();
            btnMoreDetails.Size = new Size(140, 60);
            btnMoreDetails.Location = new Point(this.ClientSize.Width - 160, 340);
            btnMoreDetails.Text = "More Details ▼";
            btnMoreDetails.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnMoreDetails.FlatStyle = FlatStyle.Flat;
            btnMoreDetails.BackColor = Color.FromArgb(120, 120, 120); // Gray
            btnMoreDetails.ForeColor = Color.White;
            btnMoreDetails.Cursor = Cursors.Hand;
            btnMoreDetails.Click += btnMoreDetails_Click;

            // Current save display
            lblCurrentSaveSimple = new Label();
            lblCurrentSaveSimple.AutoSize = false;
            lblCurrentSaveSimple.Size = new Size(600, 30);
            lblCurrentSaveSimple.Location = new Point((this.ClientSize.Width - 600) / 2, 80);
            lblCurrentSaveSimple.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblCurrentSaveSimple.ForeColor = Color.FromArgb(41, 50, 65);
            lblCurrentSaveSimple.Text = "No save selected";
            lblCurrentSaveSimple.TextAlign = ContentAlignment.MiddleCenter;

            // Simple status display
            lblSimpleStatus = new Label();
            lblSimpleStatus.AutoSize = false;
            lblSimpleStatus.Size = new Size(600, 30);
            lblSimpleStatus.Location = new Point((this.ClientSize.Width - 600) / 2, 330);
            lblSimpleStatus.Font = new Font("Segoe UI", 12F);
            lblSimpleStatus.ForeColor = Color.FromArgb(92, 184, 92); // Green
            lblSimpleStatus.Text = "Ready to sync";
            lblSimpleStatus.TextAlign = ContentAlignment.MiddleCenter;

            // Last sync information
            lblLastSyncSimple = new Label();
            lblLastSyncSimple.AutoSize = false;
            lblLastSyncSimple.Size = new Size(500, 25);
            lblLastSyncSimple.Location = new Point((this.ClientSize.Width - 500) / 2, 360);
            lblLastSyncSimple.Font = new Font("Segoe UI", 10F);
            lblLastSyncSimple.ForeColor = Color.FromArgb(80, 80, 80);
            lblLastSyncSimple.Text = "No sync actions yet";
            lblLastSyncSimple.TextAlign = ContentAlignment.MiddleCenter;

            // Status bar panel at the bottom
            panelStatusBar = new Panel();
            panelStatusBar.Height = 40;
            panelStatusBar.Dock = DockStyle.Bottom;
            panelStatusBar.BackColor = Color.FromArgb(41, 50, 65);

            // Auto-sync checkbox
            chkAutoSync = new CheckBox();
            chkAutoSync.Text = "Auto-sync when Factorio opens/closes";
            chkAutoSync.AutoSize = true;
            chkAutoSync.Font = new Font("Segoe UI", 9F);
            chkAutoSync.ForeColor = Color.White;
            chkAutoSync.Location = new Point(20, 10);
            chkAutoSync.Checked = _appSettings.OpenAction == SyncAction.Auto && _appSettings.CloseAction == SyncAction.Auto;
            chkAutoSync.CheckedChanged += chkAutoSync_CheckedChanged;

            // Add all components
            panelStatusBar.Controls.Add(chkAutoSync);

            panelSimplified.Controls.Add(btnLargeSync);
            panelSimplified.Controls.Add(btnWizard);
            panelSimplified.Controls.Add(btnMoreDetails);
            panelSimplified.Controls.Add(lblCurrentSaveSimple);
            panelSimplified.Controls.Add(lblSimpleStatus);
            panelSimplified.Controls.Add(lblLastSyncSimple);

            // Add panels to the form
            this.Controls.Add(panelSimplified);
            this.Controls.Add(panelStatusBar);

            // Apply rounded corners to buttons
            ApplyRoundCorners(btnLargeSync, 20);
            ApplyRoundCorners(btnWizard, 15);
            ApplyRoundCorners(btnMoreDetails, 15);

            // Initially hide the detailed panels
            ToggleViewMode(true);

            // Update display with current information
            UpdateSimplifiedView();
        }
    }




}

