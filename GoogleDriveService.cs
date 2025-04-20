using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Upload;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Linq;

namespace FactorioSave
{

    /// <summary>
    /// Class to hold detailed information about a file's location on Google Drive
    /// </summary>
    public class DriveFileLocation
    {
        public string Path { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string ParentName { get; set; } = string.Empty;
        public string Owner { get; set; } = "Unknown";
        public DateTime? ModifiedTime { get; set; }

        public override string ToString()
        {
            return Path;
        }
    }

    public class GoogleDriveService
    {
        // The Drive API service
        private DriveService _driveService;
        private UserCredential _credential;
        private CancellationTokenSource _cancellationTokenSource;

        private bool _isLoggedIn = false;
        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { _isLoggedIn = value; }
        }

        public UserCredential Credentials 
        {
            get { return _credential; }
        }
        

        // The name of the folder in Google Drive where saves will be stored
        private const string FACTORIO_FOLDER_NAME = "FactorioSaves";

        // The ID of the Factorio saves folder in Google Drive
        private string _factorioFolderId;
        public DateTime LastSyncEvent;


        // Constructor
        public GoogleDriveService()
        {
            _driveService = null;
            _factorioFolderId = null;
            _credential = null;
            LastSyncEvent = DateTime.MinValue;
   
        }


        /// <summary>   
        /// Checks if the user has granted access to the Google Drive API   
        /// </summary>
        public async Task<bool> IsLoggedInFn()
        {
            try
            {
                // Ensure the service is initialized
                if (_driveService == null)
                {   
                    return false;
                }
                

                // Check if we can access the Drive API
                var request = _driveService.Files.List();
                request.Fields = "files(id, name)";
                var result = await request.ExecuteAsync();

                // If we get a result, we have access
                return result.Files != null && result.Files.Count > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking access: {ex.Message}");
                return false;
            }
        }

        

        public async Task<string> CreatePublicSharingLinkAsync()
        {
            try
            {
                // Ensure the service is initialized before proceeding
                if (!await IsLoggedInFn())
                {
                    return null;
                }

                // Make sure the folder exists
                await EnsureFactorioFolderExistsAsync();

                // Set up the permission for anyone with the link to view
                var permission = new Google.Apis.Drive.v3.Data.Permission
                {
                    Type = "anyone",
                    Role = "writer",
                    AllowFileDiscovery = true
                };

                // Apply the permission to the folder
                var request = _driveService.Permissions.Create(permission, _factorioFolderId);
                await request.ExecuteAsync();

                // Get the web view link for the folder
                var getRequest = _driveService.Files.Get(_factorioFolderId);
                getRequest.Fields = "webViewLink";
                var file = await getRequest.ExecuteAsync();


                return file.WebViewLink;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating sharing link: {ex.Message}");
                return null;
            }
        }


        /// Validates if a folder ID is valid
        /// </summary>
        public async Task<bool> ValidateFolderIdAsync(string folderId)
        {
            try
            {
                // Ensure the service is initialized
                if (_driveService == null)
                {   
                    return false;
                }

                // Try to get information about the folder
                var request = _driveService.Files.Get(folderId);
                request.Fields = "id, name, mimeType";

                var file = await request.ExecuteAsync();

                // Check if it's a folder
                return file.MimeType == "application/vnd.google-apps.folder";
            }
            catch
            {
                // If any exception occurs, the folder ID is invalid
                return false;
            }
        }

       





        /// Gets the current folder ID
        /// </summary>
        public string GetFolderId()
        {
            return _factorioFolderId;
        }

        /// Sets a custom folder ID for the Factorio saves folder
        /// </summary>
        public void SetCustomFolderLink(string folderId)
        {
            if (!string.IsNullOrEmpty(folderId))
            {
                // Extract just the folder ID if a full URL was provided
                if (folderId.Contains("folders/"))
                {
                    int startIndex = folderId.IndexOf("folders/") + 8;
                    int endIndex = folderId.IndexOf('?', startIndex);
                    if (endIndex == -1) endIndex = folderId.Length;
            
                    folderId = folderId.Substring(startIndex, endIndex - startIndex);
                }
        
                _factorioFolderId = folderId;
            }
        }
        //TODO: CHANGE all requestGoogleLogin to 
        /// <summary>
        /// Gets information about a specific save file from Google Drive
        /// </summary>
        public async Task<SaveFileInfo> GetSaveFileInfoAsync(string fileName)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Getting save file info, filename:{fileName}");
                // Ensure the service is initialized before proceeding
                if (!await IsLoggedInFn())
                {
                    return null;
                }

