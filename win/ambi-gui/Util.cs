using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;

namespace ambi_gui
{
    internal static class Util
    {
        private const int DEVICE_CAP_VERTES = 10;
        private const int DEVICE_CAP_DESKTOPVERTES = 117;

        static Util()
        {
            var factor = GetScalingFactor();
            var virtualScreen = SystemInformation.VirtualScreen;

            var width = (int) Math.Ceiling(factor * virtualScreen.Width);
            var height = (int) Math.Ceiling(factor * virtualScreen.Height);

            UnscaledBounds = new Rect(
                virtualScreen.Location.X,
                virtualScreen.Location.Y,
                width,
                height
            );
        }

        public static Rect UnscaledBounds { get; }

        private static double GetScalingFactor()
        {
            var g = Graphics.FromHwnd(IntPtr.Zero);
            var desktop = g.GetHdc();
            var screenVertres = GetDeviceCaps(desktop, DEVICE_CAP_VERTES);
            var desktopVertres = GetDeviceCaps(desktop, DEVICE_CAP_DESKTOPVERTES);

            return desktopVertres / (double) screenVertres;
        }

        [DllImport("gdi32.dll")]
        private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
    }
}