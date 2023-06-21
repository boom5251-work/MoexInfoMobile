using MoexInfoMobile.HtmlFormat;
using MoexInfoMobile.Iss.Api;
using MoexInfoMobile.Iss.Data;
using MoexInfoMobile.StringPatterns;
using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoexInfoMobile.Pages
{
    /// <summary>
    /// Информационная страницы для новостей и событий.<br />
    /// Взаимодействие InfoPage.xaml
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CellInfoPage : ContentPage
    {
        public CellInfoPage(ulong infoId, SiteInfoType infoType)
        {
            InitializeComponent();

            if (infoType == SiteInfoType.HeadlineInfo)
                LoadHeadlineInfo(infoId);
            else if (infoType == SiteInfoType.EventInfo)
                LoadEventInfo(infoId);
        }



        /// <summary>
        /// Загружает подробную информацию о новости.
        /// </summary>
        /// <param name="headlineId">Идентификатор новости.</param>
        private async void LoadHeadlineInfo(ulong headlineId)
        {
            // Получение информации о новости.
            var headlineInfo = await SiteNews.GetHeadlineInfo(headlineId);

            // Обработка окончания выполнения загрузки информации о новости.
            if (headlineInfo != null)
            {
                InitializeContent(headlineInfo);
            }
            else
            {
                // TODO: Изменить сообщение об ошибке.
                App.Os.ShowToastNotification("Не удалось загрузить информацию");
                loadingIndicator.IsVisible = false;
            }
        }


        /// <summary>
        /// Загружает подробную информацию о событии.
        /// </summary>
        /// <param name="eventInfoId">Идентификатор события.</param>
        private async void LoadEventInfo(ulong eventInfoId)
        {
            var eventInfo = await Events.GetEventInfo(eventInfoId);
            // TODO: Реализовать отображение.
        }


        /// <summary>
        /// Инициализирует содержимое страницы.
        /// </summary>
        /// <param name="headlineInfo">Новостная запись.</param>
        private void InitializeContent(HeadlineInfo headlineInfo)
        {
            // Преобразование даты публикации.
            string publishedAt = headlineInfo.PublishedAt.ToString(DateTimePatterns.DateTimeFormat, CultureInfo.InvariantCulture);

            // Создание элементов интерфейса.
            var htmlDecoder = new HtmlDecoder(htmlStyles);

            // Добавление элементов извлеченных из HTML.
            if (HtmlDecoder.IsHtmlText(headlineInfo.HtmlBody))
            {
                var views = htmlDecoder.DecodeHtml(headlineInfo.HtmlBody);
                AddViews(headlineInfo.Title, publishedAt, views.ToArray());
            }
            // Добавление текста (содержимое является текстом, а не разметкой).
            else
            {
                var label = new Label
                {
                    Text = headlineInfo.HtmlBody,
                    Style = labelStyle
                };

                AddViews(headlineInfo.Title, publishedAt, label);
            }
        }


        /// <summary>
        /// Инициализирует содержимое страницы.
        /// </summary>
        /// <param name="eventInfo">Информация о событии.</param>
        private void InitializeContent(EventInfo eventInfo)
        {
            string from = eventInfo.From.ToString(DateTimePatterns.DateTimeFormat, CultureInfo.InvariantCulture);
            string till = eventInfo.Till.ToString(DateTimePatterns.DateTimeFormat, CultureInfo.InvariantCulture);

            string info = $"Начало: {from}\nКонец: {till}\nМесто проведения: {eventInfo.Place}\nОрганизатор: {eventInfo.Organizer}";

            // Создание элементов интерфейса.
            var htmlDecoder = new HtmlDecoder(htmlStyles);

            // Добавление элементов извлеченных из HTML.
            if (HtmlDecoder.IsHtmlText(eventInfo.HtmlBody))
            {
                var views = htmlDecoder.DecodeHtml(eventInfo.HtmlBody);
                AddViews(eventInfo.Title, info, views.ToArray());
            }
            // Добавление текста (содержимое является текстом, а не разметкой).
            else
            {
                var label = new Label
                {
                    Text = eventInfo.HtmlBody,
                    Style = labelStyle
                };

                AddViews(eventInfo.Title, info, label);
            }
        }


        /// <summary>
        /// Отображает текстовую информацию на странице.
        /// </summary>
        /// <param name="title">Заголовок.</param>
        /// <param name="info">Дата и время публикации.</param>
        /// <param name="body">Элементы представления.</param>
        private void AddViews(string title, string info, params View[] body)
        {
            // Установка даты публикации, заголовка и тела.
            loadingIndicator.IsVisible = false;

            titleLabel.Text = title;
            infoLabel.Text = info;

            foreach (var view in body)
            {
                if (view != null)
                    scrollDataContainer.Children.Add(view);
            }
        }


        /// <summary>
        /// Обрабатывает событие нажатия на стрелку назад.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private async void BackImageButton_Clicked(object sender, EventArgs e) =>
            await Navigation.PopModalAsync();
    }
}