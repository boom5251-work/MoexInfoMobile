using MoexInfoMobile.Iss.Api;
using MoexInfoMobile.Iss.Data;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoexInfoMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeadlineInfoPage : ContentPage
    {
        public HeadlineInfoPage(long headlineId)
        {
            InitializeComponent();
            ShowHeadlineInfo(headlineId);
        }



        // Метод запускает выполнение задачи загрузки подробной информации о новсти.
        private void ShowHeadlineInfo(long headlineId)
        {
            /// Получение информации о новсти.
            Task<HeadlineInfo> headlineInfoTask = SiteNews.GetNewsInfo(headlineId);

            /// Обработка окончания выполнения загрузки информации о новости.
            headlineInfoTask.ContinueWith((Task<HeadlineInfo> task) =>
            {
                HeadlineInfo headlineInfo = task.Result;

                if (headlineInfo != null)
                {
                    Dispatcher.BeginInvokeOnMainThread(() =>
                    {
                        /// Скрытие индикатора загрузки.
                        loadingIndicator.IsVisible = false;
                        /// Установка заголовка и информации новости.
                        titleLabel.Text = headlineInfo.Title;
                        bodyLabel.Text = headlineInfo.Body;
                        /// Установка даты публикации.
                        string format = "dd.MM.yyyy HH:mm";
                        string publishedAt = headlineInfo.PublishedAt.ToString(format, CultureInfo.InvariantCulture);
                        publishedAtLabel.Text = publishedAt;
                    });
                }
            });
        }



        // Обработка события нажатия на стрелку назад.
        private async void BackImageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}