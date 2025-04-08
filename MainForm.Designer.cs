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
        private Label lblStatus;
        private Panel panelHeader;
        private Panel panelSaveInfo;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // Form settings
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.Text = "Factorio Save Sync";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            // Create header panel
            this.panelHeader = new Panel();
            this.panelHeader.BackColor = Color.FromArgb(41, 50, 65);
            this.panelHeader.Dock = DockStyle.Top;
            this.panelHeader.Height = 70;

            // Create the title label
            this.lblTitle = new Label();
            this.lblTitle.Text = "Factorio Save Sync";
            this.lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new Point(20, 20);
            this.panelHeader.Controls.Add(this.lblTitle);

            // Create save info panel
            this.panelSaveInfo = new Panel();
            this.panelSaveInfo.BackColor = Color.White;
            this.panelSaveInfo.BorderStyle = BorderStyle.FixedSingle;
            this.panelSaveInfo.Size = new Size(560, 120);
            this.panelSaveInfo.Location = new Point(20, 90);

            // Create the current save label
            this.lblCurrentSave = new Label();
            this.lblCurrentSave.Text = "Current Save: None";
            this.lblCurrentSave.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.lblCurrentSave.ForeColor = Color.FromArgb(41, 50, 65);
            this.lblCurrentSave.AutoSize = true;
            this.lblCurrentSave.Location = new Point(10, 10);
            this.panelSaveInfo.Controls.Add(this.lblCurrentSave);

            // Create the last modified label
            this.lblLastModified = new Label();
            this.lblLastModified.Text = "Last Modified: --";
            this.lblLastModified.Font = new Font("Segoe UI", 10F);
            this.lblLastModified.ForeColor = Color.FromArgb(80, 80, 80);
            this.lblLastModified.AutoSize = true;
            this.lblLastModified.Location = new Point(10, 40);
            this.panelSaveInfo.Controls.Add(this.lblLastModified);

            // Create the save path label
            this.lblSavePath = new Label();
            this.lblSavePath.Text = "Save Path: --";
            this.lblSavePath.Font = new Font("Segoe UI", 9F);
            this.lblSavePath.ForeColor = Color.FromArgb(100, 100, 100);
            this.lblSavePath.AutoSize = true;
            this.lblSavePath.MaximumSize = new Size(540, 50);
            this.lblSavePath.Location = new Point(10, 70);
            this.panelSaveInfo.Controls.Add(this.lblSavePath);

            // Create the Select Save button
            this.btnSelectSaveFile = new Button();
            this.btnSelectSaveFile.Text = "Select Save File";
            this.btnSelectSaveFile.Size = new Size(180, 45);
            this.btnSelectSaveFile.Location = new Point(20, 230);
            this.btnSelectSaveFile.FlatStyle = FlatStyle.Flat;
            this.btnSelectSaveFile.BackColor = Color.FromArgb(66, 139, 202);
            this.btnSelectSaveFile.ForeColor = Color.White;
            this.btnSelectSaveFile.Font = new Font("Segoe UI", 10F);
            this.btnSelectSaveFile.Cursor = Cursors.Hand;
            this.btnSelectSaveFile.Click += new EventHandler(this.btnSelectSaveFile_Click);

            // Create the Download from Drive button
            this.btnDownloadFromDrive = new Button();
            this.btnDownloadFromDrive.Text = "Download from Drive";
            this.btnDownloadFromDrive.Size = new Size(180, 45);
            this.btnDownloadFromDrive.Location = new Point(220, 230);
            this.btnDownloadFromDrive.FlatStyle = FlatStyle.Flat;
            this.btnDownloadFromDrive.BackColor = Color.FromArgb(92, 184, 92);
            this.btnDownloadFromDrive.ForeColor = Color.White;
            this.btnDownloadFromDrive.Font = new Font("Segoe UI", 10F);
            this.btnDownloadFromDrive.Cursor = Cursors.Hand;
            this.btnDownloadFromDrive.Click += new EventHandler(this.btnDownloadFromDrive_Click);

            // Create the status label
            this.lblStatus = new Label();
            this.lblStatus.Text = "Status: Ready";
            this.lblStatus.Font = new Font("Segoe UI", 9F);
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = Color.FromArgb(80, 80, 80);
            this.lblStatus.Location = new Point(20, 350);

            // Add all controls to the form
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelSaveInfo);
            this.Controls.Add(this.btnSelectSaveFile);
            this.Controls.Add(this.btnDownloadFromDrive);
            this.Controls.Add(this.lblStatus);
        }

        #endregion
    }


}

