using RDPQuickAccess.Model;
using RDPQuickAccess.Utilities;
using RDPQuickAccess.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RDPQuickAccess.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        #region UI Bindings
        private string _RDAddress;
        public string RDAddress
        {
            get
            {
                return _RDAddress;
            }
            set
            {
                _RDAddress = value;
                RaisePropertyChaged(nameof(RDAddress));
            }
        }

        private ActionCommand _openRDPCommand;
        public ICommand OpenRDPCommand
        {
            get
            {
                return _openRDPCommand;
            }
        }

        private ActionCommand _showOptionWindowCommand;
        public ICommand ShowOptionsWindowCommand
        {
            get
            {
                return _showOptionWindowCommand;
            }
        }
        #endregion

        #region Private Properties
        private OptionsWindow OptionsWindow;
        #endregion

        public MainWindowViewModel()
        {
            _openRDPCommand = new ActionCommand(OpenRDP);
            _showOptionWindowCommand = new ActionCommand(ShowOptionsWindow);
        }

        private async void OpenRDP()
        {
            try
            {
                _openRDPCommand.Enabled = false;
                OpenRdpResult result = await RdpUtilities.OpenRdp(RDAddress, app.Settings.RDPFileSearchPath);
                switch (result)
                {
                    case OpenRdpResult.Success:
                        if (app.Settings.ExitOnSuccess) App.Current.Shutdown();
                        break;
                    case OpenRdpResult.EmptyAddress:
                        MessageBox.Show($"The address cannot be empty.", "Error");
                        break;
                    case OpenRdpResult.NotFound:
                        MessageBoxResult messageBoxResult = MessageBox.Show($"Unable to find RDPfile for '{RDAddress}'.\nOpen new session?", "Information", MessageBoxButton.YesNo);
                        if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            RdpUtilities.StartNewRDP(RDAddress);
                            if (app.Settings.ExitOnSuccess) App.Current.Shutdown();
                        }
                        break;
                }
            }
            finally
            {
                _openRDPCommand.Enabled = true;
            }
        }

        private void ShowOptionsWindow()
        {
            OptionsWindow = new OptionsWindow();
            OptionsWindow.ShowDialog();
        }
    }
}
