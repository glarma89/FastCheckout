namespace FastCheckout
{
    public class AppConfig
    {
        private const string settingsFileName = "config_computer.json";
        public static string AppPath { get; } = AppContext.BaseDirectory;
        public static string SettingsFilePath
        {
            get
            {
                string configFolder = Path.Combine(AppContext.BaseDirectory, "Configuration");

                if (!Directory.Exists(configFolder))
                    return string.Empty;

                string filePath = Path.Combine(configFolder, settingsFileName);

                return File.Exists(filePath) ? filePath : string.Empty;
            }
        }

    }
}
