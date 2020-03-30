using System.Collections.Generic;

namespace RDPQuickAccess.Model
{
    internal class RdpData
    {
        public string Path {get; set; }
        public Dictionary<string, string> Properties { get; set; }

        public RdpData()
        {
            Properties = new Dictionary<string, string>();
        }

        public RdpData(string path, Dictionary<string, string> properties)
        {
            Path = path;
            Properties = properties;
        }
    }
}
