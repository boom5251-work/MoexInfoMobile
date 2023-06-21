using MoexInfoMobile.Iss.Data;
using MoexInfoMobile.Iss.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace MoexInfoMobile.Iss.Api
{
    /// <summary>
    /// Работа с API движков.
    /// </summary>
    public static class Engines
    {
        /// <summary>
        /// Сообщение об ошибке получения данных.
        /// </summary>
        private static readonly string DataErrorMessage = "Не удалось получить данные ценной бумаги";



        /// <summary>
        /// Инициализирует получение свечей.
        /// </summary>
        /// <param name="args">Аргументы запроса.</param>
        /// <param name="from">Начиная с.</param>
        /// <param name="till">Заканчивая до.</param>
        /// <param name="interval">Интервал.</param>
        /// <returns>Задача со списком свечей.</returns>
        public static async Task<List<Candle>> GetCandles(Tuple<string, string, string> args, string from, string till, byte interval)
        {
            var candles = new List<Candle>();

            await Task.Run(() =>
            {
                try
                {
                    // Движок.
                    string engine = args.Item1;
                    // Рынок.
                    string market = args.Item2;
                    // Идентификатор.
                    string secId = args.Item3;

                    string path = "https://iss.moex.com/iss/engines/" + 
                        $"{engine}/markets/{market}/securities/{secId}" +
                        $"/candles.xml?from={from}&till={till}&interval={interval}";

                    // Путь http-запроса.
                    var uri = new Uri(path);

                    // Получение xml-документа.
                    var document = HttpRequestManager.GetDocumentByUri(uri);

                    // Получение элемента rows, который содержит список свечей (row).
                    var rows = document.DocumentElement.FirstChild.LastChild as XmlElement;

                    // Перебор всех элементов row. Создание экземпляров свечей.
                    for (int i = 0; i < rows?.ChildNodes.Count; i++)
                    {
                        var row = rows.ChildNodes[i];

                        // Если данные свечи удалось извлечь, то она добавляется в список.
                        if (Candle.CanExtractFromNode(row, out Candle? candle) && candle != null)
                            candles.Add(candle);
                    }
                }
                catch (UriFormatException)
                {
                    App.Os.ShowToastNotification(DataErrorMessage);
                }
                catch (InvalidOperationException)
                {
                    App.Os.ShowToastNotification(DataErrorMessage);
                }
            });

            return candles;
        }
    }
}