using Xamarin.Forms;

namespace MoexInfoMobile
{
    /// <summary>
    /// Интерфейс операционных систем.
    /// </summary>
    public interface OperatingSystem
    {
        /// <summary>
        /// Отображает TOAST-уведомление.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="isLong">Является ли сообщение продолжительным.</param>
        void ShowToastNotification(string message, bool isLong = false);


        /// <summary>
        /// Изменяет цвета верхней панели.
        /// </summary>
        /// <param name="color">Новый цвет.</param>
        void ChangeStatusBarColor(Color color);
    }
}