using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Windows.UI.ViewManagement;

namespace FileManager.Services
{
    internal static class WIndowsAccentColorsService
    {
        static readonly UISettings uiSettings = new();


        public static Color Accent()
        {
            var accentColor = uiSettings.GetColorValue(UIColorType.Accent);

            return Color.FromArgb(accentColor.A, accentColor.R, accentColor.G, accentColor.B);
        }

        public static Color AccentDark1()
        {
            var accentColor = uiSettings.GetColorValue(UIColorType.AccentDark1);

            return Color.FromArgb(accentColor.A, accentColor.R, accentColor.G, accentColor.B);
        }

        public static Color AccentDark2()
        {
            var accentColor = uiSettings.GetColorValue(UIColorType.AccentDark2);

            return Color.FromArgb(accentColor.A, accentColor.R, accentColor.G, accentColor.B);
        }

        public static Color AccentDark3()
        {
            var accentColor = uiSettings.GetColorValue(UIColorType.AccentDark3);

            return Color.FromArgb(accentColor.A, accentColor.R, accentColor.G, accentColor.B);
        }

        public static Color AccentLight1()
        {
            var accentColor = uiSettings.GetColorValue(UIColorType.AccentLight1);

            return Color.FromArgb(accentColor.A, accentColor.R, accentColor.G, accentColor.B);
        }

        public static Color Background()
        {
            var accentColor = uiSettings.GetColorValue(UIColorType.Background);

            return Color.FromArgb(accentColor.A, accentColor.R, accentColor.G, accentColor.B);
        }

        public static Color Foreground()
        {
            var accentColor = uiSettings.GetColorValue(UIColorType.Foreground);

            return Color.FromArgb(accentColor.A, accentColor.R, accentColor.G, accentColor.B);
        }

        public static Color Complement()
        {
            var accentColor = uiSettings.GetColorValue(UIColorType.Complement);

            return Color.FromArgb(accentColor.A, accentColor.R, accentColor.G, accentColor.B);
        }

    }
}
