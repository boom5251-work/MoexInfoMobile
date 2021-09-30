using Xamarin.Forms;

namespace MoexInfoMobile.Custom
{
    public class BackgroundFrame : Frame
    {
        // Слои фона элемента.
        public BackgroundResource BackgroundResource
        {
            get { return (BackgroundResource)GetValue(BackgroundResourceProperty); }
            set { SetValue(BackgroundResourceProperty, value); }
        }

        public static readonly BindableProperty BackgroundResourceProperty =
            BindableProperty.Create(nameof(BackgroundResource), typeof(BackgroundResource), typeof(BackgroundFrame), default(BackgroundResource));
    }
}