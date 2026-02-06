using System;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

class Updater
{
    private const string UpdateInfoUrl =
        "https://raw.githubusercontent.com/MrPcGamerYT/Optimizer/main/update.json";

    public static void CheckAndUpdate()
    {
        try
        {
            using (WebClient wc = new WebClient())
            {
                wc.CachePolicy = new System.Net.Cache.RequestCachePolicy(
                    System.Net.Cache.RequestCacheLevel.NoCacheNoStore);

                string[] lines = wc.DownloadString(UpdateInfoUrl)
                    .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                if (lines.Length < 2)
                    throw new Exception("Invalid update.json format");

                string latestVersionText = lines[0].Trim();
                string installerUrl = lines[1].Trim();

                Version latestVersion = new Version(latestVersionText);
                Version currentVersion = new Version(Application.ProductVersion);

                if (latestVersion <= currentVersion)
                    return; // already up to date

                DialogResult result = MessageBox.Show(
                    $"New version {latestVersion} is available.\n\nUpdate now?",
                    "Optimizer Update",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information
                );

                if (result != DialogResult.Yes)
                    return;

                string installerPath = Path.Combine(
                    Path.GetTempPath(),
                    "OptimizerSetup.exe"
                );

                if (File.Exists(installerPath))
                    File.Delete(installerPath);

                wc.DownloadFile(installerUrl, installerPath);

                // ✅ RUN INSTALLER ONLY
                Process.Start(new ProcessStartInfo
                {
                    FileName = installerPath,
                    UseShellExecute = true,
                    Verb = "runas"
                });

                // ❌ DO NOT restart app
                Application.Exit();
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
}
