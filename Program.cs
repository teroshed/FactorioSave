using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FactorioSave
{

    /// <summary>
    /// Enum representing the access status of a Google Drive folder
    /// </summary>
    public enum LinkAccessStatus
    {
        Empty,
        Invalid,
        Private,
        SharedLimited,
        PublicReadOnly,
        PublicWritable
    }

    /// <summary>
    /// Class representing the access status of a Google Drive folder
    /// </summary>
    public class LinkStatus
    {
        public LinkAccessStatus Status { get; set; }
        public string Message { get; set; }
        public string FolderName { get; set; }

        public System.Drawing.Color GetStatusColor()
        {
            switch (Status)
            {
                case LinkAccessStatus.Empty:
                    return System.Drawing.Color.FromArgb(80, 80, 80); // Gray
                case LinkAccessStatus.PublicWritable:
                    return System.Drawing.Color.FromArgb(92, 184, 92); // Green
                case LinkAccessStatus.PublicReadOnly:
                    return System.Drawing.Color.FromArgb(240, 173, 78); // Yellow
                case LinkAccessStatus.SharedLimited:
                    return System.Drawing.Color.FromArgb(91, 192, 222); // Blue
                case LinkAccessStatus.Private:
                    return System.Drawing.Color.FromArgb(217, 83, 79); // Red
                default:
                    return System.Drawing.Color.FromArgb(217, 83, 79); // Red for Invalid
            }
        }

 

    }

    /// <summary>
    /// Represents a sync event (upload or download)
    /// </summary>
    public class SyncEvent
    {
        public string Action { get; set; }
        public DateTime Time { get; set; }
        public string FileName { get; set; }
        public string UserEmail { get; set; }

        public override string ToString()
        {
            string result = $"{Action} of {FileName} at {Time:yyyy-MM-dd HH:mm:ss}";
            if (!string.IsNullOrEmpty(UserEmail))
            {
                result += $" by {UserEmail}";
            }
            return result;
        }
    }
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
