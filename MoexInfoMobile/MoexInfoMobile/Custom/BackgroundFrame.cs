using Xamarin.Forms;

namespace MoexInfoMobile.Custom
{
    /// <summary>
    /// Пользовательский фрейм.
    /// </summary>
    public class BackgroundFrame : Frame
    {
        /// <summary>Слои фона элемента.</summary>
        public BackgroundResource BackgroundResource
        {
            get { return (BackgroundResource)GetValue(BackgroundResourceProperty); }
            set { SetValue(BackgroundResourceProperty, value); }
        }

        public static readonly BindableProperty BackgroundResourceProperty =
            BindableProperty.Create(nameof(BackgroundResource), typeof(BackgroundResource), typeof(BackgroundFrame), default(BackgroundResource));
    }
}