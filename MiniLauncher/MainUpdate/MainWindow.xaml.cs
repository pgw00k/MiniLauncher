using System;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.Windows.Shapes;
using WingsTools;
using System.Threading;

namespace MainUpdate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string OnlineVersion = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        protected virtual void WindowContentRendered(object sender, EventArgs e)
        {
            UpdateChecker uc = new UpdateChecker(GlobalConfig.URLGamePackage,GlobalConfig.InstallPath);
            uc.UpdateVersionURL = GlobalConfig.URLGameVersionCheck;
            uc.OnDonwloading += OnDownloading;
            uc.OnUpdateCompelete += OnUpdateCompelete;

            if(File.Exists(GlobalConfig.VersionFile))
            {
                uc.LocalVersion = new Version(File.ReadAllText(GlobalConfig.VersionFile));
            }

            uc.Update();
        }

        protected virtual void OnDownloading(UpdateChecker uc,int ps)
        {
            PBUpdate.Value = ps;
        }

        protected virtual void OnUpdateCompelete(UpdateChecker uc)
        {
            if (File.Exists(GlobalConfig.MainExe))
            {
                OnlineVersion = uc.OnlineVersion.ToString();
                File.WriteAllText(GlobalConfig.VersionFile, uc.OnlineVersion.ToString());
                Thread NewThread = new Thread(this.StatrMainExe);
                NewThread.Start();
            }


            this.Close();
        }

        protected virtual void StatrMainExe()
        {   
            Thread.Sleep(1000);
            ProcessStartInfo startInfo = new ProcessStartInfo(GlobalConfig.MainExe);
            startInfo.WorkingDirectory = GlobalConfig.InstallPath;
            Process.Start(GlobalConfig.MainExe);
        }
    }
}
