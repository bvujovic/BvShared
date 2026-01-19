using System.Diagnostics;

namespace Bv.Shared.Core
{
    public class MyData
    {
        public MyData(string? appName)
        {
            AppName = appName;
        }

        private string? appName;

        public string? AppName
        {
            get { return appName; }
            set
            {
                if (appName == value || string.IsNullOrWhiteSpace(value)) return;
                appName = value;
                dataFolderPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", appName);
                if (!Directory.Exists(dataFolderPath))
                    Directory.CreateDirectory(dataFolderPath);
            }
        }

        private string? dataFolderPath;

        public string ToPath(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name cannot be null or whitespace.", nameof(fileName));
            if (AppName is null)
                throw new InvalidOperationException("AppName must be set before calling ToPath.");
            return Path.Combine(dataFolderPath!, fileName);
        }
    }
}
