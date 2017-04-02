using System.Runtime.InteropServices;
using System.Windows.Media;

namespace ambi_gui.Views.Theme
{
    public static class WindowsColorizationProvider
    {
        [DllImport("dwmapi.dll", EntryPoint = "#127")]
        private static extern void DwmGetColorizationParameters(out DwmColorizationParameters parameters);

        public static Color GetWindowColorizationColor()
        {
            DwmGetColorizationParameters(out DwmColorizationParameters colorizationParameters);

            return Color.FromArgb(
                (byte) ( /*colorizationParameters.fOpaque ? 255 :*/ colorizationParameters.clrColor >> 24),
                (byte) (colorizationParameters.clrColor >> 16),
                (byte) (colorizationParameters.clrColor >> 8),
                (byte) colorizationParameters.clrColor
            );
        }

        private struct DwmColorizationParameters
        {
            public uint clrColor;
            public uint clrAfterGlow;
            public uint nIntensity;
            public uint clrAfterGlowBalance;
            public uint clrBlurBalance;
            public uint clrGlassReflectionIntensity;
            public bool fOpaque;
        }
    }
}
