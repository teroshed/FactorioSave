using System.Windows.Forms;

namespace FactorioSave
{
    partial class SaveLocationDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.TextBox txtFolderLink;
        private System.Windows.Forms.Label lblLinkStatus;
        private System.Windows.Forms.Button btnUseThis;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ProgressBar progressValidation;


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
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtFolderLink = new System.Windows.Forms.TextBox();
            this.lblLinkStatus = new System.Windows.Forms.Label();
            this.btnUseThis = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.progressValidation = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(50)))), ((int)(((byte)(65)))));
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Google Drive Folder";

            // lblInstructions
            this.lblInstructions = new System.Windows.Forms.Label();
            this.lblInstructions.AutoSize = true;
            this.lblInstructions.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblInstructions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblInstructions.Location = new System.Drawing.Point(22, 60);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(400, 40);
            this.lblInstructions.TabIndex = 1;
            this.lblInstructions.Text = "Enter a Google Drive folder link or ID where your Factorio saves will be stored.\r\nThe folder must be shared publicly for others to access it.";

            // txtFolderLink
            this.txtFolderLink = new System.Windows.Forms.TextBox();
            this.txtFolderLink.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFolderLink.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtFolderLink.Location = new System.Drawing.Point(20, 110);
            this.txtFolderLink.Name = "txtFolderLink";
            this.txtFolderLink.Size = new System.Drawing.Size(450, 30);
            this.txtFolderLink.TabIndex = 2;
            this.txtFolderLink.TextChanged += txtFolderLink_TextChanged;

            // progressValidation
            this.progressValidation = new System.Windows.Forms.ProgressBar();
            this.progressValidation.Location = new System.Drawing.Point(20, 150);
            this.progressValidation.Name = "progressValidation";
            this.progressValidation.Size = new System.Drawing.Size(450, 5);
            this.progressValidation.Style = ProgressBarStyle.Marquee;
            this.progressValidation.MarqueeAnimationSpeed = 0; // Initially stopped
            this.progressValidation.Visible = false;

            // lblLinkStatus
            this.lblLinkStatus = new System.Windows.Forms.Label();
            this.lblLinkStatus.AutoSize = true;
            this.lblLinkStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblLinkStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblLinkStatus.Location = new System.Drawing.Point(20, 165);
            this.lblLinkStatus.Name = "lblLinkStatus";
            this.lblLinkStatus.Size = new System.Drawing.Size(300, 20);
            this.lblLinkStatus.TabIndex = 3;
            this.lblLinkStatus.Text = "Enter a folder link or ID to check its accessibility";

            // btnUseThis
            this.btnUseThis = new System.Windows.Forms.Button();
            this.btnUseThis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(184)))), ((int)(((byte)(92)))));
            this.btnUseThis.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUseThis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUseThis.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnUseThis.ForeColor = System.Drawing.Color.White;
            this.btnUseThis.Location = new System.Drawing.Point(350, 210);
            this.btnUseThis.Name = "btnUseThis";
            this.btnUseThis.Size = new System.Drawing.Size(120, 40);
            this.btnUseThis.TabIndex = 4;
            this.btnUseThis.Text = "Use This";
            this.btnUseThis.UseVisualStyleBackColor = false;
            this.btnUseThis.Click += btnUseThis_Click;
            this.btnUseThis.Enabled = false;

            // btnCancel
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Location = new System.Drawing.Point(220, 210);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 40);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;

            // SaveLocationDialog
            this.AcceptButton = this.btnUseThis;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(500, 270);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblInstructions);
            this.Controls.Add(this.txtFolderLink);
            this.Controls.Add(this.progressValidation);
            this.Controls.Add(this.lblLinkStatus);
            this.Controls.Add(this.btnUseThis);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SaveLocationDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Save Location";
            this.ResumeLayout(false);
            this.PerformLayout();
        }   


    }
}