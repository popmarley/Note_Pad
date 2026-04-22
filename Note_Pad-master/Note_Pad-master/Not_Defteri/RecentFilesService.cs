using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Not_Defteri
{
    internal static class RecentFilesService
    {
        private const int MaxItems = 10;

        private static string StorePath
        {
            get { return Path.Combine(AppPaths.DataDirectory, "recent-files.txt"); }
        }

        public static IList<string> Load()
        {
            try
            {
                if (!File.Exists(StorePath))
                {
                    return new List<string>();
                }

                return File.ReadAllLines(StorePath)
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Take(MaxItems)
                    .ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        public static void Add(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            try
            {
                List<string> paths = Load()
                    .Where(item => !string.Equals(item, path, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                paths.Insert(0, path);
                File.WriteAllLines(StorePath, paths.Take(MaxItems).ToArray());
            }
            catch
            {
                // Recent files should never block editing.
            }
        }

        public static void Clear()
        {
            try
            {
                if (File.Exists(StorePath))
                {
                    File.Delete(StorePath);
                }
            }
            catch
            {
                // Ignore cleanup errors.
            }
        }

        public static void Remove(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            try
            {
                List<string> paths = Load()
                    .Where(item => !string.Equals(item, path, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                File.WriteAllLines(StorePath, paths.ToArray());
            }
            catch
            {
                // Ignore recent file cleanup errors.
            }
        }
    }
}
