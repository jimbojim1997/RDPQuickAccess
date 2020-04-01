using RDPQuickAccess.Model;
using RDPQuickAccess.Utilities;
using System;
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

        private async void App_Startup(object sender, StartupEventArgs e)
        {
            Settings = AppSettings.LoadFromFile(_settingsPath);
            if (Settings == null) Settings = new AppSettings();

            var mainWindow = new View.MainWindow();
            mainWindow.ViewModel = new ViewModel.MainWindowViewModel();

            bool exitApp = false;

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                string address = args[1];
                string uriScheme = ConfigurationManager.AppSettings["UriScheme"];
                address = address.Replace($"{uriScheme}:", "");
                OpenRdpResult openRdpResult = await RdpUtilities.OpenRdp(address, Settings.RDPFileSearchPath);

                switch (openRdpResult)
                {
                    case OpenRdpResult.Success:
                        if (Settings.ExitOnSuccess) exitApp = true;
                        break;
                    case OpenRdpResult.NotFound:
                        MessageBoxResult messageBoxResult = MessageBox.Show($"Unable to find RDPfile for '{address}'.\nOpen new session?", "Information", MessageBoxButton.YesNo);
                        if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            RdpUtilities.StartNewRDP(address);
                            if (Settings.ExitOnSuccess) exitApp = true;
                        }
                        break;
                }

                mainWindow.ViewModel.RDAddress = address;
            }

            if (exitApp) App.Current.Shutdown();
            else mainWindow.Show();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Settings.SaveToFile(_settingsPath);
        }
    }
}
