using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace KLibrary.Labs.UI
{
    public static class Convert
    {
        public static Int32Rect ToInt32Rect(this System.Drawing.Rectangle r)
        {
            return new Int32Rect(r.X, r.Y, r.Width, r.Height);
        }
    }
}
