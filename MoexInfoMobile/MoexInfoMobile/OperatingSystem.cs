using Xamarin.Forms;

namespace MoexInfoMobile
{
    public interface OperatingSystem
    {
        void ShowToastNotification(string message, bool isLong = false);
        void ChangeStatusBarColor(Color color);
    }
}
