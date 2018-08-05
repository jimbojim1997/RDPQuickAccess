using System.Collections.Generic;

namespace RDPManager.Model
{
    internal class RDPData
    {
        public string Path {get; set; }
        public Dictionary<string, string> Properties { get; set; }

        public RDPData()
        {
            Properties = new Dictionary<string, string>();
        }

        public RDPData(string path, Dictionary<string, string> properties)
        {
            Path = path;
            Properties = properties;
        }
    }
}
