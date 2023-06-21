using Xamarin.Forms;

namespace MoexInfoMobile.Custom
{
    /// <summary>
    /// Пользовательский фрейм.
    /// </summary>
    public class BackgroundFrame : Frame
    {
        /// <summary>
        /// Привязка: слои фона элемента.
        /// </summary>
        public static readonly BindableProperty BackgroundResourceProperty =
            BindableProperty.Create(nameof(BackgroundResource), typeof(BackgroundResource), typeof(BackgroundFrame), default(BackgroundResource));



        /// <summary>
        /// Слои фона элемента.
        /// </summary>
        public BackgroundResource BackgroundResource
        {
            get => (BackgroundResource)GetValue(BackgroundResourceProperty);
            set => SetValue(BackgroundResourceProperty, value);
        }
    }
}