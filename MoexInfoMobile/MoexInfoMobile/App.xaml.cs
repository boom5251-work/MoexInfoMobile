using Xamarin.Forms;

namespace MoexInfoMobile
{
    public partial class App : Application
    {
        public App(OperatingSystem os)
        {
            InitializeComponent();

            Os = os;
            MainPage = new MainPage();
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
