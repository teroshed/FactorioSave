using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace FactorioSave
{
    public class FactorioMonitor
    {
        // The name of the Factorio process
        private const string FACTORIO_PROCESS_NAME = "factorio";

        // Event that fires when Factorio closes
        public event EventHandler FactorioClosed;

        // Flag to indicate if monitoring is active
        private string _saveFileName = "sayvGameTolyanSpAge.zip";
        private bool _isMonitoring;
        private CancellationTokenSource _cancellationTokenSource;

        public string SaveFileName
        {
            get { return _saveFileName; }
            set { _saveFileName = value; }
        }

        // Constructor
        public FactorioMonitor()
        {
            _isMonitoring = false;
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
                bool factorioIsRunning = IsFactorioRunning();

                // Detect when Factorio closes
                if (factorioWasRunning && !factorioIsRunning)
                {
                    OnFactorioClosed();
                }

                factorioWasRunning = factorioIsRunning;

                // Wait a bit before checking again
                await Task.Delay(2000, cancellationToken).ConfigureAwait(false);
            }
        }

        // Method to raise the FactorioClosed event
        private void OnFactorioClosed()
        {
            FactorioClosed?.Invoke(this, EventArgs.Empty);
        }

        public string GetFactorioSavesDirectory()
        {
            string savePath;

            // Traditional switch statement for C# 7.3
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
}