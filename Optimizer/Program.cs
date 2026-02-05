using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Optimizer
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Check if this EXE is running as the updater
            if (args.Length > 0 && args[0] == "/update")
            {
                // Wait a little to let old app close
                Thread.Sleep(1000);

                string oldPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Optimizer.exe");
                string newPath = Application.ExecutablePath;

                try
                {
                    // Replace old EXE with the new one
                    File.Copy(newPath, oldPath, true);

                    // Launch the updated EXE
                    Process.Start(oldPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Updater failed: " + ex.Message);
                }

                return; // Exit updater
            }

            // Normal app launch
            Application.Run(new Optimizer());
        }
    }
}
