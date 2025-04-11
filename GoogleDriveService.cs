using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace FactorioSave
{
    public class GoogleDriveService
    {
        // The Drive API service
        private DriveService _driveService;

        // The name of the folder in Google Drive where saves will be stored
        private const string FACTORIO_FOLDER_NAME = "FactorioSaves";

        // The ID of the Factorio saves folder in Google Drive
        private string _factorioFolderId;

        // Constructor
        public GoogleDriveService()
        {
            _driveService = null;
            _factorioFolderId = null;
        }

        /// <summary>
        /// Initializes the Google Drive service
        /// </summary>
        public async Task<bool> InitializeAsync()
        {
            try
            {
                // If already initialized, return true
                if (_driveService != null)
                    return true;

                // Define the scopes for the Drive API
                string[] scopes = { DriveService.Scope.DriveFile };

                // The ClientId and ClientSecret would be from your Google Cloud project
                // You should replace these with your actual credentials
                string appDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string credentialsPath = Path.Combine(appDirectory, "credentials.json");
                


                var clientSecrets = new ClientSecrets
                {
                    ClientId = "YOUR_CLIENT_ID_HERE",
                    ClientSecret = "YOUR_CLIENT_SECRET_HERE"
                };

                // Path where the token will be stored
                string tokenFolderPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "FactorioSaveSync");

                // Create the directory if it doesn't exist
                if (!Directory.Exists(tokenFolderPath))
                    Directory.CreateDirectory(tokenFolderPath);

                // Get user credential and save it
                var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    clientSecrets,
                    scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(tokenFolderPath, true));

                // Create the Drive API service
                _driveService = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Factorio Save Sync"
                });

                // Ensure our Factorio saves folder exists in Drive
                await EnsureFactorioFolderExistsAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing Google Drive: {ex.Message}");
                _driveService = null;
                return false;
            }
        }

        /// <summary>
        /// Makes sure the Factorio saves folder exists in Google Drive
        /// </summary>
        private async Task EnsureFactorioFolderExistsAsync()
        {
            try
            {
                // First, check if we've already cached the folder ID
                if (!string.IsNullOrEmpty(_factorioFolderId))
                    return;

                // Try to find the folder by name
                string folderId = await FindFolderIdByNameAsync(FACTORIO_FOLDER_NAME);

                // If folder doesn't exist, create it
                if (string.IsNullOrEmpty(folderId))
                {
                    folderId = await CreateFolderAsync(FACTORIO_FOLDER_NAME);
                }

                // Cache the folder ID
                _factorioFolderId = folderId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ensuring Factorio folder exists: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Finds a folder in Google Drive by its name
        /// </summary>
        private async Task<string> FindFolderIdByNameAsync(string folderName)
        {
            try
            {
                // Create the query to find folders with the specified name
                string query = $"mimeType='application/vnd.google-apps.folder' and name='{folderName}' and trashed=false";

                // Execute the query
                var request = _driveService.Files.List();
                request.Q = query;
                request.Fields = "files(id, name)";

                var result = await request.ExecuteAsync();

                // If we found matching folders
                if (result.Files != null && result.Files.Count > 0)
                {
                    // Return the first match
                    return result.Files[0].Id;
                }

                // No matching folder found
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error finding folder: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Creates a new folder in Google Drive
        /// </summary>
        private async Task<string> CreateFolderAsync(string folderName)
        {
            try
            {
                // Create metadata for a new folder
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = folderName,
                    MimeType = "application/vnd.google-apps.folder"
                };

                // Create the folder
                var request = _driveService.Files.Create(fileMetadata);
                request.Fields = "id";

                var folder = await request.ExecuteAsync();

                // Return the new folder's ID
                return folder.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating folder: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Finds a file in the Factorio folder by name
        /// </summary>
        private async Task<string> FindFileIdByNameAsync(string fileName)
        {
            try
            {
                // Make sure the folder exists
                await EnsureFactorioFolderExistsAsync();

                // Create the query to find files with the specified name in the Factorio folder
                string query = $"name='{fileName}' and '{_factorioFolderId}' in parents and trashed=false";

                // Execute the query
                var request = _driveService.Files.List();
                request.Q = query;
                request.Fields = "files(id, name)";

                var result = await request.ExecuteAsync();

                // If we found matching files
                if (result.Files != null && result.Files.Count > 0)
                {
                    // Return the first match
                    return result.Files[0].Id;
                }

                // No matching file found
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error finding file: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Uploads a save file to Google Drive
        /// </summary>
        public async Task<bool> UploadSaveAsync(string saveFilePath)
        {
            try
            {
                // Ensure the service is initialized
                if (_driveService == null)
                {
                    if (!await InitializeAsync())
                        return false;
                }

                // Make sure the folder exists
                await EnsureFactorioFolderExistsAsync();

                // Get just the file name
                string fileName = Path.GetFileName(saveFilePath);

                // Check if a file with this name already exists
                string existingFileId = await FindFileIdByNameAsync(fileName);

                // Create file metadata
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = fileName,
                    Parents = new List<string> { _factorioFolderId }
                };

                // Create the file content
                using (var stream = new System.IO.FileStream(saveFilePath, System.IO.FileMode.Open))
                {
                    // If the file exists, update it, otherwise create a new one
                    if (!string.IsNullOrEmpty(existingFileId))
                    {
                        // Update existing file
                        var updateRequest = _driveService.Files.Update(fileMetadata, existingFileId, stream, "application/zip");
                        await updateRequest.UploadAsync();
                    }
                    else
                    {
                        // Create new file
                        var createRequest = _driveService.Files.Create(fileMetadata, stream, "application/zip");
                        createRequest.Fields = "id";
                        await createRequest.UploadAsync();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading save: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Downloads a save file from Google Drive
        /// </summary>
        public async Task<bool> DownloadSaveAsync(string fileName, string saveFilePath)
        {
            try
            {
                // Ensure the service is initialized
                if (_driveService == null)
                {
                    if (!await InitializeAsync())
                        return false;
                }

                // Find the file ID
                string fileId = await FindFileIdByNameAsync(fileName);

                // If file doesn't exist in Drive
                if (string.IsNullOrEmpty(fileId))
                {
                    throw new FileNotFoundException($"Save file '{fileName}' not found in Google Drive.");
                }

                // Download the file
                var request = _driveService.Files.Get(fileId);

                // Save the file to the specified path
                using (var stream = new System.IO.FileStream(saveFilePath, System.IO.FileMode.Create))
                {
                    await request.DownloadAsync(stream);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading save: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Gets the list of save files available in Google Drive
        /// </summary>
        public async Task<List<SaveFileInfo>> GetSaveFilesListAsync()
        {
            try
            {
                // Ensure the service is initialized
                if (_driveService == null)
                {
                    if (!await InitializeAsync())
                        return new List<SaveFileInfo>();
                }

                // Make sure the folder exists
                await EnsureFactorioFolderExistsAsync();

                // Create the query to find files in the Factorio folder
                string query = $"'{_factorioFolderId}' in parents and trashed=false";

                // Execute the query
                var request = _driveService.Files.List();
                request.Q = query;
                request.Fields = "files(id, name, modifiedTime, size)";

                var result = await request.ExecuteAsync();

                // Create a list of save file info objects
                var saveFiles = new List<SaveFileInfo>();

                // If we found any files
                if (result.Files != null && result.Files.Count > 0)
                {
                    foreach (var file in result.Files)
                    {
                        saveFiles.Add(new SaveFileInfo
                        {
                            Name = file.Name,
                            Id = file.Id,
                            ModifiedTime = file.ModifiedTime,
                            Size = file.Size ?? 0
                        });
                    }
                }

                return saveFiles;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting save files list: {ex.Message}");
                return new List<SaveFileInfo>();
            }
        }
    }

    /// <summary>
    /// Class to hold information about a save file
    /// </summary>
    public class SaveFileInfo
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public long Size { get; set; }
    }
}