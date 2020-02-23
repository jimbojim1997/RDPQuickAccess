using System.Windows;

namespace RDPQuickAccess.ViewModel
{
    class OptionsWindowViewModel : ViewModelBase
    {
        #region UI Properties
        private string _RDPFileSearchPath;
        public string RDPFileSearchPath
        {
            get
            {
                return _RDPFileSearchPath;
            }
            set
            {
                _RDPFileSearchPath = value;
                RaisePropertyChaged(nameof(RDPFileSearchPath));
            }
        }

        private bool _ExitOnSuccess;
        public bool ExitOnSuccess
        {
            get
            {
                return _ExitOnSuccess;
            }
            set
            {
                _ExitOnSuccess = value;
                RaisePropertyChaged(nameof(ExitOnSuccess));
            }
        }

        private string _RDPApplicationPath;
        public string RDPApplicationPath
        {
            get
            {
                return _RDPApplicationPath;
            }
            set
            {
                _RDPApplicationPath = value;
                RaisePropertyChaged(nameof(RDPApplicationPath));
            }
        }

        private string _UriScheme;
        public string UriScheme { get
            {
                return _UriScheme;
            }
            set
            {
                _UriScheme = value;
                RaisePropertyChaged(nameof(UriScheme));
            }
        }

        public ActionCommand SaveSettingsCommand
        {
            get
            {
                return new ActionCommand(SaveSettings);
            }
        }

        public ActionCommand RegisterUriSchemeCommand
        {
            get
            {
                return new ActionCommand(RegisterUriScheme);
            }
        }
        #endregion

        public OptionsWindowViewModel()
        {
            LoadSettings();
            UriScheme = System.Configuration.ConfigurationManager.AppSettings["UriScheme"];
        }

        private void LoadSettings()
        {
            RDPFileSearchPath = app.Settings.RDPFileSearchPath;
            RDPApplicationPath = app.Settings.RDPApplicationPath;
            ExitOnSuccess = app.Settings.ExitOnSuccess;
        }

        private void SaveSettings()
        {
            app.Settings.RDPFileSearchPath = RDPFileSearchPath;
            app.Settings.RDPApplicationPath = RDPApplicationPath;
            app.Settings.ExitOnSuccess = ExitOnSuccess;
        }

        private void RegisterUriScheme()
        {
            Utilities.RegistryUtilities.RegisterUriScheme(UriScheme, "");
        }
    }
}
