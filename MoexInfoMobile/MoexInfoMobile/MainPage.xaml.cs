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
            LoadDefaultSecurities();
        }



        // Метод выполняет задачи первичной загрузки списка ценных бумаг.
        private void LoadDefaultSecurities()
        {
            /// Массив групп ценных бумаг.
            string[] filters = new string[]
            {
                "futures_forts",
                "stock_bonds",
                "stock_shares"
            };
            
            Task<List<Security>>[] securitiesTasks = new Task<List<Security>>[filters.Length]; /// Массив задач.

            for (int i = 0; i < filters.Length; i++)
            {
                securitiesTasks[i] = Securities.GetSecurities(0, filters[i]);
            }

            /// Создание планирования задач.
            Task.Factory.ContinueWhenAll(securitiesTasks, (Task<List<Security>>[] tasks) =>
            {
                for (int i = 0; i < tasks.Length; i++)
                {
                    List<Security> securities = tasks[i].Result;

                    /// Отображение ценных бумаг в соответсвующих вкладках.
                    for (int j = 0; j < securities.Count; j++)
                    {
                        Security stockShare = tasks[i].Result[j];

                        /// Вызов оснвного потока.
                        Dispatcher.BeginInvokeOnMainThread(() =>
                        {
                            /// Добавление дочернего элемента в список.
                            // TODO: Добавить логику отображения ценных бумаг.
                        });
                    }
                }
            });
        }
    }
}
