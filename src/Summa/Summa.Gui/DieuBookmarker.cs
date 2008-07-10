// DieuBookmarker.cs
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
using NDesk.DBus;

[Interface ("org.gnome.Dieu")]
public interface Dieu {
    void AddBookmark (string title, string url, string content, string tags);
}

namespace Summa.Gui {
    public class DieuBookmarker : Summa.Interfaces.IBookmarker {
        private Dieu dieu;
        private bool possible;
        
        public DieuBookmarker() {
            Dieu dieu = Bus.Session.GetObject<Dieu>("org.gnome.Dieu", new ObjectPath("/org/gnome/Dieu"));
            possible = true;
        }
        
        public void ShowBookmarkWindow(string title, string url, string content, string tags) {
            dieu.AddBookmark(title, url, content, tags);
        }
        
        public bool CanBookmark() {
            return possible;
        }
        
        public ArrayList GetBookmarks() {
            return new ArrayList();
        }
    }
}