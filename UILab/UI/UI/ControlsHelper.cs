using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace KLibrary.Labs.UI
{
    public static class ControlsHelper
    {
        public static void SetBorderless(this Window window)
        {
            window.AllowsTransparency = true;
            window.WindowStyle = WindowStyle.None;
        }

        public static void Relocate(this Window window, Int32Rect bounds)
        {
            window.Left = bounds.X;
            window.Top = bounds.Y;
            window.Width = bounds.Width;
            window.Height = bounds.Height;
        }

        public static void FullScreenForAll(this Window window)
        {
            window.Relocate(ScreenHelper.AllScreensBounds);
        }

        public static void FullScreenForPrimary(this Window window)
        {
            window.Relocate(ScreenHelper.PrimaryScreenBounds);
        }
    }
}
