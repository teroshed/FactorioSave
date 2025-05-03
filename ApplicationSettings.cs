using System;
using System.IO;
using Newtonsoft.Json;

namespace FactorioSave
{
    public enum SyncAction
    {
        None = 0,      // Do nothing
        Prompt = 1,    // Show prompt
        Auto = 2       // Automatically sync
    }
    public class ApplicationSettings
    {
        public SyncAction CloseAction { get; set; } = SyncAction.Prompt;
        public SyncAction OpenAction { get; set; } = SyncAction.Auto;


        public string DriveTargetLocation { get; set; } = string.Empty;

        // Last used folder link
        public string LastSharedFolderLink { get; set; } = string.Empty;

        public Boolean SharePublicly { get; set; } = false;
        // Settings file path
        private static readonly string SettingsFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "FactorioSaveSync", "settings.json");

        // Default constructor
        public ApplicationSettings() { }

        // Save settings to file
        public void SaveSettings()
        {
            try
            {
                // Ensure directory exists
                string directory = Path.GetDirectoryName(SettingsFilePath);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                // Serialize and save
                string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(SettingsFilePath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving settings: {ex.Message}");
            }
        }

        // Load settings from file
        public static ApplicationSettings LoadSettings()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Loading settings from {SettingsFilePath}");

                if (File.Exists(SettingsFilePath))
                {
                    string json = File.ReadAllText(SettingsFilePath);
                    ApplicationSettings test = JsonConvert.DeserializeObject<ApplicationSettings>(json);
                    System.Diagnostics.Debug.WriteLine($"Test:{test.LastSharedFolderLink}");

                    return JsonConvert.DeserializeObject<ApplicationSettings>(json) ?? new ApplicationSettings();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading settings: {ex.Message}");
            }

            return new ApplicationSettings();
        }
    }
}