// ISource.cs
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

using Summa.Core;

namespace Summa.Data {
    public interface ISource {
        string Name { get; set; }
        string Url { get; set; }
        string Author { get; set; }
        string Subtitle { get; set; }
        string License { get; set; }
        string Image { get; set; }
        string Status { get; set; }
        ArrayList Tags { get; set; }
        Gdk.Pixbuf Favicon { get; set; }
        int UnreadCount { get; }
        bool HasUnread { get; }
        ArrayList Items { get; }
        
        void AppendTag(string tag);
        void RemoveTag(string tag);
        
        bool Update();
        
        void MarkItemsRead();
    }
}
