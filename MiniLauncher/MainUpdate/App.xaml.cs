using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MainUpdate
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //IntPtr hwnd = WINAPI.Win32.FindWindow(null, "Wings Tools Launcher");


            //if (hwnd != (IntPtr)0)
            //{
            //    WINAPI.Win32.PostMessage(hwnd, WINAPI.WinMsg.WM_QUIT, (IntPtr)1, (IntPtr)0);
            //}

            Process.Start("cmd.exe", "/C taskkill /f /im WingsTools.exe");
        }
    }
}
