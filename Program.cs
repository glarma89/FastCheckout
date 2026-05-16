using Serilog;

namespace FastCheckout
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Configure Serilog first
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File(AppConfig.AppPath + "logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();


            if (!Settings.ReadSettings())
            {
                Log.Error($"Fail during settings read. Shutting down application");
                Environment.Exit(0);
                return;
            }

            Log.Information($"Settings read successfully, app starting..");
            
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new RFIDController());
        }
    }
}