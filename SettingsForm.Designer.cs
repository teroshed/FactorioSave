namespace FactorioSave
{
    partial class SettingsForm
    {

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblOpenAction;
        private System.Windows.Forms.Label lblCloseAction;
        private System.Windows.Forms.ComboBox cboOpenAction;
        private System.Windows.Forms.ComboBox cboCloseAction;
        private System.Windows.Forms.Label lblFolderLink;
        private System.Windows.Forms.LinkLabel lnkFolderLink;
        private System.Windows.Forms.Button btnEditLink;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRestoreDefaults;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelLink;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer settingsComponents = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (settingsComponents != null))
            {
                settingsComponents.Dispose();
            }
            base.Dispose(disposing);
        }



        private void InitializeComponent()
        {
            this.settingsComponents = new System.ComponentModel.Container();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblOpenAction = new System.Windows.Forms.Label();
            this.lblCloseAction = new System.Windows.Forms.Label();
            this.cboOpenAction = new System.Windows.Forms.ComboBox();
            this.cboCloseAction = new System.Windows.Forms.ComboBox();
            this.lblFolderLink = new System.Windows.Forms.Label();
            this.lnkFolderLink = new System.Windows.Forms.LinkLabel();
            this.btnEditLink = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRestoreDefaults = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelLink = new System.Windows.Forms.Panel();

            // 
            // panel1 - Header
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(41, 50, 65);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(450, 60);
            this.panel1.TabIndex = 0;

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.FromArgb(41, 50, 65);
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(155, 25);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Sync Settings";

            // 
            // lblOpenAction
            // 
            this.lblOpenAction.AutoSize = true;
            this.lblOpenAction.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblOpenAction.Location = new System.Drawing.Point(20, 80);
            this.lblOpenAction.Name = "lblOpenAction";
            this.lblOpenAction.Size = new System.Drawing.Size(194, 19);
            this.lblOpenAction.TabIndex = 2;
            this.lblOpenAction.Text = "When Factorio starts:";

            // 
            // lblCloseAction
            // 
            this.lblCloseAction.AutoSize = true;
            this.lblCloseAction.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCloseAction.Location = new System.Drawing.Point(20, 120);
            this.lblCloseAction.Name = "lblCloseAction";
            this.lblCloseAction.Size = new System.Drawing.Size(194, 19);
            this.lblCloseAction.TabIndex = 3;
            this.lblCloseAction.Text = "When Factorio closes:";

            // 
            // cboOpenAction
            // 
            this.cboOpenAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOpenAction.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboOpenAction.FormattingEnabled = true;
            this.cboOpenAction.Items.AddRange(new object[] {
            "Do nothing",
            "Prompt to download",
            "Auto download"});
            this.cboOpenAction.Location = new System.Drawing.Point(220, 80);
            this.cboOpenAction.Name = "cboOpenAction";
            this.cboOpenAction.Size = new System.Drawing.Size(210, 23);
            this.cboOpenAction.TabIndex = 4;

            // 
            // cboCloseAction
            // 
            this.cboCloseAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCloseAction.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboCloseAction.FormattingEnabled = true;
            this.cboCloseAction.Items.AddRange(new object[] {
            "Do nothing",
            "Prompt to upload",
            "Auto upload"});
            this.cboCloseAction.Location = new System.Drawing.Point(220, 120);
            this.cboCloseAction.Name = "cboCloseAction";
            this.cboCloseAction.Size = new System.Drawing.Size(210, 23);
            this.cboCloseAction.TabIndex = 5;

            // 
            // lblFolderLink
            // 
            this.lblFolderLink.AutoSize = true;
            this.lblFolderLink.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblFolderLink.Location = new System.Drawing.Point(20, 170);
            this.lblFolderLink.Name = "lblFolderLink";
            this.lblFolderLink.Size = new System.Drawing.Size(125, 19);
            this.lblFolderLink.TabIndex = 6;
            this.lblFolderLink.Text = "Shared Folder Link:";

            // 
            // panelLink
            // 
            this.panelLink.BackColor = System.Drawing.Color.White;
            this.panelLink.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLink.Location = new System.Drawing.Point(20, 195);
            this.panelLink.Name = "panelLink";
            this.panelLink.Size = new System.Drawing.Size(345, 35);
            this.panelLink.TabIndex = 7;

            // 
            // lnkFolderLink
            // 
            this.lnkFolderLink.AutoEllipsis = true;
            this.lnkFolderLink.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lnkFolderLink.Location = new System.Drawing.Point(25, 204);
            this.lnkFolderLink.MaximumSize = new System.Drawing.Size(335, 0);
            this.lnkFolderLink.Name = "lnkFolderLink";
            this.lnkFolderLink.Size = new System.Drawing.Size(335, 17);
            this.lnkFolderLink.TabIndex = 8;
            this.lnkFolderLink.TabStop = true;
            this.lnkFolderLink.Text = "No folder link set";
            this.lnkFolderLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFolderLink_LinkClicked);

            // 
            // btnEditLink
            // 
            this.btnEditLink.BackColor = System.Drawing.Color.FromArgb(66, 139, 202);
            this.btnEditLink.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditLink.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnEditLink.ForeColor = System.Drawing.Color.White;
            this.btnEditLink.Location = new System.Drawing.Point(375, 195);
            this.btnEditLink.Name = "btnEditLink";
            this.btnEditLink.Size = new System.Drawing.Size(55, 35);
            this.btnEditLink.TabIndex = 9;
            this.btnEditLink.Text = "Edit";
            this.btnEditLink.UseVisualStyleBackColor = false;
            this.btnEditLink.Click += new System.EventHandler(this.btnEditLink_Click);

            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(92, 184, 92);
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(325, 250);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(105, 35);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // 
            // btnRestoreDefaults
            // 
            this.btnRestoreDefaults.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.btnRestoreDefaults.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestoreDefaults.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnRestoreDefaults.ForeColor = System.Drawing.Color.Black;
            this.btnRestoreDefaults.Location = new System.Drawing.Point(20, 250);
            this.btnRestoreDefaults.Name = "btnRestoreDefaults";
            this.btnRestoreDefaults.Size = new System.Drawing.Size(135, 35);
            this.btnRestoreDefaults.TabIndex = 11;
            this.btnRestoreDefaults.Text = "Restore Defaults";
            this.btnRestoreDefaults.UseVisualStyleBackColor = false;
            this.btnRestoreDefaults.Click += new System.EventHandler(this.btnRestoreDefaults_Click);

            // 
            // SettingsForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(450, 310);
            this.Controls.Add(this.btnRestoreDefaults);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnEditLink);
            this.Controls.Add(this.lnkFolderLink);
            this.Controls.Add(this.panelLink);
            this.Controls.Add(this.lblFolderLink);
            this.Controls.Add(this.cboCloseAction);
            this.Controls.Add(this.cboOpenAction);
            this.Controls.Add(this.lblCloseAction);
            this.Controls.Add(this.lblOpenAction);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Factorio Save Sync Settings";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>


        #endregion
    }
}