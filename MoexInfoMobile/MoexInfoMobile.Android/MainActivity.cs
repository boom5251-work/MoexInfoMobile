using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;

namespace MoexInfoMobile.Droid
{
    [Activity(Label = "iMOEX", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
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


        public void ShowToastNotification(string message, bool isLong = false)
        {
            // Установка продолжительности уведомления.
            ToastLength length = isLong ? ToastLength.Long : ToastLength.Short;

            // Создание уведомления.
            var notification = Toast.MakeText(ApplicationContext, message, length);

            // Отображение уведомления.
            notification.Show();
        }


        public void ChangeStatusBarColor(Color color) =>
            Window.SetStatusBarColor(color.ToAndroid());
    }
}