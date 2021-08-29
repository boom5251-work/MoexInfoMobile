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

            // TODO: Удалить проверку toast-уведомлений.
            App.Os.ShowToastNotification("Ку! Это boom5251!");
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
                /// Получение ценных бумаг, дополненных изменением цены за день.
                List<Security> securities = getSecurities.Result;

                /// Отображение списка ценных бумаг.
                foreach (Security security in securities)
                {
                    Dispatcher.BeginInvokeOnMainThread(() =>
                    {
                        // TODO: Добавить отображение всех типов задач.
                    });
                }
            });
        }
    }
}
