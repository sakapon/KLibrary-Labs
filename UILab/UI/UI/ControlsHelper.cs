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

        public static void FullScreenForCurrent(this Window window)
        {
            // TODO: Assert loaded or initialized.
            var rect = new Int32Rect((int)window.Left, (int)window.Top, (int)window.ActualWidth, (int)window.ActualHeight);
            window.Relocate(ScreenHelper.GetScreenBounds(rect));
        }
    }
}
