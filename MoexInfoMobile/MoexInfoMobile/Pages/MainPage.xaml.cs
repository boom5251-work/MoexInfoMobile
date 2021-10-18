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
            ShowEvents();
            ShowSecurities();

            Appearing += MainPage_Appearing;
        }



        private uint loadedHeadlinesCount = 0;
        private uint loadedEventsCount = 0;



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
                        newsLoadingIndicator.IsVisible = false;

                        foreach (Headline headline in headlines)
                        {
                            InfoCellView headlineView = new InfoCellView(headline);

                            /// Добавление стилей и обработчика события.
                            headlineView.Style = Resources["InfoCellViews"] as Style;
                            headlineView.Tapped += HeadlineView_Tapped;

                            headlinesContainer.Children.Add(headlineView);
                        }
                    });
                }
            });
        }



        // Метод запускает выполнение задачи загрузки списка событий.
        private void ShowEvents()
        {
            Task<List<Event>> eventsTask = Events.GetEvents(loadedEventsCount);

            /// Обработка окончания выполнения загрузки списка событий.
            eventsTask.ContinueWith((Task<List<Event>> task) =>
            {
                List<Event> events = task.Result;

                if (events.Count > 0)
                {
                    loadedEventsCount += 50;

                    /// Добавление блоков событий в контейнер.
                    Dispatcher.BeginInvokeOnMainThread(() =>
                    {
                        /// Выключения индикатора загрузки.
                        eventsLoadingIndicator.IsVisible = false;

                        foreach (Event siteEvent in events)
                        {
                            InfoCellView eventView = new InfoCellView(siteEvent);

                            /// Добавление стилей и обработчика события.
                            eventView.Style = Resources["InfoCellViews"] as Style;
                            eventView.Tapped += EventView_Tapped;

                            eventsContainer.Children.Add(eventView);
                        }
                    });
                }
            });
        }



        // Метод запускает выполнение задачь загрузки списка ценных бумаг.
        private void ShowSecurities()
        {
            stockBondsToggleButton.BackgroundColor = (Color)Application.Current.Resources["MoexScarlet"];
            stockBondsToggleButton.TextColor = (Color)Application.Current.Resources["ClassicChalk"];
        }



        // Обработка события проявления окна.
        private void MainPage_Appearing(object sender, EventArgs e)
        {
            /// Изменение цвета statusBar.
            Color moexScarlet = (Color)Application.Current.Resources["MoexScarlet"];
            App.Os.ChangeStatusBarColor(moexScarlet);
        }



        // Обработка нажатия на новостной блок.
        private async void HeadlineView_Tapped(InfoCellView sender, ulong headlineId)
        {
            /// Переход на страницу новости.
            CellInfoPage headlineInfoPage = new CellInfoPage(headlineId);
            await Navigation.PushModalAsync(headlineInfoPage);

            /// Изменение цвета statusBar.
            Color wolfGrey = (Color)Application.Current.Resources["WolfGrey"];
            App.Os.ChangeStatusBarColor(wolfGrey);
        }



        // Обработка нажатия на блок события.
        private async void EventView_Tapped(InfoCellView sender, ulong eventId)
        {
            /// Переход на страницу события.
            CellInfoPage eventInfoPage = new CellInfoPage(eventId);
            await Navigation.PushModalAsync(eventInfoPage);

            /// Изменение цвета statusBar.
            Color wolfGrey = (Color)Application.Current.Resources["WolfGrey"];
            App.Os.ChangeStatusBarColor(wolfGrey);
        }



        // Обработка нажатия на кнопку выбора типа ценных бумаг.
        private void SecurityButton_Clicked(object sender, EventArgs e)
        {
            /// Изменение цвета фона и текста всех кнопок в контейнере.
            foreach (Button button in securityButtonsContainer.Children)
            {
                button.BackgroundColor = (Color)Application.Current.Resources["LightGrey"];
                button.TextColor = (Color)Application.Current.Resources["MidnightBadger"];
            }

            /// Изменение цвета фона и текста нажатой кнопки.
            Button clickedButton = sender as Button;
            clickedButton.BackgroundColor = (Color)Application.Current.Resources["MoexScarlet"];
            clickedButton.TextColor = (Color)Application.Current.Resources["ClassicChalk"];
        }
    }
}