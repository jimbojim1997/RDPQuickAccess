using RDPQuickAccess.Model;
using System.Configuration;
using System.Windows;

namespace RDPQuickAccess
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly string _settingsPath = ConfigurationManager.AppSettings["SettingsPath"];
        internal AppSettings Settings;

        private void App_Startup(object sender, StartupEventArgs e)
        {
            Settings = AppSettings.LoadFromFile(_settingsPath);
            if (Settings == null) Settings = new AppSettings();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Settings.SaveToFile(_settingsPath);
        }
    }
}
