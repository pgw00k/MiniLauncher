using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainUpdate
{
    public class GlobalConfig
    {

        public static string URLGameVersionCheck = "http://192.168.10.7/assets/tools/launcher/version.txt";
        public static string URLGamePackage = "http://192.168.10.7/assets/tools/launcher/WingsTools.zip";

        public static string InstallPath = Path.Combine(Directory.GetCurrentDirectory(), "launcher");
        public static string VersionFile = Path.Combine(InstallPath, "LauncherVersion.txt");
        public static string MainExe = Path.Combine(InstallPath, "WingsTools.exe") ;
    }
}
