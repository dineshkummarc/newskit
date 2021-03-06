// Actions.cs
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
using Gtk;

using Summa.Core;
using Summa.Data;
using Summa.Gui;

namespace Summa.Actions {
    public class AddAction : Gtk.Action {
        private Browser browser;
        
        public AddAction(Browser browser) : base("Add", "_Add") {
            this.browser = browser;
            
            Tooltip = "Create a new feed";
            StockId = Stock.Add;
            Activated += NewAddFeedDialog;
        }
        
        public void NewAddFeedDialog(object obj, EventArgs args) {
            AddFeedDialog add_dialog = new AddFeedDialog();
            add_dialog.TransientFor = browser;
            add_dialog.Show();
        }
    }
    
    public class ImportAction : Gtk.Action {
        private Browser browser;
        
        public ImportAction(Browser browser) : base("Import", "_Import") {
            this.browser = browser;
            
            Tooltip = "Import data";
            Activated += Import;
        }
        
        public void Import(object obj, EventArgs args) {
            FirstRun fr = new FirstRun();
            fr.TransientFor = browser;
            fr.ShowAll();
        }
    }
    
    public class UpdateAllAction : Gtk.Action {
        public UpdateAllAction(Browser browser) : base("Update_all", "_Update All") {
            Tooltip = "Update all feeds";
            StockId = Stock.Refresh;
            Activated += UpdateAll;
        }
        
        public void UpdateAll(object obj, EventArgs args) {
            Summa.Core.Application.Updater.Update();
        }
    }
    
    public class PrintAction : Gtk.Action {
        private Browser browser;
        
        public PrintAction(Browser browser) : base("Print", "_Print") {
            this.browser = browser;
            
            Tooltip = "_Print the currently selected item";
            StockId = Stock.Print;
            Activated += Print;
            Sensitive = false;
        }
        
        public void Print(object obj, EventArgs args) {
            browser.ItemNotebook.CurrentView.Print();
        }
        
        public void CheckShouldSensitive() {
            if ( browser.ItemNotebook.CurrentView.CanPrint() ) {
                browser.print_action.Sensitive = true;
            }
        }
    }
    
    public class PrintPreviewAction : Gtk.Action {
        public PrintPreviewAction(Browser browser) : base("Print_preview", "Print Previe_w") {
            Tooltip = "Show a preview of the printed document";
            StockId = Stock.PrintPreview;
            Activated += PrintPreview;
            Sensitive = false;
        }
        
        public void PrintPreview(object obj, EventArgs args) {
        }
    }
    
    public class EmailLinkAction : Gtk.Action {
        public EmailLinkAction(Browser browser) : base("Email_link", "_Email") {
            Tooltip = "Email a copy of the selected item";
            Activated += EmailLink;
            Sensitive = false;
        }
        
        public void EmailLink(object obj, EventArgs args) {
        }
    }
    
    public class BookmarkAction : Gtk.Action {
        private Browser browser;
        private IBookmarker bookmarker;
        
        public BookmarkAction(Browser browser) : base("Bookmark", "_Bookmark") {
            this.browser = browser;
            
            IconSet bookmark_iconset = new IconSet();
            IconSource bookmark_iconsource = new IconSource();
            bookmark_iconsource.IconName = "bookmark-new";
            bookmark_iconset.AddSource(bookmark_iconsource);
            browser.factory.Add("summa-bookmark-new", bookmark_iconset);
            
            Tooltip = "Bookmark this item";
            StockId = "summa-bookmark-new";
            Activated += Bookmark;
            
            switch(Config.Bookmarker) {
                case "Native":
                    bookmarker = new NativeBookmarker();
                    break;
                case "Dieu":
                    bookmarker = new DieuBookmarker();
                    break;
            }
        }
        
        public void Bookmark(object obj, EventArgs args) {
            if ( browser.ItemView.HasSelected ) {
                bookmarker.ShowBookmarkWindow(browser.curitem.Title, browser.curitem.Uri, browser.curitem.Contents, "");
            }
        }
        
        public void CheckShouldSensitive() {
            if ( bookmarker.CanBookmark() ) {
                Sensitive = true;
            }
        }
    }
    
    public class NewTabAction : Gtk.Action {
        private Browser browser;
        
        public NewTabAction(Browser browser) : base("New_tab", "Open in a New _Tab") {
            this.browser = browser;
            
            Tooltip = "Open the current item in a new tab";
            Activated += NewTab;
        }
        
        public void NewTab(object obj, EventArgs args) {
            browser.ItemNotebook.Load(browser.curitem);
            browser.ItemNotebook.ShowAll();
        }
    }
    
