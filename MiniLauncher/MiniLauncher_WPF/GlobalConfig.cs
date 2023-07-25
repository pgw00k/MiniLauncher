using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MniLauncher
{
    public class GlobalConfig
    {

        public static string URLGameVersionCheck = "http://192.168.10.7/assets/tools/libmanager/version.txt";
        public static string URLGamePackage = "http://192.168.10.7/assets/tools/libmanager/game.zip";
        public static string URLBinPackage = "http://192.168.10.7/assets/tools/libmanager/bin.zip";

        public static string RootPath = Directory.GetCurrentDirectory();
        public static string VersionFile = Path.Combine(RootPath, "Version.txt");
        public static string GameFolderName = "game";
        public static string GameMainName = "AdiaLibManagerTool.exe";
        public static string GameZip = Path.Combine(RootPath, "game.zip");
        public static string GameEXE = Path.Combine(RootPath, GameFolderName, GameMainName);

    }
}
