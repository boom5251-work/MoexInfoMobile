using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;


namespace MoexInfoMobile.Droid
{
    [Activity(Label = "MoexInfoMobile", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : FormsAppCompatActivity, OperatingSystem
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);
            LoadApplication(new App(this));
        }



        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }



        // Метод создает toast-уведомление.
        public void ShowToastNotification(string message, bool isLong = false)
        {
            /// Установка продолжительности уведомления.
            ToastLength length;

            if (isLong)
            {
                length = ToastLength.Long;
            }
            else
            {
                length = ToastLength.Short;
            }

            /// Создание уведомления.
            Toast notification = Toast.MakeText(ApplicationContext, message, length);

            /// Отображение уведомления.
            notification.Show();
        }



        // Метод изменяет цвет statusBar.
        public void ChangeStatusBarColor(Color color)
        {
            Window.SetStatusBarColor(color.ToAndroid());
        }
    }
}