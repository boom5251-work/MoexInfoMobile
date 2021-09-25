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

            NavigationPage navigationPage = new NavigationPage(new MainPage());
            MainPage = navigationPage;
        }



        public static OperatingSystem Os { get; private set; } /// Интерфейс опреационных систем Android и IOS.



        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
