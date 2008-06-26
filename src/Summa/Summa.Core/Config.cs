using System;
using GConf;

namespace Summa {
    namespace Core {
        public static class Config {
            private static GConf.Client client = new GConf.Client();
            
            //TODO: put these in a schema file.
            
            private static string SUMMA_PATH = "/apps/summa";
            private static string KEY_LIBNOTIFY = "/apps/summa/show_notifications";
            private static string KEY_WIN_WIDTH = "/apps/summa/win_width";
            private static string KEY_WIN_HEIGHT = "/apps/summa/win_height";
            private static string KEY_MAIN_PANE_POSITION = "/apps/summa/main_pane_pos";
            private static string KEY_LEFT_PANE_POSITION = "/apps/summa/left_pane_pos";
            private static string KEY_RIGHT_PANE_POSITION = "/apps/summa/right_pane_pos";
            private static string KEY_SHOULD_SORT_FEEDVIEW = "/apps/summa/sort_feedview";
            private static string KEY_DEFAULT_ZOOM_LEVEL = "/apps/summa/default_zoom_level";
            private static string KEY_GLOBAL_UPDATE_INTERVAL = "/apps/summa/global_update_interval";
            private static string KEY_BOOKMARKER = "/apps/summa/bookmarker";
            
            public static bool ShowNotifications {
                get {
                    try {
                        return (bool)client.Get(KEY_LIBNOTIFY);
                    } catch ( GConf.NoSuchKeyException e ) {
                        client.Set(KEY_LIBNOTIFY, true);
                        return true;
                    }
                }
                set {
                    client.Set(KEY_LIBNOTIFY, value);
                }
            }
            
            public static int WindowHeight {
                get {
                    try {
                        return (int)client.Get(KEY_WIN_HEIGHT);
                    } catch ( GConf.NoSuchKeyException e ) {
                        client.Set(KEY_WIN_HEIGHT, 400);
                        return 400;
                    }
                }
                set {
                    client.Set(KEY_WIN_HEIGHT, value);
                }
            }
            
            public static int WindowWidth {
                get {
                    try {
                        return (int)client.Get(KEY_WIN_WIDTH);
                    } catch ( GConf.NoSuchKeyException e ) {
                        client.Set(KEY_WIN_WIDTH, 700);
                        return 700;
                    }
                }
                set {
                    client.Set(KEY_WIN_WIDTH, value);
                }
            }
            
            public static int MainPanePosition {
                get {
                    try {
                        return (int)client.Get(KEY_MAIN_PANE_POSITION);
                    } catch ( GConf.NoSuchKeyException e ) {
                        client.Set(KEY_MAIN_PANE_POSITION, 170);
                        return 170;
                    }
                }
                set {
                    client.Set(KEY_MAIN_PANE_POSITION, value);
                }
            }
            
            public static int LeftPanePosition {
                get {
                    try {
                        return (int)client.Get(KEY_LEFT_PANE_POSITION);
                    } catch ( GConf.NoSuchKeyException e ) {
                        client.Set(KEY_LEFT_PANE_POSITION, 170);
                        return 170;
                    }
                }
                set {
                    client.Set(KEY_LEFT_PANE_POSITION, value);
                }
            }
            
            public static int RightPanePosition {
                get {
                    try {
                        return (int)client.Get(KEY_RIGHT_PANE_POSITION);
                    } catch ( GConf.NoSuchKeyException e ) {
                        client.Set(KEY_RIGHT_PANE_POSITION, 170);
                        return 170;
                    }
                }
                set {
                    client.Set(KEY_RIGHT_PANE_POSITION, value);
                }
            }
            
            public static bool SortFeedview {
                get {
                    try {
                        return (bool)client.Get(KEY_SHOULD_SORT_FEEDVIEW);
                    } catch ( GConf.NoSuchKeyException e ) {
                        client.Set(KEY_SHOULD_SORT_FEEDVIEW, false);
                        return false;
                    }
                }
                set {
                    client.Set(KEY_SHOULD_SORT_FEEDVIEW, value);
                }
            }
            
            public static int DefaultZoomLevel {
                get {
                    try {
                        return (int)client.Get(KEY_DEFAULT_ZOOM_LEVEL);
                    } catch ( GConf.NoSuchKeyException e ) {
                        client.Set(KEY_DEFAULT_ZOOM_LEVEL, 10);
                        return 10;
                    }
                }
                set {
                    client.Set(KEY_DEFAULT_ZOOM_LEVEL, value);
                }
            }
            
            public static uint GlobalUpdateInterval {
                get {
                    try {
                        int a = (int)client.Get(KEY_GLOBAL_UPDATE_INTERVAL);
                        return (uint)a;
                    } catch ( GConf.NoSuchKeyException e ) {
                        client.Set(KEY_GLOBAL_UPDATE_INTERVAL, 3600000);
                        return 3600000;
                    }
                }
                set {
                    client.Set(KEY_GLOBAL_UPDATE_INTERVAL, (int)value);
                }
            }
            
            public static string Bookmarker {
                get {
                    try {
                        return (string)client.Get(KEY_BOOKMARKER);
                    } catch ( GConf.NoSuchKeyException e ) {
                        client.Set(KEY_BOOKMARKER, "Native");
                        return "Native";
                    }
                }
                set {
                    client.Set(KEY_BOOKMARKER, value);
                }
            }
        }
    }
}
