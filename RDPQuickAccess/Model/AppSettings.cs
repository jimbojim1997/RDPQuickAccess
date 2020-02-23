using RDPQuickAccess.Utilities;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace RDPQuickAccess.Model
{
    public class AppSettings : DefaultsManager
    {
        [DefaultValue(".")]
        [XmlElement("RDPFileSearchPath")]
        public string RDPFileSearchPath { get; set; }
        [DefaultValue(true)]
        [XmlElement("ExitOnSuccess")]
        public bool ExitOnSuccess { get; set; }
        [DefaultValue(@"%windir%\system32\mstsc.exe")]
        [XmlElement("RDPApplicationPath")]
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
