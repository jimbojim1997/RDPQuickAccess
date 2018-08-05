using RDPManager.Model;
using RDPManager.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace RDPManager.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
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
                RaisePropertyChaged(value);
            }
        }

        public ICommand OpenRDPCommand
        {
            get
            {
                return new ActionCommand(OpenRDP);
            }
        }

        private void OpenRDP()
        {
            if (string.IsNullOrWhiteSpace(RDAddress))
            {
                MessageBox.Show($"The address cannot be empty.", "Error");
                return;
            }

            IEnumerable<RDPData> rdpDatas = RDPUtilities.ParseRDPFiles(_app.Settings.RDPFileSearchPath);

            //Find RDPData by file name
            RDPData rdpData = RDPUtilities.GetRDPDataByFileName(rdpDatas, RDAddress);

            //Find RDPData by domain
            if(rdpData == null)
            {
                rdpData = RDPUtilities.GetRDPDataByKeyValue(rdpDatas, new KeyValuePair<string, string>("full address", RDAddress));
            }

            //Find RDPData by IP
            if (rdpData == null)
            {
                IPAddress ipAddress;
                string[] addressParts = RDAddress.Split(':');
                if (addressParts.Length >= 1) {
                    IPAddress.TryParse(addressParts[0], out ipAddress);
                    if (ipAddress == null)
                    {
                        try
                        {
                            ipAddress = Dns.GetHostAddresses(RDAddress).First();
                        }
                        catch (System.Net.Sockets.SocketException e)
                        {
                        }
                    }
                    if (ipAddress != null)
                    {
                        string address = ipAddress.ToString();
                        if (addressParts.Length >= 2) address += $":{addressParts[1]}";
                        rdpData = RDPUtilities.GetRDPDataByKeyValue(rdpDatas, new KeyValuePair<string, string>("full address", address));
                    }
                }
            }

            //Start the RDP session
            if(rdpData != null)
            {
                RDPUtilities.StartExistingRDP(rdpData.Path);
                if (_app.Settings.ExitOnSuccess) Environment.Exit(0);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show($"Unable to find RDPfile for '{RDAddress}'.\nOpen new session?", "Information", MessageBoxButton.YesNo);
                if(result == MessageBoxResult.Yes)
                {
                    RDPUtilities.StartNewRDP(RDAddress);
                    if (_app.Settings.ExitOnSuccess) Environment.Exit(0);
                }
            }
            
        }
    }
}
