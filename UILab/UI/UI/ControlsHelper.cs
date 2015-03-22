using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace KLibrary.Labs.UI
{
    /// <summary>
    /// Provides the helper methods for controls.
    /// </summary>
    public static class ControlsHelper
    {
        /// <summary>
        /// Makes the window borderless and enables transparency.
        /// Can not be called after the window has been shown.
        /// </summary>
        /// <param name="window">The window.</param>
        public static void SetBorderless(this Window window)
        {
            window.WindowStyle = WindowStyle.None;
            window.AllowsTransparency = true;
        }

        /// <summary>
        /// Relocates and resizes the window.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="bounds">The location and the size.</param>
        public static void Relocate(this Window window, Int32Rect bounds)
        {
            var scale = window.GetScreenScale();

            window.Left = bounds.X / scale;
            window.Top = bounds.Y / scale;
            window.Width = bounds.Width / scale;
            window.Height = bounds.Height / scale;
        }

        public static void FullScreenForAll(this Window window)
        {
            window.Relocate(ScreenHelper.AllScreensBounds);
        }

        public static void FullScreenForPrimary(this Window window)
        {
            window.Relocate(ScreenHelper.PrimaryScreenBounds);
        }

        public static void FullScreenFor(this Window window, int index)
        {
            window.Relocate(ScreenHelper.GetScreenBounds(index));
        }

        public static void FullScreenForCurrent(this Window window)
        {
            var bounds = window.GetBounds();
            window.Relocate(ScreenHelper.GetScreenBounds(bounds));
        }

        public static int GetScreenIndex(this Window window)
        {
            var bounds = window.GetBounds();
            return ScreenHelper.GetScreenIndex(bounds);
        }

        public static Int32Rect GetBounds(this Window window)
        {
            if (!window.IsLoaded) throw new InvalidOperationException("The window has not been loaded.");

            return new Int32Rect((int)window.Left, (int)window.Top, (int)window.ActualWidth, (int)window.ActualHeight);
        }

        public static double GetScreenScale(this Window window)
        {
            if (!window.IsLoaded) throw new InvalidOperationException("The window has not been loaded.");

            var v = window.PointToScreen(new Point(100, 0)) - window.PointToScreen(new Point(0, 0));
            return v.X / 100;
        }
    }
}
