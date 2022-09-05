using MoexInfoMobile.Pages;
using Xamarin.Forms;

namespace MoexInfoMobile
{
    public partial class App : Application
    {
        public App(OperatingSystem os)
        {
            InitializeComponent();

            Os = os;
            MainPage = new NavigationPage(new MainPage());
        }



        /// <summary>
        /// Интерфейс опреационных систем Android и IOS.
        /// </summary>
        public static OperatingSystem Os { get; private set; }



        protected override void OnStart()
        {
            return;
        }

        protected override void OnSleep()
        {
            return;
        }

        protected override void OnResume()
        {
            return;
        }
    }
}