    public class NewWindowAction : Gtk.Action {
        public NewWindowAction(Browser browser) : base("New_window", "New _Window") {
            Tooltip = "Open a new window";
            Activated += NewWindow;
        }
        
        public void NewWindow(object obj, EventArgs args) {
            Window b = new Browser();
            Summa.Core.Application.Browsers.Add(b);
            b.ShowAll();
        }
    }
    
    public class CloseWindowAction : Gtk.Action {
        private Browser browser;
        
        public CloseWindowAction(Browser browser) : base("Close_window", "_Close Window") {
            this.browser = browser;
            
            Tooltip = "Close this window";
            StockId = Stock.Close;
            Activated += CloseWindow;
        }
        
        public void CloseWindow(object obj, EventArgs args) {
            Summa.Core.Application.CloseWindow(browser);
        }
    }
    
    public class CopyAction : Gtk.Action {
        public CopyAction(Browser browser) : base("Copy", "_Copy") {
            Tooltip = "Copy";
            StockId = Stock.Copy;
            Activated += Copy;
            Sensitive = false;
        }
        
        public void Copy(object obj, EventArgs args) {
        }
    }
    
    public class SelectAllAction : Gtk.Action {
        public SelectAllAction(Browser browser) : base("Select_all", "_Select All Text") {
            Tooltip = "Select all text";
            Activated += SelectAll;
            Sensitive = false;
        }
        
        public void SelectAll(object obj, EventArgs args) {
        }
    }
    
    public class FindAction : Gtk.Action {
        public FindAction(Browser browser) : base("Find", "_Find...") {
            Tooltip = "Find an item";
            StockId = Stock.Find;
            Activated += Find;
            Sensitive = false;
        }
        
        public void Find(object obj, EventArgs args) {
        }
    }
    
    public class PreferencesAction : Gtk.Action {
        private Browser browser;
        
        public PreferencesAction(Browser browser) : base("Preferences", "_Preferences") {
            this.browser = browser;
            
            Tooltip = "Preferences for Summa";
            StockId = Stock.Preferences;
            Activated += ShowConfigDialog;
        }
        
        public void ShowConfigDialog(object obj, EventArgs args) {
            Window w = new ConfigDialog();
            w.TransientFor = browser;
            w.ShowAll();
        }
    }
    
    public class UpdateAction : Gtk.Action {
        private Browser browser;
        
        public UpdateAction(Browser browser) : base("Update", "_Update Feed") {
            this.browser = browser;
            
            Tooltip = "Update selected feed";
            StockId = Stock.Refresh;
            Activated += Update;
        }
        
        public void Update(object obj, EventArgs args) {
            if ( browser.FeedView.HasSelected ) {
                Summa.Core.Application.Updater.UpdateFeed(browser.FeedView.Selected);
                
                /*bool updated = browser.FeedView.Selected.Update();
                
                if ( updated ) {
                    browser.FeedView.UpdateSelected();
                    browser.ItemView.Update();
                    browser.UpdateName();
                    browser.ShowNotification(browser.FeedView.Selected);
                }*/
            }
        }
    }
    
    public class MarkReadAction : Gtk.Action {
        private Browser browser;
        
        public MarkReadAction(Browser browser) : base("Mark_read", "_Mark Read") {
            this.browser = browser;
            
            Tooltip = "Mark all items in the selected feed as read";
            StockId = Stock.Apply;
            Activated += MarkAllItemsRead;
        }
        
        public void MarkAllItemsRead(object obj, EventArgs args) {
            if ( browser.FeedView.HasSelected ) {
                browser.FeedView.Selected.MarkItemsRead();
            }
        }
    }
    
    public class DeleteAction : Gtk.Action {
        private Browser browser;
        
        public DeleteAction(Browser browser) : base("Delete", "_Delete Feed") {
            this.browser = browser;
            
            Tooltip = "Delete the selected feed";
            StockId = Stock.Delete;
            Activated += DeleteFeed;
        }
        
        public void DeleteFeed(object obj, EventArgs args) {
            if ( browser.FeedView.HasSelected ) {
                Window del = new DeleteConfirmationDialog(browser.curfeed);
                del.TransientFor = browser;
                del.ShowAll();
            }
        }
    }
    
    public class PropertiesAction : Gtk.Action {
        private Browser browser;
        
        public PropertiesAction(Browser browser) : base("Properties", "_Properties") {
            this.browser = browser;
            
            Tooltip = "Properties of the selected feed";
            StockId = Stock.Properties;
            Activated += ShowPropertiesDialog;
        }
        
        public void ShowPropertiesDialog(object obj, EventArgs args) {
            if ( browser.FeedView.HasSelected ) {
                Window dialog = new FeedPropertiesDialog(browser.FeedView.Selected);
                dialog.TransientFor = browser;
                dialog.ShowAll();
            }
        }
    }
    
