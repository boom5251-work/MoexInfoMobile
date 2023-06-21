using Xamarin.Forms;

namespace MoexInfoMobile.Resources.Colors
{
    internal abstract class Colors
    {
        protected static ResourceDictionary AppResources => Application.Current.Resources;


        /// <summary>
        /// Белый ластик.
        /// </summary>
        internal static Color ClassicChalk => (Color)AppResources["ClassicChalk"];

        /// <summary>
        /// Светло-серый.
        /// </summary>
        internal static Color LightGrey => (Color)AppResources["LightGrey"];

        /// <summary>
        /// Темно-серый.
        /// </summary>
        internal static Color MidnightBadger => (Color)AppResources["MidnightBadger"];

        /// <summary>
        /// Алый.
        /// </summary>
        internal static Color MoexScarlet => (Color)AppResources["MoexScarlet"];

        /// <summary>
        /// Серый.
        /// </summary>
        internal static Color WolfGrey => (Color)AppResources["WolfGrey"];
    }
}