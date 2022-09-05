using MoexInfoMobile.Iss.Data;
using MoexInfoMobile.Iss.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace MoexInfoMobile.Iss.Api
{
    /// <summary>
    /// Движки ИСС.
    /// </summary>
    public static class Engines
    {
        /// <summary>
        /// Инициализирует получение свечей.
        /// </summary>
        /// <param name="args">Аргументы запроса.</param>
        /// <param name="from">Начиная с.</param>
        /// <param name="till">Заканяивая до.</param>
        /// <param name="interval">Интервал.</param>
        /// <returns>Задача со списком свечей.</returns>
        public static async Task<List<Candle>> GetCandles(Tuple<string, string, string> args, string from, string till, byte interval)
        {
            List<Candle> candles = new List<Candle>();

            await Task.Run(() =>
            {
                try
                {
                    string engine = args.Item1;
                    string market = args.Item2;
                    string secId = args.Item3;

                    string path = $"https://iss.moex.com/iss/engines/" + 
                        $"{engine}/markets/{market}/securities/{secId}" +
                        $"/candles.xml?from={from}&till={till}&interval={interval}";

                    var uri = new Uri(path); // Путь http-запроса.

                    var document = ReqRes.GetDocumentByUri(uri); // Получение xml-документа.

                    // Получение элемента rows, который содержит список свечей (row).
                    var rows = document.DocumentElement.FirstChild.LastChild as XmlElement;

                    // Перебор всех элементов row. Создание экземпляров свечей.
                    for (int i = 0; i < rows.ChildNodes.Count; i++)
                    {
                        var row = rows.ChildNodes[i];

                        // Если данные свечи, то она добавляется в список.
                        if (Candle.CanExtractFromNode(row, out Candle candle))
                        {
                            candles.Add(candle);
                        }
                    }
                }
                catch (UriFormatException ex)
                {
                    App.Os.ShowToastNotification(ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    App.Os.ShowToastNotification(ex.Message);
                }
            });

            return candles;
        }
    }
}