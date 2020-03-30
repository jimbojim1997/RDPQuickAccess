using RDPQuickAccess.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RDPQuickAccess.Utilities
{
    internal static class RdpUtilities
    {
        private const char RDP_PART_SPLIT = ':';

        public static RdpData ParseRDPFile(string path)
        {
            path = FileUtilities.ResolvePath(path);
            if (!FileUtilities.FileExists(path)) throw new System.IO.FileNotFoundException("The supplied file path does not exist.", path);

            RdpData data = new RdpData();
            data.Path = path;

            string fileContents = FileUtilities.ReadFile(path);
            foreach (string line in fileContents.Split(Environment.NewLine.ToCharArray()).Where(s => !string.IsNullOrWhiteSpace(s)))
            {
                string[] parts = line.Split(RDP_PART_SPLIT);
                if (parts.Length < 3) continue;

                data.Properties.Add(parts[0], parts.Skip(2).Aggregate((t, s) => t + RDP_PART_SPLIT + s));
            }

            return data;
        }

        public static IEnumerable<RdpData> ParseRDPFiles(string path)
        {
            path = FileUtilities.ResolvePath(path);
            if (!FileUtilities.DirectoryExists(path)) throw new System.IO.FileNotFoundException("The supplied directory path does not exist.", path);

            List<RdpData> data = new List<RdpData>();
            foreach (string file in FileUtilities.GetDirectoryFiles(path, "*.rdp"))
            {
                data.Add(ParseRDPFile(file));
            }

            return data;
        }

        public static RdpData GetRDPDataByKeyValue(IEnumerable<RdpData> rdpDatas, KeyValuePair<string, string> criteria)
        {
            foreach (RdpData rdpData in rdpDatas)
            {
                if (rdpData.Properties.ContainsKey(criteria.Key) && rdpData.Properties[criteria.Key] == criteria.Value) return rdpData;
            }
            return null;
        }

        public static RdpData GetRDPDataByFileName(IEnumerable<RdpData> rdpDatas, string criteria)
        {
            return GetRDPDataByFileName(rdpDatas, criteria, (c, d) => System.IO.Path.GetFileNameWithoutExtension(d.Path).StartsWith(c, StringComparison.CurrentCultureIgnoreCase));
        }

        public static RdpData GetRDPDataByFileName(IEnumerable<RdpData> rdpDatas, string criteria, Func<string, RdpData, bool> compare)
        {
            foreach (RdpData rdpData in rdpDatas)
            {
                if (compare(criteria, rdpData)) return rdpData;
            }
            return null;
        }

        public static async Task<OpenRdpResult> OpenRdp(string query, string fileSearchPath)
        {
            if (string.IsNullOrWhiteSpace(query)) return OpenRdpResult.EmptyAddress;

            IEnumerable<RdpData> rdpDatas = RdpUtilities.ParseRDPFiles(fileSearchPath);

            //Find RDPData by file name
            RdpData rdpData = RdpUtilities.GetRDPDataByFileName(rdpDatas, query);

            //Find RDPData by domain
            if (rdpData == null)
            {
                rdpData = RdpUtilities.GetRDPDataByKeyValue(rdpDatas, new KeyValuePair<string, string>("full address", query));
            }

            //Find RDPData by IP
            if (rdpData == null)
            {
                IPAddress ipAddress;
                string[] addressParts = query.Split(':');
                if (addressParts.Length >= 1)
                {
                    IPAddress.TryParse(addressParts[0], out ipAddress);
                    if (ipAddress == null)
                    {
                        try
                        {
                            ipAddress = (await Dns.GetHostAddressesAsync(query)).First();
                        }
                        catch { }
                    }
                    if (ipAddress != null)
                    {
                        string address = ipAddress.ToString();
                        if (addressParts.Length >= 2) address += $":{addressParts[1]}";
                        rdpData = RdpUtilities.GetRDPDataByKeyValue(rdpDatas, new KeyValuePair<string, string>("full address", address));
                    }
                }
            }

            //Start the RDP session
            if (rdpData != null)
            {
                RdpUtilities.StartExistingRDP(rdpData.Path);
                return OpenRdpResult.Success;
            }
            else
            {
                return OpenRdpResult.NotFound;
            }

        }

        public static void StartExistingRDP(string path)
        {
            Process process = new Process();
            process.StartInfo.FileName = path;
            process.Start();
        }

        public static void StartNewRDP(string target)
        {
            Process process = new Process();
            process.StartInfo.FileName = FileUtilities.ResolvePath(((App)App.Current).Settings.RDPApplicationPath);
            process.StartInfo.Arguments = $"/v:{target}";
            process.Start();
        }
    }
}
