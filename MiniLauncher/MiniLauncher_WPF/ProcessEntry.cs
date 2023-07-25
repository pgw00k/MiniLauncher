using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessageBox = System.Windows.MessageBox;
using System.Windows;
using System.IO;
using System.Security.Policy;
using System.Diagnostics;

namespace WingsTools
{
    public class ProcessEntry
    {
        public static string URLDoMain = "http://192.168.10.7/assets/tools";

        public static string InstallRoot = "";

        public string VersionCheckURL;
        public string PackFileURL;
        public string InstallDirectory;
        public string RunExePath;
        public string LocalVersionFile;

        public ProcessEntry(string name)
        {
            VersionCheckURL = Path.Combine(URLDoMain, name, "version.txt");
            PackFileURL = Path.Combine(URLDoMain, name, $"{name}.zip");
            InstallDirectory = Path.GetFullPath(Path.Combine(InstallRoot, name));
            RunExePath = Path.Combine(InstallDirectory, $"{name}.exe");
            LocalVersionFile = Path.Combine(InstallDirectory, "version.txt");
        }

        public static ProcessEntry[] Entries = new ProcessEntry[] {
            new ProcessEntry("AdiaLibManagerTool"),
            new ProcessEntry("LibHome"),
            new ProcessEntry("MaxPluginManager"),
            new ProcessEntry("ChromeStandaloneSetup64")
        };
    }

    public partial class MainWindow
    {

        public int CurrentIndex = -1;

        public virtual void Window_ContentRendered(object sender, EventArgs e)
        {
        }

        public virtual void BtnDatabaseHomeClick(object sender, RoutedEventArgs e)
        {
            UpdateTarget(1);
        }

        public virtual void BtnLibManagerClick(object sender, RoutedEventArgs e)
        {
            UpdateTarget(0);
        }

        public virtual void BtnMaxPluginClick(object sender, RoutedEventArgs e)
        {
            UpdateTarget(2);
        }

        public virtual void BtnSetupChromeClick(object sender, RoutedEventArgs e)
        {
            UpdateTarget(3);
        }

        public virtual void UpdateTarget(int index = 0)
        {

            if(CurrentIndex>0)
            {
                MessageBox.Show("请等待上一个任务完成！");
                return;
            }

            CurrentIndex = index;

            ProcessEntry Entry = ProcessEntry.Entries[CurrentIndex];

            UpdateChecker uc = new UpdateChecker(Entry.PackFileURL, Entry.InstallDirectory);
            uc.UpdateVersionURL = Entry.VersionCheckURL;

            if(File.Exists(Entry.LocalVersionFile))
            {
                uc.LocalVersion = new Version(File.ReadAllText(Entry.LocalVersionFile));
            }

            uc.OnDonwloading += OnDownloading;
            uc.OnUpdateCompelete += OnUpdateCompelete;
            uc.Update();
        }

        protected virtual void OnDownloading(UpdateChecker uc, int ps)
        {
            PBUpdate.Value = ps;
        }

        protected virtual void OnUpdateCompelete(UpdateChecker uc)
        {

            ProcessEntry Entry = ProcessEntry.Entries[CurrentIndex];

            if (File.Exists(Entry.RunExePath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(Entry.RunExePath);
                startInfo.WorkingDirectory = Entry.InstallDirectory;
                Process.Start(startInfo);
            }

            File.WriteAllText(Entry.LocalVersionFile, uc.OnlineVersion.ToString());

            CurrentIndex = -1;
            this.Close();
        }
    }
}
