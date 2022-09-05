using MoexInfoMobile.HtmlFormat;
using MoexInfoMobile.Iss.Api;
using MoexInfoMobile.Iss.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoexInfoMobile.Pages
{
    /// <summary>
    /// Информационная страницы для новостей и событий.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CellInfoPage : ContentPage
    {
        public CellInfoPage(ulong infoId, SiteInfoType infoType)
        {
            InitializeComponent();

            if (infoType == SiteInfoType.HeadlineInfo)
            {
                GetHeadlineInfo(infoId);
            }
            else if (infoType == SiteInfoType.EventInfo)
            {
                GetEventInfo(infoId);
            }
        }



        /// <summary>
        /// Загружает подробную информацию о новсти.
        /// </summary>
        /// <param name="headlineId">Идентификатор новости.</param>
        private void GetHeadlineInfo(ulong headlineId)
        {
            // Получение информации о новсти.
            Task<HeadlineInfo> headlineInfoTask = SiteNews.GetNewsInfo(headlineId);

            // Обработка окончания выполнения загрузки информации о новости.
            headlineInfoTask.ContinueWith((Task<HeadlineInfo> task) =>
            {
                HeadlineInfo headlineInfo = task.Result;

                // Преобразование даты побликации.
                string format = "dd.MM.yyyy HH:mm";
                string publishedAt = headlineInfo.PublishedAt.ToString(format, CultureInfo.InvariantCulture);

                // Создание элементов интерфейса.
                HtmlDecoder htmlDecoder = new HtmlDecoder(htmlStyles);
                
                if (HtmlDecoder.IsHtmlText(headlineInfo.HtmlBody))
                {
                    try
                    {
                        List<View> views = htmlDecoder.DecodeHtml(headlineInfo.HtmlBody);
                        ShowInfo(headlineInfo.Title, publishedAt, views.ToArray());
                    }
                    catch (XmlException ex)
                    {
                        // TODO: XmlExeption
                        Debug.WriteLine(ex);
                    }
                    catch
                    {
                        // TODO: Добавить обработку исключений.
                    }
                }
                else
                {
                    Label label = new Label
                    {
                        Text = headlineInfo.HtmlBody,
                        Style = labelStyle
                    };

                    ShowInfo(headlineInfo.Title, publishedAt, label);
                }
            });
        }



        /// <summary>
        /// Загружает подробную информацию о событии.
        /// </summary>
        /// <param name="eventInfoId">Идентификатор события.</param>
        private void GetEventInfo(ulong eventInfoId)
        {
            // TODO: Добавить логику выполнения.
            throw new NotImplementedException();
        }



        /// <summary>
        /// Отображает информацию на странице.
        /// </summary>
        /// <param name="title">Заголовок.</param>
        /// <param name="publishedAt">Дата и время публикации.</param>
        /// <param name="body">Элементы представления.</param>
        private void ShowInfo(string title, string publishedAt, params View[] body)
        {
            // Установка даты публикации, заголовка и тела.
            Dispatcher.BeginInvokeOnMainThread(() =>
            {
                loadingIndicator.IsVisible = false;

                titleLabel.Text = title;
                publishedAtLabel.Text = publishedAt;

                foreach (View view in body)
                {
                    if (view != null)
                        scrollDataContainer.Children.Add(view);
                }
            });
        }



        /// <summary>
        /// Обрабатывает событие нажатия на стрелку назад.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Агрументы события</param>
        private async void BackImageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}