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



        // Метод загружает подробную информацию о новсти.
        private void GetHeadlineInfo(ulong headlineId)
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

                /// Создание элементов интерфейса.
                HtmlDecoder htmlReader = new HtmlDecoder(Resources);
                
                if (htmlReader.IsHtmlText(headlineInfo.Body))
                {
                    try
                    {
                        List<View> views = htmlReader.DecodeHtml(headlineInfo.Body);
                        ShowInfo(headlineInfo.Title, publishedAt, views.ToArray());
                    }
                    catch (XmlException ex)
                    {
                        // TODO: XmlExeption
                        Debug.WriteLine(ex);
                    }
                }
                else
                {
                    Label label = new Label
                    {
                        Text = headlineInfo.Body,
                        Style = labelStyle
                    };

                    ShowInfo(headlineInfo.Title, publishedAt, label);
                }
            });
        }



        // Метод загружает подробную информацию о событии.
        private void GetEventInfo(ulong eventInfoId)
        {
            // TODO: Добавить логику выполнения.
            throw new NotImplementedException();
        }



        // Метод отображает информацию на странице.
        private void ShowInfo(string title, string publishedAt, params View[] body)
        {
            /// Установка даты публикации, заголовка и тела.
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



        // Обработка события нажатия на стрелку назад.
        private async void BackImageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}