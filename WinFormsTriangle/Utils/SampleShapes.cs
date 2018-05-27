using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WinFormsTriangle.Utils
{
    public class SampleShapes
    {
        public const string DataFolder = "Data";
        public const string BinDebug = "bin\\Debug";

        public static List<string> LoadFileNames()
        {
            List<string> paths = LoadPaths();
            if (paths != null)
            {
                var fileNames = paths.Select(q => Path.GetFileName(q));
                return fileNames.ToList();
            }
            return null;
        }

        public static List<string> LoadPaths()
        {
            string path = GetDataDirectoryPath();
            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path).Where(f => Path.GetExtension(f) == ".xml");
                return files.ToList();
            }

            return null;
        }

        public static string BuildFilePath(string fileName)
        {
            return GetDataDirectoryPath() + "\\" + fileName;
        }

        public static string GetDataDirectoryPath()
        {
            return Directory.GetCurrentDirectory().Replace(BinDebug, DataFolder);
        }
    }
}
