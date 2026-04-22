using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Not_Defteri
{
    internal static class UpdateService
    {
        public const string VersionUrl = "https://raw.githubusercontent.com/popmarley/Note_Pad/master/version.txt";
        public const string LatestReleaseUrl = "https://github.com/popmarley/Note_Pad/releases/latest";

        public static async Task<string> GetOnlineVersionAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(15);
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Not-Defteri-Updater/3.6");
                string version = await client.GetStringAsync(VersionUrl);
                return (version ?? string.Empty).Trim();
            }
        }

        public static bool IsOnlineVersionNewer(string localVersion, string onlineVersion)
        {
            Version local;
            Version online;

            if (Version.TryParse(NormalizeVersion(localVersion), out local) &&
                Version.TryParse(NormalizeVersion(onlineVersion), out online))
            {
                return online > local;
            }

            return !string.Equals(localVersion, onlineVersion, StringComparison.OrdinalIgnoreCase);
        }

        private static string NormalizeVersion(string version)
        {
            if (string.IsNullOrWhiteSpace(version))
            {
                return "0.0.0";
            }

            return version.Trim();
        }
    }
}
