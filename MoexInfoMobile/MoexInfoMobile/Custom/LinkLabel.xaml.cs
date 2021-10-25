using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoexInfoMobile.Custom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LinkLabel : Label
    {
        public LinkLabel()
        {
            InitializeComponent();
            tapGestureRecognizer.Command = new Command(OnTap);
        }


        public Uri Uri { get; set; } // Путь.


        // Метод открывет браузер при нажатии на элемент.
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