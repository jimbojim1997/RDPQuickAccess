using RDPManager.Utilities;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace RDPManager.Model
{
    public class AppSettings : DefaultsManager
    {
        [DefaultValue(".")]
        [XmlElement("RDPFileSearchPath")]
        public string RDPFileSearchPath { get; set; }
        [DefaultValue(true)]
        public bool ExitOnSuccess { get; set; }
        [DefaultValue(@"%windir%\system32\mstsc.exe")]
        public string RDPApplicationPath { get; set; }

        public AppSettings() : base()
        {

        }

        public static AppSettings LoadFromFile(string path)
        {
            try
            {
                path = FileUtilities.ResolvePath(path);
                if (FileUtilities.FileExists(path)) return FileUtilities.ReadXMLFile<AppSettings>(path);
                else return null;
            }catch(Exception e)
            {
                return null;
            }
        }

        public void SaveToFile(string path)
        {
            try
            {
                path = FileUtilities.ResolvePath(path);
                FileUtilities.WriteXMLFile<AppSettings>(path, this);
            }catch(Exception e)
            {
                
            }
        }
    }
}
