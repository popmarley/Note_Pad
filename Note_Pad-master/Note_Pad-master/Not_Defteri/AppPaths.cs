using System;
using System.IO;
using System.Windows.Forms;

namespace Not_Defteri
{
    internal static class AppPaths
    {
        private const string AppFolderName = "Not_Defteri";

        public static string DataDirectory
        {
            get
            {
                string basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string path = Path.Combine(basePath, AppFolderName);
                Directory.CreateDirectory(path);
                return path;
            }
        }

        public static string StartupVersionFile
        {
            get { return Path.Combine(Application.StartupPath, "version.txt"); }
        }
    }
}
