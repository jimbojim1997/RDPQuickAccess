using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace RDPManager.Utilities
{
    internal static class FileUtilities
    {
        #region Text Files
        public static string ReadFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException("path", "Path cannot be empty.");

            using (FileStream fs = new FileStream(path, FileMode.Open))
            using (StreamReader reader = new StreamReader(fs))
            {
                return reader.ReadToEnd();
            }
        }

        public static void WriteFile(string path, string data)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException("path", "Path cannot be empty.");
            if (data == null) throw new ArgumentNullException("data", "Data cannot be null.");

            using (FileStream fs = new FileStream(path, FileMode.Create))
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(data);
            }
        }
        #endregion

        #region Binary Files
        public static T ReadBinaryFile<T>(string path) where T: class
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException("path", "Path cannot be empty.");

            using(FileStream fs = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(fs) as T;
            }
        }

        public static void WriteBinaryFile<T>(string path, T data) where T:class
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException("path", "Path cannot be empty.");
            if (data == null) throw new ArgumentNullException("data", "Data cannot be null.");

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, data);
            }
        }
        #endregion

        #region XML Files
        internal static T ReadXMLFile<T>(string path) where T: class
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException("path", "Path cannot be empty.");

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return serializer.Deserialize(fs) as T;
            }
        }

        internal static void WriteXMLFile<T>(string path, T data) where T : class
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException("path", "Path cannot be empty.");
            if (data == null) throw new ArgumentNullException("data", "Data cannot be null.");

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(fs, data);
            }
        }
        #endregion

        public static IEnumerable<string> GetDirectoryFiles(string path, string filter = "*.*", SearchOption searchOption = SearchOption.AllDirectories)
        {
            return Directory.EnumerateFiles(path, filter, searchOption);
        }

        internal static string ResolvePath(string path)
        {
            return System.Environment.ExpandEnvironmentVariables(path);
        }

        public static bool IsFile(string path)
        {
            return File.Exists(path);
        }

        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public static bool IsDirectory(string path)
        {
            return Directory.Exists(path);
        }

        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }
    }
}
