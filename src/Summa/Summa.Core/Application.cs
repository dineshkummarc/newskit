// Application.cs
//
// Copyright (c) 2008 Ethan Osten
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;

using System.Runtime.InteropServices;
using System.Text;

using Gtk;

namespace Summa.Core {
    public static class Application {
        public static LogList Log;
        public static Summa.Core.Database Database;
        public static Gtk.ListStore TagStore;
        public static ArrayList Browsers;
        public static Summa.Gui.StatusIcon StatusIcon;
        public static Summa.Core.Updater Updater;
        public static Summa.Core.DBusInterface DBus;
        public static bool WindowsShown;
        
        public static void Main() {
            //GLib.Thread.Init();
            Gdk.Threads.Init();
            
            Log = new LogList();
            
            Gtk.Application.Init();
            SetProcessName("summa");
            
            GLib.Log.SetLogHandler(null, GLib.LogLevelFlags.All, new GLib.LogFunc(Summa.Core.Log.LogFunc)); //FIXME
            
            /*
             * the ListStore for all Summa.Gui.TagViews, since tags don't
             * vary by context.
             */
            TagStore = new Gtk.ListStore(typeof(Gdk.Pixbuf), typeof(string));
            
            Database = new Summa.Core.Database();
            Updater = new Summa.Core.Updater();
            DBus = new Summa.Core.DBusInterface();
            /*
             * a list of browser instances - when a browser is created, it
             * should be added to this list, and when it is destroyed, it
             * should be removed.
             */
            Browsers = new ArrayList();
            Browsers.Add(new Summa.Gui.Browser());
            StatusIcon = new Summa.Gui.StatusIcon();
            
            foreach ( Summa.Gui.Browser browser in Browsers ) {
                browser.ShowAll();
            }
            WindowsShown = true;
            
            DebugTest();
            
            Gtk.Application.Run();
        }
        
        private static void DebugTest() {
        }
        
        public static void CloseWindow(Summa.Gui.Browser browser) {
            if ( Browsers.Count == 1 ) {
                /* get dimensions */
                int width;
                int height;
                
                browser.GetSize(out width, out height);
                
                Summa.Core.Config.WindowWidth = width;
                Summa.Core.Config.WindowHeight = height;
                
                /* get pane positions */
                int main_size;
                int left_size;
                int right_size;
                
                main_size = browser.main_paned.Position;
                left_size = browser.left_paned.Position;
                right_size = browser.right_paned.Position;
                
                Summa.Core.Config.MainPanePosition = main_size;
                Summa.Core.Config.LeftPanePosition = left_size;
                Summa.Core.Config.RightPanePosition = right_size;
                
                Gtk.Main.Quit();
            } else {
                browser.Destroy();
                Browsers.Remove(browser);
            }
        }
        
        public static void ToggleShown() {
            if ( WindowsShown ) {
                foreach ( Summa.Gui.Browser browser in Summa.Core.Application.Browsers ) {
                    browser.Hide();
                }
                WindowsShown = false;
            } else {
                foreach ( Summa.Gui.Browser browser in Summa.Core.Application.Browsers ) {
                    browser.Show();
                }
                WindowsShown = true;
            }
        }
        
        /* this stuff from Beagle */
        [DllImport("libc")]
		private static extern int prctl (int option, byte [] arg2, ulong arg3, ulong arg4, ulong arg5);

		/* From /usr/include/linux/prctl.h */
		private const int PR_SET_NAME = 15;

		public static void SetProcessName(string name) {
			if (prctl (PR_SET_NAME, Encoding.ASCII.GetBytes (name + '\0'), 0, 0, 0) < 0) {
				Summa.Core.Log.Message(String.Format("Couldn't set process name to '{0}': {1}", name, Mono.Unix.Native.Stdlib.GetLastError()));
			}
		}
    }
}
