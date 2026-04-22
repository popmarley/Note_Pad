using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Not_Defteri
{
    internal sealed class RecoveryInfo
    {
        public string TextPath { get; set; }
        public string MetaPath { get; set; }
        public string OriginalPath { get; set; }
        public DateTime LastWriteTime { get; set; }
    }

    internal static class RecoveryService
    {
        private static string RecoveryDirectory
        {
            get
            {
                string path = Path.Combine(AppPaths.DataDirectory, "Recovery");
                Directory.CreateDirectory(path);
                return path;
            }
        }

        public static IList<RecoveryInfo> ListRecoveries()
        {
            try
            {
                return Directory.GetFiles(RecoveryDirectory, "*.recover.txt")
                    .Select(CreateRecoveryInfo)
                    .Where(info => info != null)
                    .OrderByDescending(info => info.LastWriteTime)
                    .ToList();
            }
            catch
            {
                return new List<RecoveryInfo>();
            }
        }

        public static void Save(string sessionId, string originalPath, string text)
        {
            if (string.IsNullOrEmpty(sessionId) || string.IsNullOrEmpty(text))
            {
                return;
            }

            try
            {
                string textPath = GetTextPath(sessionId);
                string metaPath = GetMetaPath(sessionId);
                File.WriteAllText(textPath, text, new UTF8Encoding(false));
                File.WriteAllLines(metaPath, new[]
                {
                    originalPath ?? string.Empty,
                    DateTime.UtcNow.ToString("O")
                });
            }
            catch
            {
                // Auto recovery must never interrupt editing.
            }
        }

        public static string Read(RecoveryInfo info)
        {
            if (info == null || string.IsNullOrEmpty(info.TextPath) || !File.Exists(info.TextPath))
            {
                return string.Empty;
            }

            return TextFileService.ReadAllText(info.TextPath).Text;
        }

        public static void Delete(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return;
            }

            DeleteFile(GetTextPath(sessionId));
            DeleteFile(GetMetaPath(sessionId));
        }

        public static void Delete(RecoveryInfo info)
        {
            if (info == null)
            {
                return;
            }

            DeleteFile(info.TextPath);
            DeleteFile(info.MetaPath);
        }

        private static RecoveryInfo CreateRecoveryInfo(string textPath)
        {
            try
            {
                string fileName = Path.GetFileName(textPath);
                string sessionId = fileName.EndsWith(".recover.txt", StringComparison.OrdinalIgnoreCase)
                    ? fileName.Substring(0, fileName.Length - ".recover.txt".Length)
                    : Path.GetFileNameWithoutExtension(textPath);
                string metaPath = Path.Combine(Path.GetDirectoryName(textPath), sessionId + ".recover.meta");
                string originalPath = string.Empty;
                if (File.Exists(metaPath))
                {
                    originalPath = File.ReadLines(metaPath).FirstOrDefault() ?? string.Empty;
                }

                return new RecoveryInfo
                {
                    TextPath = textPath,
                    MetaPath = metaPath,
                    OriginalPath = originalPath,
                    LastWriteTime = File.GetLastWriteTime(textPath)
                };
            }
            catch
            {
                return null;
            }
        }

        private static string GetTextPath(string sessionId)
        {
            return Path.Combine(RecoveryDirectory, sessionId + ".recover.txt");
        }

        private static string GetMetaPath(string sessionId)
        {
            return Path.Combine(RecoveryDirectory, sessionId + ".recover.meta");
        }

        private static void DeleteFile(string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch
            {
                // Ignore recovery cleanup errors.
            }
        }
    }
}
