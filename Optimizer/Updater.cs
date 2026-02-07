using System;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text.RegularExpressions;

class Updater
{
    private const string UpdateInfoUrl =
        "https://raw.githubusercontent.com/MrPcGamerYT/Optimizer/main/update.json";

    private const string TempInstallerName = "OptimizerSetup.exe";

    public static void CheckAndUpdate()
    {
        try
        {
            using (WebClient wc = new WebClient())
            {
                // Disable caching to always get the latest JSON
                wc.CachePolicy = new System.Net.Cache.RequestCachePolicy(
                    System.Net.Cache.RequestCacheLevel.NoCacheNoStore);

                string json = wc.DownloadString(UpdateInfoUrl);

                string latestVersion = ExtractJsonValue(json, "version");
                string installerUrl = ExtractJsonValue(json, "url");

                // Compare versions
                if (CompareVersions(latestVersion, Application.ProductVersion) <= 0)
                    return; // Up-to-date → do nothing

                string installerPath = Path.Combine(Path.GetTempPath(), TempInstallerName);

                // If installer already downloaded for this version, skip download
                if (!File.Exists(installerPath) || !VerifyInstallerVersion(installerPath, latestVersion))
                {
                    // Remove old installer
                    if (File.Exists(installerPath))
                        File.Delete(installerPath);

                    // Download new installer
                    wc.DownloadFile(installerUrl, installerPath);
                }

                // Ask user to update
                if (MessageBox.Show(
                    $"A new version {latestVersion} is available.\nDo you want to update now?",
                    "Optimizer Update",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information) != DialogResult.Yes)
                    return;

                // Run installer as admin
                Process.Start(new ProcessStartInfo
                {
                    FileName = installerPath,
                    UseShellExecute = true,
                    Verb = "runas"
                });

                // Exit current app immediately
                Environment.Exit(0);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Update failed:\n" + ex.Message,
                "Update Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }

    private static string ExtractJsonValue(string json, string key)
    {
        var match = Regex.Match(json, $"\"{key}\"\\s*:\\s*\"([^\"]+)\"", RegexOptions.IgnoreCase);
        if (!match.Success)
            throw new Exception($"Missing '{key}' in update.json");
        return match.Groups[1].Value;
    }

    private static int CompareVersions(string vLatest, string vCurrent)
    {
        int[] latestParts = ParseVersion(vLatest);
        int[] currentParts = ParseVersion(vCurrent);
        int maxLen = Math.Max(latestParts.Length, currentParts.Length);
        for (int i = 0; i < maxLen; i++)
        {
            int latest = i < latestParts.Length ? latestParts[i] : 0;
            int current = i < currentParts.Length ? currentParts[i] : 0;
            if (latest > current) return 1;
            if (latest < current) return -1;
        }
        return 0;
    }

    private static int[] ParseVersion(string v)
    {
        var match = Regex.Match(v, @"\d+(\.\d+)*");
        if (!match.Success) throw new Exception("Invalid version format: " + v);
        string[] parts = match.Value.Split('.');
        int[] nums = new int[parts.Length];
        for (int i = 0; i < parts.Length; i++) nums[i] = int.Parse(parts[i]);
        return nums;
    }

    // Optional: verify installer filename contains version (you can embed version in name if needed)
    private static bool VerifyInstallerVersion(string path, string version)
    {
        return Path.GetFileNameWithoutExtension(path).Contains(version.Replace(".", "_"));
    }
}
