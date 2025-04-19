using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FactorioSave
{
    
    public partial class SaveLocationDialog : Form
    {
        private readonly GoogleDriveService _googleDriveService;
        private string _folderLink;
        private LinkStatus _linkStatus = null;
        private Timer _validationTimer;

        public string FolderLink => _folderLink;
        public LinkStatus LinkStatus => _linkStatus;

        public SaveLocationDialog(GoogleDriveService googleDriveService, string initialLink = "")
        {
            InitializeComponent();
            _googleDriveService = googleDriveService;
            _folderLink = initialLink;


            // Set the initial folder link
            txtFolderLink.Text = initialLink;

            // Initialize the validation timer to reduce UI lag during typing
            _validationTimer = new Timer();
            _validationTimer.Interval = 800; // Validate after typing stops for 0.8 seconds
            _validationTimer.Tick += ValidationTimer_Tick;

            // Check the initial link if one was provided
            if (!string.IsNullOrEmpty(initialLink))
            {
                ValidateFolderLink();
            }
        }

        private void txtFolderLink_TextChanged(object sender, EventArgs e)
        {
            // Reset the validation timer
            _validationTimer.Stop();
            _validationTimer.Start();

            // Show validation is in progress
            progressValidation.Visible = true;
            progressValidation.MarqueeAnimationSpeed = 30;

            lblLinkStatus.Text = "Validating...";
            lblLinkStatus.ForeColor = Color.FromArgb(80, 80, 80);

            // Disable the "Use This" button until validation is complete
            btnUseThis.Enabled = false;
        }

        private async void ValidationTimer_Tick(object sender, EventArgs e)
        {
            // Stop the timer
            _validationTimer.Stop();

            // Validate the folder link
            LinkStatus status = await ValidateFolderLink();
            UpdateLinkStatusDisplay(status);
        }

        private void UpdateLinkStatusDisplay(LinkStatus status)
        {

            switch (status.Status)
            {
                case LinkAccessStatus.Empty:
                    progressValidation.Visible = false;
                    progressValidation.MarqueeAnimationSpeed = 0;

                    // Update status
                    lblLinkStatus.Text = "Enter a folder link or ID to check its accessibility";
                    lblLinkStatus.ForeColor = Color.FromArgb(80, 80, 80);

                    // Disable the "Use This" button
                    btnUseThis.Enabled = false;
                    break;
                case LinkAccessStatus.Invalid:
                    // Invalid link format
                    progressValidation.Visible = false;
                    progressValidation.MarqueeAnimationSpeed = 0;

                    lblLinkStatus.Text = "Invalid folder link or ID format";
                    lblLinkStatus.ForeColor = Color.FromArgb(217, 83, 79); // Red

                    btnUseThis.Enabled = false;
                    break;
                default:
                    // Hide the validation progress
                    progressValidation.Visible = false;
                    progressValidation.MarqueeAnimationSpeed = 0;

                    // Update the status label
                    lblLinkStatus.Text = _linkStatus.Message;
                    if (!string.IsNullOrEmpty(_linkStatus.FolderName))
                    {
                        lblLinkStatus.Text += $" ({_linkStatus.FolderName})";
                    }

                    // Set the color based on the status
                    lblLinkStatus.ForeColor = _linkStatus.GetStatusColor();

                    // Enable the "Use This" button if the folder is valid
                    btnUseThis.Enabled = _linkStatus.Status != LinkAccessStatus.Invalid;
                    break;

            }


        }
        private async Task<LinkStatus> ValidateFolderLink()
        {
            try
            {
                // Get the link from the text box
                string link = txtFolderLink.Text.Trim();

                if (string.IsNullOrEmpty(link))
                {
                    
                    return new LinkStatus
                    {
                        Status = LinkAccessStatus.Empty,
                        Message = "Link cannot be empty"
                    };
                }

                // Extract the folder ID from the link
                string folderId = _googleDriveService.ExtractFolderIdFromLink(link);

                if (string.IsNullOrEmpty(folderId))
                {
                    
                    return new LinkStatus
                    {
                        Status = LinkAccessStatus.Invalid,
                        Message = "Link is invalid"
                    };
                }

                // Check the folder's accessibility
                _linkStatus = await _googleDriveService.CheckFolderAccessAsync(folderId);

                // Save the folder link

                _folderLink = link;

                return _linkStatus;

                

            }
            catch (Exception ex)
            {
                // Hide the validation progress
                progressValidation.Visible = false;
                progressValidation.MarqueeAnimationSpeed = 0;

                // Update status with error
                lblLinkStatus.Text = $"Error validating folder: {ex.Message}";
                lblLinkStatus.ForeColor = Color.FromArgb(217, 83, 79); // Red

                // Disable the "Use This" button
                btnUseThis.Enabled = false;

                return new LinkStatus
                {
                    Status = LinkAccessStatus.Invalid,
                    Message = "Error validating link"
                };
            }
        }

        private void btnUseThis_Click(object sender, EventArgs e)
        {
            // Set the dialog result
            this.DialogResult = DialogResult.OK;
            // Close the dialog
            this.Close();
        }   

    }
}
