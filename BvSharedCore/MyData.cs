using System.Diagnostics;

namespace Bv.Shared.Core
{
    public static class MyData
    {
        /// <summary>Initialize MyData with application name.</summary>
        public static void Init(string? appName)
        {
            AppName = appName;
        }

        private static string? appName;

        /// <summary>Gets or sets the application name.</summary>
        public static string? AppName
        {
            get { return appName; }
            set
            {
                if (appName == value)
                    return;
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("AppName cannot be null or whitespace.");
                appName = value;
                appFolderPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
                    , appFolderLocation, appName);
                if (!Directory.Exists(appFolderPath))
                    Directory.CreateDirectory(appFolderPath);
            }
        }

        private static readonly string appFolderLocation = "OneDrive\\x\\AppData";

        private static string? appFolderPath;

        /// <summary>Gets the path for the specified file name. File should be located in app folder (OneDrive\x\AppData).</summary>
        public static string ToPath(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name cannot be null or whitespace.", nameof(fileName));
            if (AppName is null)
                throw new InvalidOperationException("AppName must be set before calling ToPath.");
            return Path.Combine(appFolderPath!, fileName);
        }

        /// <summary>Gets or sets the name of the data set file. E.g. "ds.xml"</summary>
        public static string DataSetFileName { get; set; } = "ds.xml";

        /// <summary>Gets the full file path of the data set file.</summary>
        public static string GetDataSetFilePath()
            => ToPath(DataSetFileName);
    }
}
