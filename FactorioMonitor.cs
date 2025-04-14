using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace FactorioSave
{
    public class FactorioMonitor
    {
        // The name of the Factorio process
        private const string FACTORIO_PROCESS_NAME = "factorio";

        // Event that fires when Factorio closes
        public event EventHandler FactorioClosed;
        public event EventHandler FactorioOpened;


        // Flag to indicate if monitoring is active
        private string _saveFileName = "sayvGameTolyanSpAge.zip";
        private bool _isMonitoring;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isRunning;

        private static readonly string DataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "FactorioSaveSync", "sessions.json");


        private Session _currentSession;
        private GameSessionData _sessionData;

        public GameSessionData SessionData => _sessionData;

        public System.Collections.Generic.List<GameSessionData> SessionHistory { get; set; } = new System.Collections.Generic.List<GameSessionData>();

        public string SaveFileName
        {
            get { return _saveFileName; }
            set { _saveFileName = value; }
        }

        public bool isRunning
        {
            get { return _isRunning; }
        }

        // Constructor
        public FactorioMonitor()
        {
            _isMonitoring = false;
            _isRunning = IsFactorioRunning();

            LoadSessionData();

        }

        /// <summary>
        /// Records a game launch event
        /// </summary>
        public void RecordGameLaunch()
        {
            System.Diagnostics.Debug.WriteLine($"Record game launched..");
            _sessionData.LastLaunchTime = DateTime.Now;
            _sessionData.IsCurrentlyPlaying = true;
            _sessionData.CurrentSessionStart = DateTime.Now;

            _currentSession = new Session
            {
                LaunchTime = DateTime.Now,
            };

            _sessionData.CurrentSession = _currentSession;

            SaveSessionData();
        }

        /// <summary>
        /// Records a game close event and calculates the session duration
        /// </summary>
        public void RecordGameClose()
        {
            if (_sessionData.IsCurrentlyPlaying && _sessionData.CurrentSessionStart.HasValue)
            {
                _sessionData.LastCloseTime = DateTime.Now;
                _sessionData.IsCurrentlyPlaying = false;

                // Calculate session duration
                TimeSpan duration = DateTime.Now - _sessionData.CurrentSessionStart.Value;
                _sessionData.LastSessionDuration = duration;
                System.Diagnostics.Debug.WriteLine($"Record game closed, duration: {duration}");


                // Update total play time
                _sessionData.TotalPlayTime += duration;

                _sessionData.CurrentSession.CloseTime = DateTime.Now;

                _sessionData.SessionHistory.Insert(0, _sessionData.CurrentSession);

                // Reset current session 
                _sessionData.CurrentSessionStart = null;
                _sessionData.CurrentSession = null;

                SaveSessionData();
            }
        }

        /// <summary>
        /// Records a save file changed event
        /// </summary>
        public void RecordSaveFileChanged(string fileName)
        {
            System.Diagnostics.Debug.WriteLine($"Save file changed.");
            _sessionData.LastSaveFileChangeTime = DateTime.Now;
            _sessionData.LastChangedFileName = fileName;

            SaveSessionData();
        }

        /// <summary>
        /// Records a drive sync event (upload or download)
        /// </summary>
        public void RecordDriveSync(string action, string fileName, string userEmail = null)
        {
            SyncEvent syncEvent = new SyncEvent
            {
                Action = action,
                Time = DateTime.Now,
                FileName = fileName,
                UserEmail = userEmail
            };

            _sessionData.LastSyncEvent = syncEvent;

            // Add to history (maintain last 50 events)
            _sessionData.SyncHistory.Insert(0, syncEvent);
            if (_sessionData.SyncHistory.Count > 50)
            {
                _sessionData.SyncHistory.RemoveAt(_sessionData.SyncHistory.Count - 1);
            }

            SaveSessionData();
        }

        

        /// <summary>
        /// Gets formatted text about the last game session
        /// </summary>
        public string GetLastSessionInfo()
        {
            if (!_sessionData.LastLaunchTime.HasValue)
                return "No recorded game sessions";

            
            string result = $"Last played: ";


            if (_sessionData.LastSessionDuration.HasValue)
            {
                result += $"Last session duration: {FactorioMonitor.FormatTime(_sessionData.LastSessionDuration.Value)}";
            }
            return result;
        }

        public string GetLastPlayed()
        {
            if (!_sessionData.LastLaunchTime.HasValue)
                return null;
            return FactorioMonitor.FormatTime(_sessionData.LastCloseTime ?? DateTime.Now);
        }


        /// <summary>
        /// Gets current session information if the game is running
        /// </summary>
        public string GetCurrentSessionInfo()
        {
            if (!_sessionData.IsCurrentlyPlaying || !_sessionData.CurrentSessionStart.HasValue)
                return null;

            TimeSpan duration = DateTime.Now - _sessionData.CurrentSessionStart.Value;
            return $"Currently playing for {FactorioMonitor.FormatTime(duration)}";
        }

        /// <summary>
        /// Gets information about the total play time
        /// </summary>
        public string GetTotalPlayTimeInfo()
        {
            return $"Total play time: {FactorioMonitor.FormatTime(_sessionData.TotalPlayTime)}";
        }

 

        

        /// <summary>
        /// Helper method to format a TimeSpan as a human-readable duration
        /// </summary>
        public static string FormatTime(TimeSpan duration)
        {
            if (duration.TotalDays >= 1)
            {
                return $"{Math.Floor(duration.TotalDays):0} days {duration.Hours} hrs";
            }
            else if (duration.TotalHours >= 1)
            {
                return $"{Math.Floor(duration.TotalHours):0} hrs {duration.Minutes} min";
            }
            else if (duration.TotalMinutes >= 1)
            {
                return $"{Math.Floor(duration.TotalMinutes):0} min {duration.Seconds} sec";
            }
            else
            {
                return $"{duration.Seconds} seconds";
            }
        }

        /// <summary>
        /// Helper method to format a DateTime as a human-readable "time ago" string
        /// </summary>
        public static string FormatTime(DateTime time)
        {
            TimeSpan elapsed = DateTime.Now - time;

            if (elapsed.TotalDays > 30)
            {
                return time.ToString("dd MMM yyyy");
            }
            else if (elapsed.TotalDays >= 1)
            {
                int days = (int)Math.Floor(elapsed.TotalDays);
                return $"{days} day{(days > 1 ? "s" : "")} ago";
            }
            else if (elapsed.TotalHours >= 1)
            {
                int hours = (int)Math.Floor(elapsed.TotalHours);
                return $"{hours} hour{(hours > 1 ? "s" : "")} ago";
            }
            else if (elapsed.TotalMinutes >= 1)
            {
                int minutes = (int)Math.Floor(elapsed.TotalMinutes);
                return $"{minutes} minute{(minutes > 1 ? "s" : "")} ago";
            }
            else
            {
                return "Just now";
            }
        }

        /// <summary>
        /// Loads session data from disk
        /// </summary>
        private void LoadSessionData()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Loading session data..");

                if (File.Exists(DataPath))
                {
                    string json = File.ReadAllText(DataPath);
                    _sessionData = JsonConvert.DeserializeObject<GameSessionData>(json);

                    // Safety check - in case we crashed while playing
                    if (_sessionData.IsCurrentlyPlaying &&
                        _sessionData.CurrentSessionStart.HasValue &&
                        (DateTime.Now - _sessionData.CurrentSessionStart.Value).TotalHours > 24)
                    {
                        // Reset the currently playing flag if it's been more than 24 hours
                        _sessionData.IsCurrentlyPlaying = false;
                        _sessionData.CurrentSessionStart = null;
                        SaveSessionData();
                    }
                }
                else
                {
                    // Create a new session data object
                    _sessionData = new GameSessionData();

                    // Ensure the directory exists
                    string directory = Path.GetDirectoryName(DataPath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // Save the new session data
                    SaveSessionData();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading session data: {ex.Message}");
                _sessionData = new GameSessionData();
            }
        }

        /// <summary>
        /// Saves session data to disk
        /// </summary>
        private void SaveSessionData()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Saving session data..");

                string json = JsonConvert.SerializeObject(_sessionData, Formatting.Indented);
                File.WriteAllText(DataPath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving session data: {ex.Message}");
            }
        }




        // Start monitoring for Factorio process
        public void StartMonitoring()
        {
            if (_isMonitoring)
                return;

            _isMonitoring = true;
            _cancellationTokenSource = new CancellationTokenSource();

            Task.Run(() => MonitorFactorioProcess(_cancellationTokenSource.Token));
        }

        // Stop monitoring
        public void StopMonitoring()
        {
            if (!_isMonitoring)
                return;

            _cancellationTokenSource?.Cancel();
            _isMonitoring = false;
        }

        // Check if Factorio is currently running
        public bool IsFactorioRunning()
        {
            return Process.GetProcessesByName(FACTORIO_PROCESS_NAME).Length > 0;
        }

        // The main monitoring loop that runs on a background thread
        private async Task MonitorFactorioProcess(CancellationToken cancellationToken)
        {
            bool factorioWasRunning = IsFactorioRunning();

            while (!cancellationToken.IsCancellationRequested)
            {
                _isRunning = IsFactorioRunning();

                // Detect when Factorio closes
                if (factorioWasRunning && !_isRunning)
                {
                    OnFactorioClosed();
                }
                if(!factorioWasRunning && _isRunning)
                {
                    OnFactorioOpened();
                }

                factorioWasRunning = _isRunning;

                // Wait a bit before checking again
                await Task.Delay(2000, cancellationToken).ConfigureAwait(false);
            }
        }

        // Method to raise the FactorioClosed event
        private void OnFactorioClosed()
        {
            this.RecordGameClose();
            FactorioClosed?.Invoke(this, EventArgs.Empty);
        }
        
        private void OnFactorioOpened()
        {
            this.RecordGameLaunch();
            FactorioOpened?.Invoke(this, EventArgs.Empty);
        }

        public string GetFactorioSavesDirectory()
        {
            string savePath;

            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    savePath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        "Factorio", "saves", _saveFileName);
                    break;

                case PlatformID.Unix:
                    savePath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                        ".factorio", "saves", _saveFileName);
                    break;

                default:
                    throw new PlatformNotSupportedException("Your OS is not supported.");
            }

            return savePath;
        }


    }

    public class Session
    {
        public DateTime? LaunchTime { get; set; }

        public DateTime? CloseTime { get; set; }


    }

    public class GameSessionData
    {
        
        public Session CurrentSession { get; set; }
        public DateTime? LastLaunchTime { get; set; }
        public DateTime? LastCloseTime { get; set; }
        public TimeSpan? LastSessionDuration { get; set; }
        public TimeSpan TotalPlayTime { get; set; } = TimeSpan.Zero;
        public bool IsCurrentlyPlaying { get; set; }
        public DateTime? CurrentSessionStart { get; set; }
        public DateTime? LastSaveFileChangeTime { get; set; }
        public string LastChangedFileName { get; set; }
        public SyncEvent LastSyncEvent { get; set; }
        public System.Collections.Generic.List<SyncEvent> SyncHistory { get; set; } = new System.Collections.Generic.List<SyncEvent>();

        public System.Collections.Generic.List<Session> SessionHistory { get; set; } = new System.Collections.Generic.List<Session>();
    }
}