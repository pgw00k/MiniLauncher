using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.ComponentModel;

namespace WingsTools
{

    public enum UpdateState
    {
        READY = 0,
        FAILD = -1,
        DOWNLOADING = 1,
        DOWNLOADCOMPELETE = 2,
        INSTALLING = 3,
        INSTALLCOMPELETE = 4,
        SUCCESS = 10,
    }

    public class UpdateChecker
    {
        public string UpdatePackURL;
        public string UpdateVersionURL;
        public string InstallPath;
        public string DownloadTempPath = "download";
        public string DownloadTempFileName;
        public Version LocalVersion = new Version("0.0.0.0");
        public Version OnlineVersion;
        public UpdateState State = UpdateState.READY;

        public bool IsAutoDeleteTempFileAfterInstall = true;
        public bool IsAutoInstallAfterDownload = true;

        public Action<UpdateChecker> OnDonwloadCompelete = null;
        public Action<UpdateChecker,int> OnDonwloading = null;
        public Action<UpdateChecker> OnUpdateCompelete = null;
        public Action<Exception> OnError = null;

        protected string _TempFullFilePath;

        public UpdateChecker(string url= "http://192.168.10.7/assets/tools/MainUpdate/MainUpdate.zip", string installPath="")
        {
            UpdatePackURL = url;
            InstallPath = Path.GetFullPath(installPath);

            DownloadTempFileName = Path.GetFileName(UpdatePackURL);
        }

        public virtual void Update()
        {
            if(!Directory.Exists(DownloadTempPath))
            {
                Directory.CreateDirectory(DownloadTempPath);
            }
            _TempFullFilePath = Path.Combine(DownloadTempPath, DownloadTempFileName);

            try
            {
                WebClient webClient = new WebClient();
                OnlineVersion = new Version(webClient.DownloadString(UpdateVersionURL));

                if (OnlineVersion != LocalVersion)
                {
                    DownloadNewPack();
                }else
                {
                    OnUpdateCompelete?.Invoke(this);
                }
            }
            catch (Exception err)
            {
                OnError?.Invoke(err);
            }
        }

        public virtual void DownloadNewPack()
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressChanged);
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted);
                webClient.DownloadFileAsync(new Uri(UpdatePackURL), _TempFullFilePath, this);
            }
            catch (Exception err)
            {
                OnError?.Invoke(err);
            }
        }

        public virtual void InstallPack()
        {
            try
            {
                using (Stream zipStream = File.OpenRead(_TempFullFilePath))
                {
                    using (ZipArchive archive = new ZipArchive(zipStream))
                    {
                        archive.ExtractToDirectory(InstallPath, true);
                    }
                }

                if(IsAutoDeleteTempFileAfterInstall)
                {
                    File.Delete(_TempFullFilePath);
                }

                OnUpdateCompelete?.Invoke(this);
            }
            catch (Exception err)
            {
                OnError?.Invoke(err);
            }
        }


        public virtual void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            OnDonwloading?.Invoke(this, e.ProgressPercentage);
        }

        public virtual void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            OnDonwloadCompelete?.Invoke(this);

            if (IsAutoInstallAfterDownload)
            {
                InstallPack();
            }
        }
    }
}
