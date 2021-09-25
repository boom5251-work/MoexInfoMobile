using MoexInfoMobile.Custom;
using MoexInfoMobile.Iss.Api;
using MoexInfoMobile.Iss.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoexInfoMobile.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            ShowNews();

            Appearing += MainPage_Appearing;
        }



        private uint loadedHeadlinesCount = 0;



        // Метод запускает выполнение задачи загрузки списка новостей.
        private void ShowNews()
        {
            Task<List<Headline>> headlinesTask = SiteNews.GetHeadlines(loadedHeadlinesCount);

            /// Обработка окончания выполнения загрузки списка новостей.
            headlinesTask.ContinueWith((Task<List<Headline>> task) =>
            {
                List<Headline> headlines = task.Result;

                if (headlines.Count > 0)
                {
                    loadedHeadlinesCount += 50;

                    /// Добавление новостных блоков в контейнер.
                    Dispatcher.BeginInvokeOnMainThread(() =>
                    {
                        /// Выключения индикатора загрузки.
                        loadingIndicator.IsVisible = false;

                        foreach (Headline headline in headlines)
                        {
                            HeadlineView headlineView = new HeadlineView(headline);

                            /// Добавление стилей и обработчика события.
                            headlineView.Style = Resources["HeadlineViews"] as Style;
                            headlineView.Tapped += HeadlineView_Tapped;

                            headlinesContainer.Children.Add(headlineView);
                        }
                    });
                    
                }
            });
        }



        // Обработка события проявления окна.
        private void MainPage_Appearing(object sender, EventArgs e)
        {
            /// Изменение цвета statusBar.
            Color moexScarlet = (Color)Application.Current.Resources["MoexScarlet"];
            App.Os.ChangeStatusBarColor(moexScarlet);
        }



        // Обработка нажатия на новостной блок.
        private async void HeadlineView_Tapped(HeadlineView sender, long headlineId)
        {
            /// Переход на страницу новости.
            HeadlineInfoPage newsInfoPage = new HeadlineInfoPage(headlineId);
            await Navigation.PushModalAsync(newsInfoPage);

            /// Изменение цвета statusBar.
            Color wolfGrey = (Color)Application.Current.Resources["WolfGrey"];
            App.Os.ChangeStatusBarColor(wolfGrey);
        }
    }
}
