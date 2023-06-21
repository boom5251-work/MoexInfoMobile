using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoexInfoMobile.Custom.Html
{
    /// <summary>
    /// Элемент управления, представляющий текст-ссылку.<br />
    /// Логика взаимодействия с HtmlLinkLabel.xaml
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HtmlLinkLabel : Label
    {
        public HtmlLinkLabel()
        {
            InitializeComponent();
            tapGestureRecognizer.Command = new Command(OnTap);
        }



        /// <summary>
        /// URI ссылки.
        /// </summary>
        public Uri? Uri { get; set; }



        /// <summary>
        /// Открывет браузер при нажатии на элемент.
        /// </summary>
        private async void OnTap()
        {
            try
            {
                await Browser.OpenAsync(Uri, BrowserLaunchMode.External);
            }
            catch
            {
                // TODO: Обработать исключения.
            }
        }
    }
}