using RDPManager.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace RDPManager.Utilities
{
    internal static class RDPUtilities
    {
        private const char RDP_PART_SPLIT = ':';

        public static RDPData ParseRDPFile(string path)
        {
            path = FileUtilities.ResolvePath(path);
            if (!FileUtilities.FileExists(path)) throw new System.IO.FileNotFoundException("The supplied file path does not exist.", path);

            RDPData data = new RDPData();
            data.Path = path;

            string fileContents = FileUtilities.ReadFile(path);
            foreach(string line in fileContents.Split(Environment.NewLine.ToCharArray()).Where(s => !string.IsNullOrWhiteSpace(s)))
            {
                string[] parts = line.Split(RDP_PART_SPLIT);
                if (parts.Length < 3) continue;

                data.Properties.Add(parts[0], parts.Skip(2).Aggregate((t, s) => t + RDP_PART_SPLIT + s));
            }

            return data;
        }

        public static IEnumerable<RDPData> ParseRDPFiles(string path)
        {
            path = FileUtilities.ResolvePath(path);
            if(!FileUtilities.DirectoryExists(path)) throw new System.IO.FileNotFoundException("The supplied directory path does not exist.", path);

            List<RDPData> data = new List<RDPData>();
            foreach(string file in FileUtilities.GetDirectoryFiles(path, "*.rdp"))
            {
                data.Add(ParseRDPFile(file));
            }

            return data;
        }

        public static RDPData GetRDPDataByKeyValue(IEnumerable<RDPData> rdpDatas, KeyValuePair<string, string> criteria)
        {
            foreach(RDPData rdpData in rdpDatas)
            {
                if (rdpData.Properties.ContainsKey(criteria.Key) && rdpData.Properties[criteria.Key] == criteria.Value) return rdpData;
            }
            return null;
        }

        public static RDPData GetRDPDataByFileName(IEnumerable<RDPData> rdpDatas, string criteria)
        {
            return GetRDPDataByFileName(rdpDatas, criteria, (c, d) => System.IO.Path.GetFileNameWithoutExtension(d.Path).StartsWith(c, StringComparison.CurrentCultureIgnoreCase));
        }

        public static RDPData GetRDPDataByFileName(IEnumerable<RDPData> rdpDatas, string criteria, Func<string, RDPData, bool> compare)
        {
            foreach(RDPData rdpData in rdpDatas)
            {
                if (compare(criteria, rdpData)) return rdpData;
            }
            return null;
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
