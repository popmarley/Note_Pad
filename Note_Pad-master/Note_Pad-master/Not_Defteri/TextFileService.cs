using System;
using System.IO;
using System.Text;

namespace Not_Defteri
{
    internal sealed class TextFileContent
    {
        public TextFileContent(string text, Encoding encoding, string displayName)
        {
            Text = text;
            Encoding = encoding;
            DisplayName = displayName;
        }

        public string Text { get; private set; }
        public Encoding Encoding { get; private set; }
        public string DisplayName { get; private set; }
    }

    internal static class TextFileService
    {
        private static readonly Encoding Utf8NoBom = new UTF8Encoding(false, true);
        private static readonly Encoding Utf8Bom = new UTF8Encoding(true, true);

        public static TextFileContent ReadAllText(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);
            int preambleLength;
            Encoding encoding = DetectEncoding(bytes, out preambleLength);

            if (encoding != null)
            {
                string text = encoding.GetString(bytes, preambleLength, bytes.Length - preambleLength);
                return new TextFileContent(text, encoding, GetDisplayName(encoding, preambleLength > 0));
            }

            try
            {
                return new TextFileContent(Utf8NoBom.GetString(bytes), Utf8NoBom, "UTF-8");
            }
            catch (DecoderFallbackException)
            {
                Encoding fallback = Encoding.Default;
                return new TextFileContent(fallback.GetString(bytes), fallback, fallback.EncodingName);
            }
        }

        public static void WriteAllTextAtomic(string path, string text, Encoding encoding)
        {
            encoding = encoding ?? Utf8NoBom;

            string directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string tempPath = Path.Combine(
                string.IsNullOrEmpty(directory) ? Directory.GetCurrentDirectory() : directory,
                Path.GetFileName(path) + "." + Guid.NewGuid().ToString("N") + ".tmp");

            File.WriteAllText(tempPath, text, encoding);

            try
            {
                if (File.Exists(path))
                {
                    File.Copy(tempPath, path, true);
                    File.Delete(tempPath);
                }
                else
                {
                    File.Move(tempPath, path);
                }
            }
            catch
            {
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }

                throw;
            }
        }

        public static Encoding DefaultEncoding
        {
            get { return Utf8NoBom; }
        }

        public static string GetDisplayName(Encoding encoding)
        {
            return GetDisplayName(encoding, HasPreamble(encoding));
        }

        private static Encoding DetectEncoding(byte[] bytes, out int preambleLength)
        {
            preambleLength = 0;

            if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
            {
                preambleLength = 3;
                return Utf8Bom;
            }

            if (bytes.Length >= 4 && bytes[0] == 0xFF && bytes[1] == 0xFE && bytes[2] == 0x00 && bytes[3] == 0x00)
            {
                preambleLength = 4;
                return Encoding.UTF32;
            }

            if (bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xFE)
            {
                preambleLength = 2;
                return Encoding.Unicode;
            }

            if (bytes.Length >= 2 && bytes[0] == 0xFE && bytes[1] == 0xFF)
            {
                preambleLength = 2;
                return Encoding.BigEndianUnicode;
            }

            return null;
        }

        private static bool HasPreamble(Encoding encoding)
        {
            return encoding != null && encoding.GetPreamble().Length > 0;
        }

        private static string GetDisplayName(Encoding encoding, bool hasPreamble)
        {
            if (encoding == null)
            {
                return "UTF-8";
            }

            if (encoding.CodePage == Encoding.UTF8.CodePage)
            {
                return hasPreamble ? "UTF-8 BOM" : "UTF-8";
            }

            if (encoding.CodePage == Encoding.Unicode.CodePage)
            {
                return "UTF-16 LE";
            }

            if (encoding.CodePage == Encoding.BigEndianUnicode.CodePage)
            {
                return "UTF-16 BE";
            }

            return encoding.EncodingName;
        }
    }
}
