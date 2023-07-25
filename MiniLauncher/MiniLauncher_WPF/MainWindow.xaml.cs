using MniLauncher;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Interop;

namespace WingsTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected MiniNotifyIcon _MiniIcon;
        protected HwndSource _HwndSource;
        protected bool _NeedQuit = false;

        public MainWindow()
        {
            InitializeComponent();

            _MiniIcon = new MiniNotifyIcon(this); 
        }

        #region SingleInstance
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            _HwndSource = PresentationSource.FromVisual(this) as HwndSource;
            _HwndSource.AddHook(WndProc);
        }

        protected virtual IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == (int)WINAPI.WinMsg.WM_SHOWWINDOW)
            {
                BringToForeground();
            }

            return IntPtr.Zero;
        }

        public virtual void NotifyIconDClick(object sender, EventArgs e)
        {
            BringToForeground();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            this.ShowInTaskbar = this.WindowState != WindowState.Minimized;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = !_NeedQuit;
            this.WindowState = WindowState.Minimized;
            base.OnClosing(e);
        }

        public virtual void BringToForeground()
        {
            this.WindowState = WindowState.Normal;
            this.Activate();
        }

        public virtual void ExitClick(object sender, EventArgs e)
        {
            _NeedQuit = true;
            this.Close();
            //App.Current.Shutdown();
        }
        #endregion

    }
}
