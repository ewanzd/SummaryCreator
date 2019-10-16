using SummaryCreator.Services;
using SummaryCreator.View;
using System;
using System.IO;
using System.Windows.Forms;

namespace SummaryCreator
{
    /// <summary>
    /// Main entry point.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Path to configuration file.
        /// </summary>
        private static string IniPath = Path.Combine(Application.StartupPath, "SummaryCreator.ini");

        /// <summary>
        /// For logging.
        /// </summary>
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        [STAThread]
        private static void Main()
        {
            Logger.Info("{0} started.", Application.ProductName);
            Logger.Info("Path to configuration file: {0}", IniPath);

            try
            {
                Startup();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
            finally
            {
                Logger.Info("{0} exited.", Application.ProductName);
            }
        }

        private static void Startup()
        {
            // configure application for windows forms
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // load configurations
            var configuration = new IniConfigurationService(IniPath);
            configuration.Reload();

            // dependency injection
            var dataService = new DataService();
            var configView = new ConfigForm();
            var configPresenter = new ConfigPresenter(configView, dataService, configuration);

            // show windows
            configView.Show();

            // run application until exit
            Application.Run();
        }
    }
}