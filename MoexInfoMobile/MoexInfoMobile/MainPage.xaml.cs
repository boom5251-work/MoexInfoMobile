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

            LoadDefaultSecurities();
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
        private void LoadDefaultSecurities()
        {
            Task<List<SecurityWithPriceChanges>>[] getSecuritiesWithPriceChanges = new Task<List<SecurityWithPriceChanges>>[]
            {
                LoadSecuritiesWitPriceChanges(FuturesFortsStart, futuresFortsFilter),
                LoadSecuritiesWitPriceChanges(StockBondsStart, stockBondsFilter),
                LoadSecuritiesWitPriceChanges(StockSharesStart, stockSharesFilter)
            };

            // TODO: Добавить отображение списка на экране.
            Task.Factory.ContinueWhenAll(getSecuritiesWithPriceChanges, (Task<List<SecurityWithPriceChanges>>[] tasks) => { });
        }



        // Метод выполняет получение списка бумаг и изменения их цены за день.
        private Task<List<SecurityWithPriceChanges>> LoadSecuritiesWitPriceChanges(uint start, string filter)
        {
            /// Задача, загружающая список ценных бумаг.
            Task<List<Security>> getSecurities = Securities.GetSecurities(start, filter);

            /// Задача, загружающая изменение цен за день для бумаг, полученных предыдущей.
            Task<List<SecurityWithPriceChanges>> getPriceChanges = getSecurities.ContinueWith((Task<List<Security>> task) =>
            {
                return new List<SecurityWithPriceChanges>();
                // TODO: Добавить получение изменения цены за день.
            });

            return getPriceChanges;
        }
    }
}
