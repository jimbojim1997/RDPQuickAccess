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
                RaisePropertyChaged("RDPFileSearchPath");
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
                RaisePropertyChaged("ExitOnSuccess");
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
                RaisePropertyChaged("RDPApplicationPath");
            }
        }

        public ActionCommand SaveSettingsCommand
        {
            get
            {
                return new ActionCommand(SaveSettings);
            }
        }
        #endregion

        public OptionsWindowViewModel()
        {
            LoadSettings();
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
    }
}
