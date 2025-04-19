using FactorioSave.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FactorioSave
{
    public partial class SettingsForm : Form
    {
        private ApplicationSettings _settings;

        private string _currentLink;

        public string CurrentLink
        {
            get { return _currentLink; }
            set { _currentLink = value; }
        }


        public SettingsForm(ApplicationSettings settings)
        {
            InitializeComponent();
            _settings = settings;
            LoadSettingsToForm();
        }



        /// <summary>
        /// Handles the Restore Defaults button click
        /// </summary>
        private void btnRestoreDefaults_Click(object sender, EventArgs e)
        {
            // Ask the user for confirmation
            DialogResult confirmResult = MessageBox.Show(
                "Are you sure you want to restore all settings to their default values?",
                "Confirm Restore Defaults",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                // Set default values for all settings
                cboOpenAction.SelectedIndex = (int)SyncAction.Auto;       // Do nothing when opening
                cboCloseAction.SelectedIndex = (int)SyncAction.Prompt;    // Prompt when closing
                _currentLink = string.Empty;                              // Clear folder link
                UpdateLinkDisplay();                                      // Update the display

                // Provide feedback to the user
                MessageBox.Show(
                    "Settings have been restored to defaults. Click Save to apply these changes.",
                    "Defaults Restored",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void LoadSettingsToForm()
        {
            // Set combo box selections based on settings
            cboCloseAction.SelectedIndex = (int)_settings.CloseAction;
            cboOpenAction.SelectedIndex = (int)_settings.OpenAction;


            // Set the folder link
            _currentLink = _settings.LastSharedFolderLink;
            System.Diagnostics.Debug.WriteLine($"Load settings to form: {_currentLink}");

            UpdateLinkDisplay();
        }

        /// <summary>
        /// Updates the link label display based on the current link value
        /// </summary>
        private void UpdateLinkDisplay()
        {
            if (string.IsNullOrEmpty(_currentLink))
            {
                lnkFolderLink.Text = "No folder link set";
                lnkFolderLink.LinkBehavior = LinkBehavior.NeverUnderline;
                lnkFolderLink.LinkColor = System.Drawing.Color.Gray;
                lnkFolderLink.Enabled = false;
            }
            else
            {
                // Show truncated link if it's too long
                if (_currentLink.Length > 70)
                {
                    lnkFolderLink.Text = _currentLink.Substring(0, 67) + "...";
                }
                else
                {
                    lnkFolderLink.Text = _currentLink;
                }

                lnkFolderLink.LinkBehavior = LinkBehavior.AlwaysUnderline;
                lnkFolderLink.LinkColor = System.Drawing.Color.FromArgb(0, 102, 204);
                lnkFolderLink.Enabled = true;
            }
        }

        /// <summary>
        /// Handles clicking on the folder link
        /// </summary>
        private void lnkFolderLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Link clicked: {_settings.LastSharedFolderLink}");

            if (!string.IsNullOrEmpty(_settings.LastSharedFolderLink))
            {
                try
                {
                    // Open the link in the default browser
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = _settings.LastSharedFolderLink,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Error opening link: {ex.Message}\n\nLink: {_settings.LastSharedFolderLink}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Handles the Edit Link button click
        /// </summary>
        private void btnEditLink_Click(object sender, EventArgs e)
        {
            using (var inputForm = new Form())
            {
                inputForm.Width = 500;
                inputForm.Height = 150;
                inputForm.Text = "Edit Shared Folder Link";
                inputForm.StartPosition = FormStartPosition.CenterParent;
                inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                inputForm.MaximizeBox = false;
                inputForm.MinimizeBox = false;

                var label = new Label
                {
                    Text = "Enter Google Drive folder link:",
                    AutoSize = true,
                    Location = new System.Drawing.Point(20, 20)
                };

                var textBox = new TextBox
                {
                    Text = _currentLink,
                    Width = 450,
                    Location = new System.Drawing.Point(20, 45)
                };

                var okButton = new Button
                {
                    Text = "OK",
                    DialogResult = DialogResult.OK,
                    Width = 75,
                    Location = new System.Drawing.Point(395, 80)
                };

                var cancelButton = new Button
                {
                    Text = "Cancel",
                    DialogResult = DialogResult.Cancel,
                    Width = 75,
                    Location = new System.Drawing.Point(310, 80)
                };

                inputForm.Controls.Add(label);
                inputForm.Controls.Add(textBox);
                inputForm.Controls.Add(okButton);
                inputForm.Controls.Add(cancelButton);

                inputForm.AcceptButton = okButton;
                inputForm.CancelButton = cancelButton;

                var result = inputForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    _currentLink = textBox.Text.Trim();
                    UpdateLinkDisplay();
                }
            }
        }



        /// <summary>
        /// Handles the Save button click
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Save settings from form controls
            _settings.CloseAction = (SyncAction)cboCloseAction.SelectedIndex;
            _settings.OpenAction = (SyncAction)cboOpenAction.SelectedIndex;
            _settings.LastSharedFolderLink = _currentLink;

            // Save to file
            _settings.SaveSettings();

            DialogResult = DialogResult.OK;
            Close();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

    }
}