    public class TagsAction : Gtk.Action {
        private Browser browser;
        
        public TagsAction(Browser browser) : base("Tags", "Edit Feed _Tags") {
            this.browser = browser;
            
            Tooltip = "Edit the tags of your feeds by tag";
            Activated += ShowTagsWindow;
        }
        
        public void ShowTagsWindow(object obj, EventArgs args) {
            Window dialog = new TagWindow();
            dialog.TransientFor = browser;
            dialog.ShowAll();
        }
    }
    
    public class ZoomInAction : Gtk.Action {
        private Browser browser;
        
        public ZoomInAction(Browser browser) : base("ZoomIn", "Zoom _In") {
            this.browser = browser;
            
            Tooltip = "Increase text size";
            StockId = Stock.ZoomIn;
            Activated += ZoomIn;
        }
        
        public void ZoomIn(object obj, EventArgs args) {
            browser.ItemNotebook.CurrentView.ZoomIn();
        }
        
        public void CheckShouldSensitive() {
            if ( browser.ItemView.HasSelected ) {
                Sensitive = true;
            }
        }
    }
    
    public class ZoomOutAction : Gtk.Action {
        private Browser browser;
        
        public ZoomOutAction(Browser browser) : base("ZoomOut", "Zoom _Out") {
            this.browser = browser;
            
            Tooltip = "Decrease text size";
            StockId = Stock.ZoomOut;
            Activated += ZoomOut;
        }
        
        public void ZoomOut(object obj, EventArgs args) {
            browser.ItemNotebook.CurrentView.ZoomOut();
        }
        
        public void CheckShouldSensitive() {
            if ( browser.ItemView.HasSelected ) {
                if ( Config.DefaultZoomLevel != 3 ) {
                    Sensitive = true;
                } else {
                    Sensitive = false;
                }
            }
        }
    }
    
    public class LoadImagesAction : ToggleAction {
        public LoadImagesAction(Browser browser) : base("LoadImages", "_Load Images", null, null) {
            Tooltip = "Load images in items";
            Toggled += LoadImages;
            
            Sensitive = false;
            Active = true;
        }
        
        public void LoadImages(object obj, EventArgs args) {
        }
    }
    
    public class HideReadAction : ToggleAction {
        public HideReadAction(Browser browser) : base("Hide_read", "_Hide ReadSumma.Data.Items", null, null) {
            Tooltip = "Hide read items in the item list";
            Toggled += HideRead;
            
            Sensitive = false;
            Active = false;
        }
        
        public void HideRead(object obj, EventArgs args) {
        }
    }
    
    public class NextItemAction : Gtk.Action {
        private Browser browser;
        
        public NextItemAction(Browser browser) : base("Next_item", "_NextSumma.Data.Item") {
            this.browser = browser;
            
            Tooltip = "Go to next unread item";
            StockId = Stock.GoForward;
            Activated += GoToNextItem;
        }
        
        public void GoToNextItem(object obj, EventArgs args) {
            bool win = browser.ItemView.GoToNextItem();
            if (!win) {
                bool nextfeed = browser.FeedView.GoToNextUnreadFeed();
                
                if ( nextfeed ) {
                    browser.ItemView.Populate(browser.FeedView.Selected);
                    
                    browser.ItemView.GoToPreviousItem();
                    browser.UpdateHtmlview();
                } else {
                    Notifier.Notify("There are no more unread items.");
                }
            } else {
                browser.UpdateHtmlview();
            }
        }
    }
    
    public class PreviousItemAction : Gtk.Action {
        private Browser browser;
        
        public PreviousItemAction(Browser browser) : base("Previous_item", "_PreviousSumma.Data.Item") {
            this.browser = browser;
            
            Tooltip = "Go to the previous item";
            StockId = Stock.GoBack;
            Activated += GoToPreviousItem;
        }
        
        public void GoToPreviousItem(object obj, EventArgs args) {
            browser.ItemView.GoToPreviousItem();
            if ( browser.ItemView.HasSelected ) {
                browser.UpdateHtmlview();
            }
        }
    }
    
    public class FlagAction : Gtk.Action {
        private Browser browser;
        private Summa.Data.Item item;
        
        public FlagAction(Browser browser) : base("Flag_item", "Flag This Item") {
            this.browser = browser;
            
            Tooltip = "Flag/unflag this item";
            Activated += Flag;
        }
        
        public void Flag(object obj, EventArgs args) {
            browser.ItemView.MarkSelectedFlagged();
            Populate(item);
        }
        
