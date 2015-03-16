using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace KLibrary.Labs.UI
{
    /// <summary>
    /// Provides the helper methods for screen.
    /// </summary>
    public static class ScreenHelper
    {
        /// <summary>
        /// Gets the bounds of all screens.
        /// </summary>
        /// <value>The bounds of all screens.</value>
        public static Int32Rect AllScreensBounds
        {
            get { return _AllScreensBounds.Value; }
        }

        static readonly Lazy<Int32Rect> _AllScreensBounds = new Lazy<Int32Rect>(() => SystemInformation.VirtualScreen.ToInt32Rect());

        /// <summary>
        /// Gets the bounds of the primary screen.
        /// </summary>
        /// <value>The bounds of the primary screen.</value>
        public static Int32Rect PrimaryScreenBounds
        {
            get { return _PrimaryScreenBounds.Value; }
        }

        static readonly Lazy<Int32Rect> _PrimaryScreenBounds = new Lazy<Int32Rect>(() => Screen.PrimaryScreen.Bounds.ToInt32Rect());

        public static int GetScreenIndex(Int32Rect rect)
        {
            var screen = Screen.FromRectangle(rect.ToRectangle());
            return Array.IndexOf(Screen.AllScreens, screen);
        }

        public static Int32Rect GetScreenBounds(Int32Rect rect)
        {
            return Screen.GetBounds(rect.ToRectangle()).ToInt32Rect();
        }

        public static Point GetLeftTop(this Int32Rect rect)
        {
            return new Point(rect.X, rect.Y);
        }

        public static Point GetLeftBottom(this Int32Rect rect)
        {
            return new Point(rect.X, rect.Y + rect.Height - 1);
        }

        public static Point GetRightTop(this Int32Rect rect)
        {
            return new Point(rect.X + rect.Width - 1, rect.Y);
        }

        public static Point GetRightBottom(this Int32Rect rect)
        {
            return new Point(rect.X + rect.Width - 1, rect.Y + rect.Height - 1);
        }
    }
}