                // Find the file ID
                string fileId = await FindFileIdByNameAsync(fileName);

                // If file doesn't exist in Drive
                if (string.IsNullOrEmpty(fileId))
                {
                    return null;
                }

                // Get the file metadata
                var request = _driveService.Files.Get(fileId);
                request.Fields = "id, name, modifiedTime, size";

                var file = await request.ExecuteAsync();

                
                
                return new SaveFileInfo
                {
                    Name = file.Name,
                    Id = file.Id,
                    ModifiedTimeOffset = file.ModifiedTimeDateTimeOffset,
                    Size = file.Size ?? 0
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting file info from Drive: {ex.Message}");
                return null;
            }
        }

        /// Gets the web view link for the Factorio folder
        /// </summary>
        public async Task<string> GetFolderLinkAsync()
        {
            try
            {
                // Ensure the service is initialized
                if (!await IsLoggedInFn())
                {
                    return null;
                }

                // Make sure the folder exists
                await EnsureFactorioFolderExistsAsync();

                // Get the web view link
                var request = _driveService.Files.Get(_factorioFolderId);
                request.Fields = "webViewLink";
                var file = await request.ExecuteAsync();

                return file.WebViewLink;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting folder link: {ex.Message}");
                return null;
            }
        }



       
      

        /// <summary>
        /// Initializes the Google Drive service
        /// </summary>
        public async Task<bool> RequestGoogleLogin()
        {
            Console.WriteLine("Initializing Google Drive service...");
            try
            {
                // If already initialized, return true
                if (_driveService != null)
                    return true;

                // Define the scopes for the Drive API
                string[] scopes = { DriveService.Scope.DriveFile };
                string tokenFolderPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "FactorioSaveSync");

                // Create the directory if it doesn't exist
                if (!Directory.Exists(tokenFolderPath))
                    Directory.CreateDirectory(tokenFolderPath);

                // The ClientId and ClientSecret would be from your Google Cloud project
                // You should replace these with your actual credentials
                string appDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string projectRoot = Path.GetFullPath(Path.Combine(appDirectory, "..", ".."));
                string credentialsPath = Path.Combine(projectRoot, "client_secrets.json");
                //TODO Wrong path..
                System.Diagnostics.Debug.WriteLine("Credentials path: " + credentialsPath);

                using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
                {
                    _credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        scopes,
                        "user", CancellationToken.None, new FileDataStore(tokenFolderPath, true));
                }

