using MoexInfoMobile.Custom;
using MoexInfoMobile.Iss.Api;
using MoexInfoMobile.Iss.Data;
using MoexInfoMobile.Resources.Colors;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoexInfoMobile.Pages
{
    /// <summary>
    /// Главная страница.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Appearing += MainPage_Appearing;

            // Загрузка списка новостей.
            LoadNews();
            // Загрузка списка событий.
            LoadEvents();
            // Загрузка списков всех типов ценных бумаг.
            LoadDefaultSecurities();
        }



        /// <summary>Количество загруженных новостей.</summary>
        private uint LoadedHeadlinesCount { get; set; }

        /// <summary>Количество загруженных событий.</summary>
        private uint LoadedEventsCount { get; set; }

        /// <summary>Количество загруженных акций.</summary>
        public uint LoadedStockBondsCount { get; private set; }

        /// <summary>Количество загруженных облигаций.</summary>
        public uint LoadedStockSharesCount { get; private set; }

        /// <summary>Количество загруженных фьючерсов.</summary>
        public uint LoadedFuturesFortsCount { get; private set; }



        /// <summary>
        /// Инициализирует загрузку списка новостей.
        /// </summary>
        private async void LoadNews()
        {
            try
            {
                Task<List<Headline>> headlinesTask = SiteNews.GetHeadlines(LoadedHeadlinesCount);

                // Обработка окончания выполнения загрузки списка новостей.
                await headlinesTask.ContinueWith((Task<List<Headline>> task) =>
                {
                    List<Headline> headlines = task.Result;

                    if (headlines.Count > 0)
                    {
                        // Увлеичение счетчика на 50.
                        LoadedHeadlinesCount += 50;

                        Dispatcher.BeginInvokeOnMainThread(() =>
                        {
                            // Выключения индикатора загрузки.
                            newsLoadingIndicator.IsVisible = false;
                        });

                        foreach (Headline headline in headlines)
                        {
                            var headlineView = new InfoCellView(headline);

                            // Добавление обработчика события "нажатие".
                            headlineView.Tapped += HeadlineView_Tapped;

                            Dispatcher.BeginInvokeOnMainThread(() =>
                            {
                                // Добавление стилей.
                                headlineView.Style = infoCellViewStyle;
                                headlinesContainer.Children.Add(headlineView);
                            });
                        }
                    }
                });
            }
            catch
            {
                // TODO: Обработать.
            }
        }



        /// <summary>
        /// Инициализирует загрузку списка событий.
        /// </summary>
        private async void LoadEvents()
        {
            try
            {
                Task<List<Event>> eventsTask = Events.GetEvents(LoadedEventsCount);

                // Обработка окончания выполнения загрузки списка событий.
                await eventsTask.ContinueWith((Task<List<Event>> task) =>
                {
                    List<Event> events = task.Result;

                    if (events.Count > 0)
                    {
                        // Увлеичение счетчика на 50.
                        LoadedEventsCount += 50;

                        Dispatcher.BeginInvokeOnMainThread(() =>
                        {
                            // Выключения индикатора загрузки.
                            eventsLoadingIndicator.IsVisible = false;
                        });

                        foreach (Event siteEvent in events)
                        {
                            // Добавление новостных блоков в контейнер
                            var eventView = new InfoCellView(siteEvent);

                            // Добавление обработчика события "нажатие".
                            eventView.Tapped += EventView_Tapped;

                            Dispatcher.BeginInvokeOnMainThread(() =>
                            {
                                // Добавление стилей.
                                eventView.Style = infoCellViewStyle;
                                eventsContainer.Children.Add(eventView);
                            });
                        }
                    }
                });
            }
            catch
            {
                // TODO: Обработать.
            }
        }



        /// <summary>
        /// Инициализирует загрузку всех списков ценных бумаг.
        /// </summary>
        private async void LoadDefaultSecurities()
        {
            try
            {
                // Кнопка "акции" активна по умаолчанию.
                stockBondsToggleButton.BackgroundColor = Colors.WolfGrey;
                stockBondsToggleButton.TextColor = Colors.ClassicChalk;

                // Получение список всех типов ценных бумаг.
                Task<List<Security>>[] securities = new Task<List<Security>>[]
                {
                    Securities.GetSecurities(LoadedStockBondsCount, Securities.StockBondsGroup),
                    Securities.GetSecurities(LoadedStockSharesCount, Securities.StockSharesGroup),
                    Securities.GetSecurities(LoadedFuturesFortsCount, Securities.FuturesFortsGroup)
                };

                // Когда все списки ценных бумаг загружены.
                await Task.Factory.ContinueWhenAll(securities, (Task<List<Security>>[] tasks) =>
                {
                    foreach (Task<List<Security>> task in tasks)
                    {
                        ShowSecurities(task.Result, task.Result[0].SecurityGroup);
                    }
                });
            }
            catch
            {
                // TODO: Обработать.
            }
        }



        /// <summary>
        /// Отображает список ценных бумаг.
        /// </summary>
        /// <param name="securities">Ценные бумаги.</param>
        /// <param name="securityGroup">Тип ценных бумаг.</param>
        private void ShowSecurities(List<Security> securities, string securityGroup)
        {
            StackLayout container;

            // Получение контейнера для каждого типа ценных бумаг.
            switch (securityGroup)
            {
                // Акции.
                case Securities.StockBondsGroup:
                    container = stockBondsContainer;
                    LoadedStockBondsCount += 50;
                    break;
                // Облигации.
                case Securities.StockSharesGroup:
                    container = stockSharesContainer;
                    LoadedStockSharesCount += 50;
                    break;
                // Фьючерсы.
                case Securities.FuturesFortsGroup:
                    container = futuresFortsContainer;
                    LoadedFuturesFortsCount += 50;
                    break;
                default:
                    container = null;
                    break;
            }

            if (container != null)
            {
                foreach (Security security in securities)
                {
                    var securityView = new SecurityCellView(security);
                    securityView.Tapped += SecurityView_Tapped;

                    // Добавление стилей и обработчика события.
                    securityView.Style = securityCellViewStyle;
                    

                    container.Children.Add(securityView);
                }
            }
        }



        /// <summary>
        /// Обрабатывает событие появления окна.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainPage_Appearing(object sender, EventArgs e)
        {
            // Изменение цвета statusBar.
            var moexScarlet = Colors.MoexScarlet;
            App.Os.ChangeStatusBarColor(moexScarlet);
            
        }



        /// <summary>
        /// Обрабатывает нажатие на новостной блок.
        /// </summary>
        /// <param name="sender">Ячейка, вызвавшая событие.</param>
        private async void HeadlineView_Tapped(ICellView sender)
        {
            var view = sender as InfoCellView;

            // Переход на страницу новости.
            var headlineInfoPage = new CellInfoPage(view.ItemId, SiteInfoType.HeadlineInfo);
            await Navigation.PushModalAsync(headlineInfoPage);

            // Изменение цвета statusBar.
            var wolfGrey = Colors.WolfGrey;
            App.Os.ChangeStatusBarColor(wolfGrey);
        }



        /// <summary>
        /// Обрабатывает нажатие на блок события.
        /// </summary>
        /// <param name="sender">Ячейка, вызвавшая событие.</param>
        private async void EventView_Tapped(ICellView sender)
        {
            var view = sender as InfoCellView;

            // Переход на страницу события.
            var eventInfoPage = new CellInfoPage(view.ItemId, SiteInfoType.EventInfo);
            await Navigation.PushModalAsync(eventInfoPage);

            // Изменение цвета statusBar.
            var wolfGrey = Colors.WolfGrey;
            App.Os.ChangeStatusBarColor(wolfGrey);
        }



        private async void SecurityView_Tapped(ICellView sender)
        {
            // TODO: Реализовать.
            throw new NotImplementedException();
        }



        /// <summary>
        /// Обрабатывает нажатие на кнопку выбора типа ценных бумаг.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события</param>
        private void SecurityButton_Clicked(object sender, EventArgs e)
        {
            // Изменение цвета фона и текста всех кнопок в контейнере.
            foreach (Button button in securityButtonsContainer.Children)
            {
                button.BackgroundColor = Colors.LightGrey;
                button.TextColor = Colors.MidnightBadger;
            }

            // Изменение цвета фона и текста нажатой кнопки.
            Button clickedButton = sender as Button;
            clickedButton.BackgroundColor = Colors.WolfGrey;
            clickedButton.TextColor = Colors.ClassicChalk;

            // Скрытие всех контейнеров ценных бумаг.
            foreach (StackLayout container in securitiesContainers.Children)
            {
                container.IsVisible = false;
            }

            // Отображение необходимого контейнера.
            if (clickedButton.Id == stockBondsToggleButton.Id)
            {
                stockBondsContainer.IsVisible = true;
            }
            else if (clickedButton.Id == stockSharesToggleButton.Id)
            {
                stockSharesContainer.IsVisible = true;
            }
            else if (clickedButton.Id == futuresFortsContainer.Id)
            {
                futuresFortsContainer.IsVisible = true;
            }
        }
    }
}