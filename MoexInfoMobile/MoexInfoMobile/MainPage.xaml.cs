using MoexInfoMobile.Iss.Api;
using MoexInfoMobile.Iss.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoexInfoMobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            /// По умолчанию позиции отсчета списка равны нулю.
            FuturesFortsStart = 0;
            StockBondsStart = 0;
            StockSharesStart = 0;

            ShowAllDefaultSecurities();
        }



        /// Начало отсчета свписка для каждого типа ценных бумаг.
        public uint FuturesFortsStart { get; private set; }
        public uint StockBondsStart { get; private set; }
        public uint StockSharesStart { get; private set; }


        /// Фильтры ценных (группы) бумаг.
        private const string futuresFortsFilter = "futures_forts";
        private const string stockBondsFilter = "stock_bonds";
        private const string stockSharesFilter = "stock_shares";



        // Метод запускает выполнение задач загрузки списка бумаг и изменений их стоимости за день (по умолчанию).
        private void ShowAllDefaultSecurities()
        {
            /// Массив вариантов для всех типов ценных бумаг.
            var options = new Tuple<uint, string>[]
            {
                new Tuple<uint, string>(FuturesFortsStart, futuresFortsFilter),
                new Tuple<uint, string>(StockBondsStart, stockBondsFilter),
                new Tuple<uint, string>(StockSharesStart, stockSharesFilter)
            };

            /// Перебор вариантов.
            foreach (Tuple<uint, string> option in options)
            {
                ShowSecurities(option.Item1, option.Item2);
            }
        }



        // Метод выполняет получение списка бумаг и изменения их цены за день.
        private void ShowSecurities(uint start, string filter)
        {
            /// Задача, загружающая список ценных бумаг.
            Securities.GetSecurities(start, filter).ContinueWith((Task<List<Security>> getSecurities) => 
            {
                /// Последний день торгов (по умолчанию текущий день).
                DateTime lastTradingDay = DateTime.Today;

                /// Если текущий день суббота или воскресенье, то на последний день торгов усатнавливается последняя пятница.
                switch (lastTradingDay.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                        lastTradingDay = lastTradingDay.AddDays(-1);
                        break;
                    case DayOfWeek.Sunday:
                        lastTradingDay = lastTradingDay.AddDays(-2);
                        break;
                }

                string from = lastTradingDay.ToString("yyyy-mm-dd");

                /// Получение ценных бумаг, дополненных изменением цены за день.
                List<Security> securities = getSecurities.Result;
                Task<Security>[] getUpdatedSecurities = new Task<Security>[securities.Count];

                for (int i = 0; i < securities.Count; i++)
                {
                    Security security = securities[i];
                    getUpdatedSecurities[i] = LoadCandlesForSecurity(security, from);
                }

                /// Отображение списка ценных бумаг.
                Task.Factory.ContinueWhenAll(getUpdatedSecurities, (Task<Security>[] tasks) =>
                {
                    foreach (Task<Security> task in tasks)
                    {
                        // TODO: Добавить отображение всех типов задач.
                    }
                });
            });
        }



        // Метод дополняет ценную бумагу значением изменения цены за день.
        private async Task<Security> LoadCandlesForSecurity(Security security, string from)
        {
            await Task.Run(() =>
            {
                /// Установк значений, в зависимости от типа ценной бумаги.
                string engine = string.Empty;
                string market = string.Empty;

                switch (security.SecurityGroup)
                {
                    case futuresFortsFilter:
                        engine = "futures";
                        market = "forts";
                        break;
                    case stockBondsFilter:
                        engine = "stock";
                        market = "bonds";
                        break;
                    case stockSharesFilter:
                        engine = "stock";
                        market = "shares";
                        break;
                }

                /// Части http-запроса.
                var args = new Tuple<string, string, string>(engine, market, security.SecId);

                /// Получение свечей.
                Task<List<Candle>> getCandles = Engines.GetCandles(args, from, string.Empty, 24);
                getCandles.Wait();

                List<Candle> candles = getCandles.Result; /// Свечи.

                /// Расчет изменения цены за день.
                if (candles.Count > 0)
                {
                    Candle first = candles[0]; /// Первая свеча.
                    Candle last = candles[candles.Count - 1]; /// Последняя свеча.

                    double difference = last.Close - first.Open;
                    double percent = Math.Round(difference / first.Open, 2);

                    security.PercentPriceChange = percent;
                }
            });

            return security;
        }
    }
}
