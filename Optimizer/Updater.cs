using System;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text.RegularExpressions;

public static class Updater
{
    // URL of the update.json file
    private const string UpdateInfoUrl =
        "https://raw.githubusercontent.com/MrPcGamerYT/Optimizer/main/update.json";

    /// <summary>
    /// Checks for updates and runs installer if available.
    /// </summary>
    public static void CheckAndUpdate()
    {
        try
        {
            using (WebClient wc = new WebClient())
            {
                // Always get the latest version (disable caching)
                wc.CachePolicy = new System.Net.Cache.RequestCachePolicy(
                    System.Net.Cache.RequestCacheLevel.NoCacheNoStore);

                string json = wc.DownloadString(UpdateInfoUrl);

                string latestVersion = GetJsonValue(json, "version");
                string installerUrl = GetJsonValue(json, "url");

                // Compare versions: skip if already up-to-date
                if (CompareVersions(latestVersion, Application.ProductVersion) <= 0)
                    return;

                // Ask the user before updating
                if (MessageBox.Show(
                    $"A new version {latestVersion} is available.\nDo you want to update now?",
                    "Optimizer Update",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information) != DialogResult.Yes)
                {
                    return;
                }

                // Prepare installer path in temp folder
                string installerPath = Path.Combine(Path.GetTempPath(), "OptimizerSetup.exe");

                // Remove any previous installer
                if (File.Exists(installerPath))
                    File.Delete(installerPath);

                // Download latest installer
                wc.DownloadFile(installerUrl, installerPath);

                // Run the installer with admin rights
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

    /// <summary>
    /// Extracts a value from a simple JSON string (no library required)
    /// </summary>
    private static string GetJsonValue(string json, string key)
    {
        var match = Regex.Match(json, $"\"{key}\"\\s*:\\s*\"([^\"]+)\"", RegexOptions.IgnoreCase);
        if (!match.Success)
            throw new Exception($"Missing '{key}' in update.json");
        return match.Groups[1].Value;
    }

    /// <summary>
    /// Compare semantic versions. Returns:
    /// 1 if latest > current,
    /// 0 if equal,
    /// -1 if latest < current
    /// </summary>
    private static int CompareVersions(string latest, string current)
    {
        int[] latestParts = ParseVersion(latest);
        int[] currentParts = ParseVersion(current);
        int maxLen = Math.Max(latestParts.Length, currentParts.Length);

        for (int i = 0; i < maxLen; i++)
        {
            int l = (i < latestParts.Length) ? latestParts[i] : 0;
            int c = (i < currentParts.Length) ? currentParts[i] : 0;

            if (l > c) return 1;
            if (l < c) return -1;
        }

        return 0;
    }

    /// <summary>
    /// Parses a version string like "1.0.3.25" into integer array
    /// </summary>
    private static int[] ParseVersion(string v)
    {
        var match = Regex.Match(v, @"\d+(\.\d+)*");
        if (!match.Success)
            throw new Exception($"Invalid version format: {v}");

        string[] parts = match.Value.Split('.');
        int[] numbers = new int[parts.Length];
        for (int i = 0; i < parts.Length; i++)
            numbers[i] = int.Parse(parts[i]);

        return numbers;
    }
}
