using System;
using System.Windows.Forms;
using System.Drawing;

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

        private Panel panelLastAction;
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
            this.btnSelectSaveFile = new System.Windows.Forms.Button();
            this.btnSearchSave = new System.Windows.Forms.Button();
            this.btnUploadToDrive = new System.Windows.Forms.Button();
            this.btnDownloadFromDrive = new System.Windows.Forms.Button();
            this.panelSharing = new System.Windows.Forms.Panel();
            this.lblSharingTitle = new System.Windows.Forms.Label();
            this.btnCopyLink = new System.Windows.Forms.Button();
            this.btnEditLink = new System.Windows.Forms.Button();
            this.btnGenerateLink = new System.Windows.Forms.Button();
            this.txtFolderUrl = new System.Windows.Forms.TextBox();
            this.btnOpenLink = new System.Windows.Forms.Button();
            this.panelGameStatus = new System.Windows.Forms.Panel();
            this.lblGameStatus = new System.Windows.Forms.Label();
            this.panelLastAction = new System.Windows.Forms.Panel();
            this.lblLastAction = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            this.panelSaveInfo.SuspendLayout();
            this.panelSharing.SuspendLayout();
            this.panelGameStatus.SuspendLayout();
            this.panelLastAction.SuspendLayout();
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
            this.panelHeader.Size = new System.Drawing.Size(700, 70);
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
            this.btnSettings.Location = new System.Drawing.Point(560, 15);
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
            this.panelSaveInfo.Location = new System.Drawing.Point(20, 90);
            this.panelSaveInfo.Name = "panelSaveInfo";
            this.panelSaveInfo.Size = new System.Drawing.Size(660, 150);
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
            this.lblLastModified.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLastModified.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblLastModified.Location = new System.Drawing.Point(10, 40);
            this.lblLastModified.Name = "lblLastModified";
            this.lblLastModified.Size = new System.Drawing.Size(189, 23);
            this.lblLastModified.TabIndex = 1;
            this.lblLastModified.Text = "Last Modified (Local): --";
            // 
            // lblDriveLastModified
            // 
            this.lblDriveLastModified.AutoSize = true;
            this.lblDriveLastModified.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDriveLastModified.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.lblDriveLastModified.Location = new System.Drawing.Point(10, 70);
            this.lblDriveLastModified.Name = "lblDriveLastModified";
            this.lblDriveLastModified.Size = new System.Drawing.Size(189, 23);
            this.lblDriveLastModified.TabIndex = 2;
            this.lblDriveLastModified.Text = "Last Modified (Drive): --";
            // 
            // lblDriveLocation
            // 
            this.lblDriveLocation.AutoSize = true;
            this.lblDriveLocation.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDriveLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.lblDriveLocation.Location = new System.Drawing.Point(10, 100);
            this.lblDriveLocation.Name = "lblDriveLocation";
            this.lblDriveLocation.Size = new System.Drawing.Size(218, 20);
            this.lblDriveLocation.TabIndex = 3;
            this.lblDriveLocation.Text = "Drive Location: Not determined";
            // 
            // lblSavePath
            // 
            this.lblSavePath.AutoSize = true;
            this.lblSavePath.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSavePath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblSavePath.Location = new System.Drawing.Point(10, 125);
            this.lblSavePath.MaximumSize = new System.Drawing.Size(640, 50);
            this.lblSavePath.Name = "lblSavePath";
            this.lblSavePath.Size = new System.Drawing.Size(91, 20);
            this.lblSavePath.TabIndex = 4;
            this.lblSavePath.Text = "Save Path: --";
            // 
            // btnSelectSaveFile
            // 
            this.btnSelectSaveFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(139)))), ((int)(((byte)(202)))));
            this.btnSelectSaveFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectSaveFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectSaveFile.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnSelectSaveFile.ForeColor = System.Drawing.Color.White;
            this.btnSelectSaveFile.Location = new System.Drawing.Point(20, 250);
            this.btnSelectSaveFile.Name = "btnSelectSaveFile";
            this.btnSelectSaveFile.Size = new System.Drawing.Size(170, 40);
            this.btnSelectSaveFile.TabIndex = 2;
            this.btnSelectSaveFile.Text = "Select Save File";
            this.btnSelectSaveFile.UseVisualStyleBackColor = false;
            this.btnSelectSaveFile.Click += new System.EventHandler(this.btnSelectSaveFile_Click);
            // 
            // btnSearchSave
            // 
            this.btnSearchSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(173)))), ((int)(((byte)(78)))));
            this.btnSearchSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearchSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchSave.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnSearchSave.ForeColor = System.Drawing.Color.White;
            this.btnSearchSave.Location = new System.Drawing.Point(20, 340);
            this.btnSearchSave.Name = "btnSearchSave";
            this.btnSearchSave.Size = new System.Drawing.Size(170, 40);
            this.btnSearchSave.TabIndex = 3;
            this.btnSearchSave.Text = "Search on Drive";
            this.btnSearchSave.UseVisualStyleBackColor = false;
            this.btnSearchSave.Click += new System.EventHandler(this.btnSearchSave_Click);
            // 
            // btnUploadToDrive
            // 
            this.btnUploadToDrive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(83)))), ((int)(((byte)(79)))));
            this.btnUploadToDrive.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUploadToDrive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUploadToDrive.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUploadToDrive.ForeColor = System.Drawing.Color.White;
            this.btnUploadToDrive.Location = new System.Drawing.Point(20, 295);
            this.btnUploadToDrive.Name = "btnUploadToDrive";
            this.btnUploadToDrive.Size = new System.Drawing.Size(170, 40);
            this.btnUploadToDrive.TabIndex = 4;
            this.btnUploadToDrive.Text = "Upload to Drive";
            this.btnUploadToDrive.UseVisualStyleBackColor = false;
            this.btnUploadToDrive.Click += new System.EventHandler(this.btnUploadToDrive_Click);
            // 
            // btnDownloadFromDrive
            // 
            this.btnDownloadFromDrive.BackColor = System.Drawing.Color.PaleVioletRed;
            this.btnDownloadFromDrive.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDownloadFromDrive.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDownloadFromDrive.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnDownloadFromDrive.ForeColor = System.Drawing.Color.White;
            this.btnDownloadFromDrive.Location = new System.Drawing.Point(20, 385);
            this.btnDownloadFromDrive.Name = "btnDownloadFromDrive";
            this.btnDownloadFromDrive.Size = new System.Drawing.Size(170, 40);
            this.btnDownloadFromDrive.TabIndex = 5;
            this.btnDownloadFromDrive.Text = "Download from Drive";
            this.btnDownloadFromDrive.UseVisualStyleBackColor = false;
            this.btnDownloadFromDrive.Click += new System.EventHandler(this.btnDownloadFromDrive_Click);
            // 
            // panelSharing
            // 
            this.panelSharing.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.panelSharing.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSharing.Controls.Add(this.lblSharingTitle);
            this.panelSharing.Controls.Add(this.btnCopyLink);
            this.panelSharing.Controls.Add(this.btnEditLink);
            this.panelSharing.Controls.Add(this.btnGenerateLink);
            this.panelSharing.Controls.Add(this.txtFolderUrl);
            this.panelSharing.Controls.Add(this.btnOpenLink);
            this.panelSharing.Location = new System.Drawing.Point(200, 250);
            this.panelSharing.Name = "panelSharing";
            this.panelSharing.Size = new System.Drawing.Size(484, 175);
            this.panelSharing.TabIndex = 6;
            // 
            // lblSharingTitle
            // 
            this.lblSharingTitle.AutoSize = true;
            this.lblSharingTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblSharingTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(50)))), ((int)(((byte)(65)))));
            this.lblSharingTitle.Location = new System.Drawing.Point(10, 14);
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
            this.btnCopyLink.Location = new System.Drawing.Point(15, 130);
            this.btnCopyLink.Name = "btnCopyLink";
            this.btnCopyLink.Size = new System.Drawing.Size(67, 30);
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
            this.btnEditLink.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditLink.ForeColor = System.Drawing.Color.White;
            this.btnEditLink.Location = new System.Drawing.Point(330, 100);
            this.btnEditLink.Name = "btnEditLink";
            this.btnEditLink.Size = new System.Drawing.Size(130, 60);
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
            this.btnGenerateLink.Location = new System.Drawing.Point(197, 130);
            this.btnGenerateLink.Name = "btnGenerateLink";
            this.btnGenerateLink.Size = new System.Drawing.Size(127, 30);
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
            this.txtFolderUrl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtFolderUrl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.txtFolderUrl.Location = new System.Drawing.Point(11, 64);
            this.txtFolderUrl.Name = "txtFolderUrl";
            this.txtFolderUrl.ReadOnly = true;
            this.txtFolderUrl.Size = new System.Drawing.Size(400, 27);
            this.txtFolderUrl.TabIndex = 5;
            this.txtFolderUrl.Text = "No sharing link available";
            this.txtFolderUrl.Click += new System.EventHandler(this.txtFolderUrl_Click);
            // 
            // btnOpenLink
            // 
            this.btnOpenLink.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnOpenLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenLink.Enabled = false;
            this.btnOpenLink.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenLink.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnOpenLink.ForeColor = System.Drawing.Color.White;
            this.btnOpenLink.Location = new System.Drawing.Point(88, 130);
            this.btnOpenLink.Name = "btnOpenLink";
            this.btnOpenLink.Size = new System.Drawing.Size(103, 30);
            this.btnOpenLink.TabIndex = 6;
            this.btnOpenLink.Text = "🌐 Open";
            this.btnOpenLink.UseVisualStyleBackColor = false;
            this.btnOpenLink.Click += new System.EventHandler(this.btnOpenLink_Click);
            // 
            // panelGameStatus
            // 
            this.panelGameStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelGameStatus.Controls.Add(this.lblGameStatus);
            this.panelGameStatus.Location = new System.Drawing.Point(20, 430);
            this.panelGameStatus.Name = "panelGameStatus";
            this.panelGameStatus.Size = new System.Drawing.Size(325, 40);
            this.panelGameStatus.TabIndex = 7;
            // 
            // lblGameStatus
            // 
            this.lblGameStatus.AutoSize = true;
            this.lblGameStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblGameStatus.ForeColor = System.Drawing.Color.DarkRed;
            this.lblGameStatus.Location = new System.Drawing.Point(40, 10);
            this.lblGameStatus.Name = "lblGameStatus";
            this.lblGameStatus.Size = new System.Drawing.Size(186, 23);
            this.lblGameStatus.TabIndex = 0;
            this.lblGameStatus.Text = "Factorio: Not Running";
            this.lblGameStatus.Click += new System.EventHandler(this.lblGameStatus_Click);
            // 
            // panelLastAction
            // 
            this.panelLastAction.BackColor = System.Drawing.Color.White;
            this.panelLastAction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLastAction.Controls.Add(this.lblLastAction);
            this.panelLastAction.Location = new System.Drawing.Point(365, 430);
            this.panelLastAction.Name = "panelLastAction";
            this.panelLastAction.Size = new System.Drawing.Size(320, 40);
            this.panelLastAction.TabIndex = 8;
            // 
            // lblLastAction
            // 
            this.lblLastAction.AutoSize = true;
            this.lblLastAction.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLastAction.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblLastAction.Location = new System.Drawing.Point(15, 10);
            this.lblLastAction.Name = "lblLastAction";
            this.lblLastAction.Size = new System.Drawing.Size(158, 23);
            this.lblLastAction.TabIndex = 0;
            this.lblLastAction.Text = "No sync actions yet";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblStatus.Location = new System.Drawing.Point(15, 475);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(97, 20);
            this.lblStatus.TabIndex = 9;
            this.lblStatus.Text = "Status: Ready";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(700, 500);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelSaveInfo);
            this.Controls.Add(this.btnSelectSaveFile);
            this.Controls.Add(this.btnSearchSave);
            this.Controls.Add(this.btnUploadToDrive);
            this.Controls.Add(this.btnDownloadFromDrive);
            this.Controls.Add(this.panelSharing);
            this.Controls.Add(this.panelGameStatus);
            this.Controls.Add(this.panelLastAction);
            this.Controls.Add(this.lblStatus);
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
            this.panelLastAction.ResumeLayout(false);
            this.panelLastAction.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

    }


}

