using Xamarin.Forms;

namespace MoexInfoMobile
{
    /// <summary>
    /// Интерфейс операционных систем.
    /// </summary>
    public interface OperatingSystem
    {
        /// <summary>
        /// Отображает тост-уведомление.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="isLong">Большая продолжительность.</param>
        void ShowToastNotification(string message, bool isLong = false);


        /// <summary>
        /// Изменение цвета верхней панели.
        /// </summary>
        /// <param name="color">Новый цвет.</param>
        void ChangeStatusBarColor(Color color);
    }
}