        public void Populate(Summa.Data.Item item) {
            this.item = item;
            
            if ( item.Flagged ) {
                Label = "Unflag This Item";
            } else {
                Label = "Flag This Item";
            }
            
            Sensitive = true;
        }
    }
    
    public class UnreadAction : Gtk.Action {
        private Browser browser;
        private Summa.Data.Item item;
        
        public UnreadAction(Browser browser) : base("Read_item", "Mark As Unread") {
            this.browser = browser;
            
            Tooltip = "Mark this item as unread/read";
            IconName = "feed-item";
            Activated += Mark;
        }
        
        public void Mark(object obj, EventArgs args) {
            if ( browser.ItemView.HasSelected ) {
                if ( browser.ItemView.Selected.Read ) {
                    browser.ItemView.Selected.Read = false;
                } else {
                    browser.ItemView.Selected.Read = true;
                }
            }
            Populate(item);
        }
        
        public void Populate(Summa.Data.Item item) {
            this.item = item;
            
            if ( item.Read ) {
                Label = "Mark As Unread";
            } else {
                Label = "Mark As Read";
            }
            
            Sensitive = true;
        }
    }
    
    public class EnclosureAction : Gtk.Action {
        private Browser browser;
        private IMediaPlayer mediaplayer;
        
        public EnclosureAction(Browser browser) : base("Play_pause", "_Play") {
            this.browser = browser;
            
            Tooltip = "Play or pause the media enclosed";
            StockId = Stock.MediaPlay;
            Activated += Play;
            
            mediaplayer = new TotemMediaPlayer();
        }
        
        public void Play(object obj, EventArgs args) {
            if ( browser.play_action.StockId == Stock.MediaPause ) {
                mediaplayer.Pause();
                SetToPlay();
            } else {
                mediaplayer.Play(browser.ItemView.Selected.EncUri);
                SetToPause();
            }
        }
        
        public void Populate(Summa.Data.Item item) {
            if ( item.EncUri != "" ) {
                Sensitive = true;
            } else {
                Sensitive = false;
            }
        }
        
        public void SetToPlay() {
            Label = "_Play";
            StockId = Stock.MediaPlay;
        }
        
        public void SetToPause() {
            Label = "_Pause";
            StockId = Stock.MediaPause;
        }
    }
    
    public class SaveEnclosureAction : Gtk.Action {
        public SaveEnclosureAction(Browser browser) : base("Save_enclosed", "_Save Enclosed Media") {
            Tooltip = "Save the enclosed media";
            StockId = Stock.Save;
            Activated += Save;
            Sensitive = false;
        }
        
        public void Save(object obj, EventArgs args) {
        }
        
        public void Populate(Summa.Data.Item item) {
            if ( item.EncUri != "" ) {
                Sensitive = false;
            } else {
                Sensitive = false;
            }
        }
    }
    
    public class HelpAction : Gtk.Action {
        public HelpAction(Browser browser) : base("Contents", "_Contents") {
            Tooltip = "Get help";
            StockId = Stock.Help;
            Activated += Help;
            Sensitive = false;
        }
        
        public void Help(object obj, EventArgs args) {
        }
    }
    
    public class AboutAction : Gtk.Action {
        private Browser browser;
        
        public AboutAction(Browser browser) : base("About", "_About") {
            this.browser = browser;
            
            Tooltip = "About";
            StockId = Stock.About;
            Activated += ShowAbout;
        }
        
        public void ShowAbout(object obj, EventArgs args) {
            Window w = new Summa.Gui.AboutDialog();
            w.TransientFor = browser;
            w.ShowAll();
        }
    }
    
    public class WidescreenViewAction : RadioAction {
        public WidescreenViewAction(Browser browser) : base("Wide_view", "_Use Widescreen View", null, null, 0) {
            Tooltip = "Use the widescreen view";
            Activated += SetView;
            
            if ( Config.WidescreenView ) {
                CurrentValue = Value;
            }
            
            //Notifier.ViewChanged += OnViewChanged;
        }
        
        public void SetView(object obj, EventArgs args) {
            if ( Summa.Core.Application.Browsers.Count > 0 ) {
                Config.WidescreenView = true;
            }
        }
    }
    
    public class NormalViewAction : RadioAction {
        public NormalViewAction(Browser browser) : base("Normal_view", "_Use Normal View", null, null, 0) {
            Tooltip = "Use the normal view";
            Activated += SetView;
            
            if ( !Config.WidescreenView ) {
                CurrentValue = Value;
            }
            
            //Notifier.ViewChanged += OnViewChanged;
        }
        
        public void SetView(object obj, EventArgs args) {
            if ( Summa.Core.Application.Browsers.Count > 0 ) {
                Config.WidescreenView = false;
            }
        }
    }
}