                // Create the Drive API service
                _driveService = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = _credential,
                    ApplicationName = "Factorio Save Sync"
                });

                // Ensure our Factorio saves folder exists in Drive
                await EnsureFactorioFolderExistsAsync();


                System.Diagnostics.Debug.WriteLine("Here");




     


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
        public async Task EnsureFactorioFolderExistsAsync()
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
        /// Gets detailed information about a save file including who last modified it
        /// </summary>
        public async Task<FileDetails> GetFileDetailsAsync(string fileName)
        {
            try
            {
                // Ensure the service is initialized
                if (_driveService == null)
                {
                    return null;
                }

                // Find the file ID
                string fileId = await FindFileIdByNameAsync(fileName);

                // If file doesn't exist in Drive
                if (string.IsNullOrEmpty(fileId))
                {
                    return null;
                }

                // Get detailed file information
                var request = _driveService.Files.Get(fileId);
                request.Fields = "id, name, modifiedTime, lastModifyingUser, owners, size";

                var file = await request.ExecuteAsync();

                // Create result object
                var details = new FileDetails
                {
                    Id = file.Id,
                    Name = file.Name,
                    ModifiedTime = file.ModifiedTimeDateTimeOffset?.LocalDateTime,
                    Size = file.Size ?? 0
                };

                // Get last modifier if available
                if (file.LastModifyingUser != null)
                {
                    details.LastModifiedBy = file.LastModifyingUser.DisplayName;
                    details.LastModifierEmail = file.LastModifyingUser.EmailAddress;
                    details.LastModifierPhotoUrl = file.LastModifyingUser.PhotoLink;
                }

                // Get owner if available
                if (file.Owners != null && file.Owners.Count > 0)
                {
                    details.OwnerName = file.Owners[0].DisplayName;
                    details.OwnerEmail = file.Owners[0].EmailAddress;
                }

                return details;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting file details: {ex.Message}");
                return null;
            }
        }

        static void Upload_ProgressChanged(IUploadProgress progress) =>
                    System.Diagnostics.Debug.WriteLine(progress.Status + " " + progress.BytesSent);                


        static void Upload_ResponseReceived(Google.Apis.Drive.v3.Data.File file) =>
            System.Diagnostics.Debug.WriteLine(file.Name + " was uploaded successfully");

        /// <summary>
        /// Uploads a save file to Google Drive, replacing any existing file with the same name
        /// </summary>
        public async Task<bool> UploadSaveAsync(string saveFilePath)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Uploading save {saveFilePath}");

                // Ensure the service is initialized before proceeding
                if (!await IsLoggedInFn())
                {
                    return false;
                }

                // Make sure the folder exists
                await EnsureFactorioFolderExistsAsync();

                // Get just the file name
                string fileName = Path.GetFileName(saveFilePath);

                string existingFileId;

                // Check if a file with this name already exists
                
                existingFileId = await FindFileIdByNameAsync(fileName);
                using var uploadStream = System.IO.File.OpenRead(saveFilePath);

                // If a file with the same name exists, delete it first
                if (!string.IsNullOrEmpty(existingFileId))
                {
                    // File exists, update it
                    System.Diagnostics.Debug.WriteLine($"Updating existing file with ID: {existingFileId}");
            
                    // For update, we don't need to specify the Parents since the file already has a location
                    Google.Apis.Drive.v3.Data.File fileMetadata = new Google.Apis.Drive.v3.Data.File
                    {
                        Name = fileName
                    };

                    // Create the update request
                    FilesResource.UpdateMediaUpload updateRequest = _driveService.Files.Update(
                        fileMetadata, existingFileId, uploadStream, "application/zip");

                    // Add progress handlers
                    updateRequest.ProgressChanged += Upload_ProgressChanged;
                    updateRequest.ResponseReceived += (file) => 
                        System.Diagnostics.Debug.WriteLine(file.Name + " was updated successfully");

                    // Execute the update
                    await updateRequest.UploadAsync();
                }
                else
                {
                    // File doesn't exist, create it
                    Google.Apis.Drive.v3.Data.File fileMetaData = new Google.Apis.Drive.v3.Data.File
                    {
                        Name = fileName,
                        Parents = new List<string> { _factorioFolderId }
                    };

                    // Create the upload request
                    FilesResource.CreateMediaUpload insertRequest = _driveService.Files.Create(
                        fileMetaData, uploadStream, "application/zip");

                    // Add handlers
                    insertRequest.ProgressChanged += Upload_ProgressChanged;
                    insertRequest.ResponseReceived += Upload_ResponseReceived;

                    // Execute the upload
                    await insertRequest.UploadAsync();
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
                // Ensure the service is initialized before proceeding
                if (!await IsLoggedInFn())
                {
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
        /// Gets detailed information about a file's location in Google Drive including owner and modified time
        /// </summary>
        public async Task<DriveFileLocation> GetFileLocationAsync(string fileId)
        {
            try
            {
                // Ensure the service is initialized
                if (_driveService == null)
                {
                   
                    return new DriveFileLocation { Path = "Unknown location" };
                }

                // Get the file to find its parents, owner and modified time
                var fileRequest = _driveService.Files.Get(fileId);
                fileRequest.Fields = "name, parents, owners, modifiedTime";
                var file = await fileRequest.ExecuteAsync();

                // Create the result object
                var result = new DriveFileLocation
                {
                    Name = file.Name,
                    Id = fileId,
                    ModifiedTime = file.ModifiedTimeDateTimeOffset?.LocalDateTime,
                    Owner = file.Owners?.FirstOrDefault()?.DisplayName ?? "Unknown"
                };

                // If file has no parents, it's in root
                if (file.Parents == null || file.Parents.Count == 0)
                {
                    result.Path = $"/{file.Name}";
                    return result;
                }

                // Get the parent folder
                var parentId = file.Parents[0];
                var parentRequest = _driveService.Files.Get(parentId);
                parentRequest.Fields = "name";
                var parent = await parentRequest.ExecuteAsync();

                // Set the path
                result.Path = $"/{parent.Name}/{file.Name}";
                result.ParentName = parent.Name;

                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting file location: {ex.Message}");
                return new DriveFileLocation { Path = "Unknown location" };
            }
        }

        /// <summary>
        /// Checks if a folder on Google Drive is publicly accessible
        /// </summary>
        /// <param name="folderId">The ID of the folder to check</param>
        /// <returns>LinkStatus object with access information</returns>
        public async Task<LinkStatus> CheckFolderAccessAsync(string folderId)
        {
            try
            {
                // Ensure the service is initialized
                if (_driveService == null)
                {
                    return new LinkStatus { Status = LinkAccessStatus.Invalid, Message = "Drive service not initialized" };
                }

                // Try to get information about the folder
                var request = _driveService.Files.Get(folderId);
                request.Fields = "id, name, mimeType, shared, permissions, sharingUser, capabilities";

                var file = await request.ExecuteAsync();

                // Check if it's a folder
                if (file.MimeType != "application/vnd.google-apps.folder")
                {
                    return new LinkStatus
                    {
                        Status = LinkAccessStatus.Invalid,
                        Message = "Not a folder"
                    };
                }

                // Check if it's shared
                if (!file.Shared.HasValue || !file.Shared.Value)
                {
                    return new LinkStatus
                    {
                        Status = LinkAccessStatus.Private,
                        Message = "Folder is private",
                        FolderName = file.Name
                    };
                }

                // Check permissions to see if it has public access
                var permRequest = _driveService.Permissions.List(folderId);
                var permissions = await permRequest.ExecuteAsync();

                bool isPublic = false;
                bool hasWriteAccess = false;

                foreach (var permission in permissions.Permissions)
                {
                    if (permission.Type == "anyone")
                    {
                        isPublic = true;
                        if (permission.Role == "writer")
                        {
                            hasWriteAccess = true;
                        }
                    }
                }

                if (isPublic)
                {
                    return new LinkStatus
                    {
                        Status = hasWriteAccess ? LinkAccessStatus.PublicWritable : LinkAccessStatus.PublicReadOnly,
                        Message = hasWriteAccess ? "Folder is public with write access" : "Folder is public with read-only access",
                        FolderName = file.Name
                    };
                }
                else
                {
                    return new LinkStatus
                    {
                        Status = LinkAccessStatus.SharedLimited,
                        Message = "Folder is shared with specific people only",
                        FolderName = file.Name
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking folder access: {ex.Message}");
                return new LinkStatus
                {
                    Status = LinkAccessStatus.Invalid,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Extracts a folder ID from various Google Drive link formats
        /// </summary>
        /// <param name="link">The link or ID to parse</param>
        /// <returns>The extracted folder ID or null if invalid</returns>
        public string ExtractFolderIdFromLink(string link)
        {
            if (string.IsNullOrWhiteSpace(link))
                return null;

            // Trim any whitespace
            link = link.Trim();

            // Case 1: Already just the ID (alphanumeric string, typically 33 chars)
            if (link.Length >= 25 && link.Length <= 45 && !link.Contains("/") && !link.Contains("?"))
                return link;

            // Case 2: Full Drive URL with folders/ID format
            if (link.Contains("folders/"))
            {
                int startIndex = link.IndexOf("folders/") + 8;
                int endIndex = link.IndexOf('?', startIndex);
                if (endIndex == -1) endIndex = link.Length;

                return link.Substring(startIndex, endIndex - startIndex);
            }

            // Case 3: Shortened URL with ID parameter
            if (link.Contains("id="))
            {
                int startIndex = link.IndexOf("id=") + 3;
                int endIndex = link.IndexOf('&', startIndex);
                if (endIndex == -1) endIndex = link.Length;

                return link.Substring(startIndex, endIndex - startIndex);
            }

            // Invalid format
            return null;
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
                            ModifiedTimeOffset = file.ModifiedTimeDateTimeOffset,
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

        /// <summary>
        /// Ensures that the Google Drive service is initialized when needed
        /// Returns true if already initialized or initialization was successful
        /// </summary>
        

        /// <summary>
        /// Checks if the Drive service is currently initialized
        /// </summary>
        public bool IsInitialized()
        {
            return _driveService != null;
        }
    }



    /// <summary>
    /// Class to hold detailed information about a file
    /// </summary>
    public class FileDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public long Size { get; set; }
        public string LastModifiedBy { get; set; }
        public string LastModifierEmail { get; set; }
        public string LastModifierPhotoUrl { get; set; }
        public string OwnerName { get; set; }
        public string OwnerEmail { get; set; }

        /// <summary>
        /// Returns a formatted string with file details
        /// </summary>
        public override string ToString()
        {
            return $"{Name} (modified {(ModifiedTime.HasValue ? ModifiedTime.Value.ToString() : "unknown")}{(!string.IsNullOrEmpty(LastModifiedBy) ? $" by {LastModifiedBy}" : "")})";
        }
    }

    /// <summary>
    /// Class to hold information about a save file
    /// </summary>
    public class SaveFileInfo
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public DateTimeOffset? ModifiedTimeOffset { get; set; }
        public long Size { get; set; }
    }
    /// <summary>
    /// Class to hold information about a save file
    /// </summary>
    /// 
}