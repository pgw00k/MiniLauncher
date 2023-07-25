using WingsTools.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Resources;

using Application = System.Windows.Application;

namespace WingsTools
{

    public class MiniNotifyMenu: ContextMenuStrip
    {

        public MiniNotifyMenu(MainWindow mainwin)
        {
            ToolStripMenuItem itemRight = new ToolStripMenuItem();
            itemRight.Text = "Wings";
            itemRight.Enabled = false;

            ToolStripMenuItem itemExit = new ToolStripMenuItem();
            itemExit.Text = "Exit";
            itemExit.Click += new EventHandler(mainwin.ExitClick);

            Items.Add(itemRight);
            Items.Add(itemExit);
        }
    }

    public class MiniNotifyIcon
    {
        protected NotifyIcon nIcon;

        public MiniNotifyIcon(MainWindow mainwin) 
        {
            nIcon = new NotifyIcon();
            nIcon.Text = "Wings Tools";
            nIcon.Icon = Util.ToIcon(@"pack://application:,,,/icon.ico");
            nIcon.Visible = true;
            nIcon.DoubleClick += mainwin.NotifyIconDClick;
            nIcon.ContextMenuStrip = new MiniNotifyMenu(mainwin);
        }
    }

    public class Util
    {
        public static Icon ToIcon(string uri)
        {
            if (string.IsNullOrEmpty(uri)) return null;

            StreamResourceInfo streamInfo = Application.GetResourceStream(new Uri(uri));

            if (streamInfo == null)
            {
                string msg = "The supplied image source '{0}' could not be resolved.";
                msg = String.Format(msg, uri);
                throw new ArgumentException(msg);
            }

            return new Icon(streamInfo.Stream);
        }
    }
}
