using MoexInfoMobile.Html;
using MoexInfoMobile.Iss.Api;
using MoexInfoMobile.Iss.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoexInfoMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CellInfoPage : ContentPage
    {
        public CellInfoPage(ulong headlineId)
        {
            InitializeComponent();
            ShowHeadlineInfo(headlineId);
        }



        // Метод запускает выполнение задачи загрузки подробной информации о новсти.
        private void ShowHeadlineInfo(ulong headlineId)
        {
            /// Получение информации о новсти.
            Task<HeadlineInfo> headlineInfoTask = SiteNews.GetNewsInfo(headlineId);

            /// Обработка окончания выполнения загрузки информации о новости.
            headlineInfoTask.ContinueWith((Task<HeadlineInfo> task) =>
            {
                HeadlineInfo headlineInfo = task.Result;

                /// Преобразование даты побликации.
                string format = "dd.MM.yyyy HH:mm";
                string publishedAt = headlineInfo.PublishedAt.ToString(format, CultureInfo.InvariantCulture);

                /// Вычленение данных из html-тегов.
                HtmlDecoder htmlDecoder = new HtmlDecoder
                {
                    Resources = Resources,
                    CellsBackgroundColor = (Color)Application.Current.Resources["ClassicChalk"],
                    EmptyCellsBackgroundColor = (Color)Application.Current.Resources["LightGrey"]
                };

                bool isEncoded = htmlDecoder.EncodeHtml(headlineInfo.Body, out LinkedList<View> views);

                /// Добавление элементов на страницу.
                if (isEncoded)
                {
                    Dispatcher.BeginInvokeOnMainThread(() =>
                    {
                        /// Выключение индикатора загрузки.
                        loadingIndicator.IsVisible = false;
                        /// Добавление элементов на страницу.
                        foreach (View view in views)
                            scrollDataContainer.Children.Add(view);
                    });
                }
                else
                {
                    Dispatcher.BeginInvokeOnMainThread(() =>
                    {
                        /// Выключение индикатора загрузки.
                        loadingIndicator.IsVisible = false;
                        /// Добавление текста на страницу.
                        Label label = new Label
                        {
                            Text = headlineInfo.Body,
                            Style = (Style)Resources["BodyLabel"]
                        };
                    });
                }

                /// Установка даты публикации и заголовка.
                Dispatcher.BeginInvokeOnMainThread(() =>
                {
                    titleLabel.Text = headlineInfo.Title;
                    publishedAtLabel.Text = publishedAt;
                });
            });
        }



        // Обработка события нажатия на стрелку назад.
        private async void BackImageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}