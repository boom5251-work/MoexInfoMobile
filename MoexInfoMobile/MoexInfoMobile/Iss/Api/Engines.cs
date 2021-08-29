using MoexInfoMobile.Iss.Data;
using MoexInfoMobile.Iss.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace MoexInfoMobile.Iss.Api
{
    public static class Engines
    {
        // Метод озвращает задачу с обобщенным типом "список свечей".
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

                    string path = $"https://iss.moex.com/iss/engines/{ engine }/markets/{ market }/securities/{ secId }/candles.xml?from={ from }&till={ till }&interval={ interval }";
                    Uri uri = new Uri(path); /// Путь http-запроса.

                    XmlDocument document = ReqRes.GetDocumentByUri(uri); /// Получение xml-документа.

                    /// Получение элемента rows, который содержит список свечей (row).
                    XmlElement rows = document.DocumentElement.FirstChild.LastChild as XmlElement;

                    /// Перебор всех элементов row. Создание экземпляров свечей.
                    for (int i = 0; i < rows.ChildNodes.Count; i++)
                    {
                        XmlNode row = rows.ChildNodes[i];

                        /// Если данные свечи, то она добавляется в список.
                        if (Candle.CanExtractFromNode(row, out Candle candle))
                        {
                            candles.Add(candle);
                        }
                    }
                }
                catch (UriFormatException)
                {
                    // TODO: Выводить уведомление об ошибке.
                }
                catch (InvalidOperationException)
                {
                    // TODO: Выводить уведомление об ошибке.
                }
            });

            return candles;
        }
    }
}
