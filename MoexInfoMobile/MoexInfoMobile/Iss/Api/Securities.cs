using MoexInfoMobile.Iss.Data;
using MoexInfoMobile.Iss.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Xml;

namespace MoexInfoMobile.Iss.Api
{
    public static class Securities
    {
        // Метод возвращает задачу с обобщенным типом "список ценных бумаг".
        public static async Task<List<Security>> GetSecurities(uint start, string securityGroup, string q = "")
        {
            List<Security> securities = new List<Security>(); /// Списко ценных бумаг.

            await Task.Run(() =>
            {
                try
                {
                    string path = $"https://iss.moex.com/iss/securities.xml?limit=100&group_by=group&group_by_filter={ securityGroup }&start={ start }&q={ q }";
                    Uri uri = new Uri(path); /// Путь http-запроса.

                    XmlDocument document = ReqRes.GetDocumentByUri(uri); /// Получение xml-документа.

                    /// Получение элемента rows, который содержит элементы ценных бумаг (row).
                    XmlElement rows = document.DocumentElement.FirstChild.LastChild as XmlElement;

                    /// Перебор всех элементов row. Создание экземпляров ценных бумаг.
                    for (int i = 0; i < rows.ChildNodes.Count; i++)
                    {
                        XmlNode row = rows.ChildNodes[i];

                        /// Если данные ценной бумаги можно извлечь, то она добавляется в список.
                        if (Security.CanExtractFromNode(row, out Security security))
                        {
                            securities.Add(security);
                        }
                    }
                }
                catch (UriFormatException)
                {
                    App.Os.ShowToastNotification("Не удалось получить список ценных бумаг.");
                }
                catch (InvalidOperationException)
                {
                    App.Os.ShowToastNotification("Не удалось получить список ценных бумаг.");
                }
            });

            return securities;
        }



        // Метод возвращает задачу с обобщенным типом "информация о ценной бумаге".
        public static async Task<SecurityInfo> GetSecurityInfo(string seqId, string group)
        {
            SecurityInfo securityInfo = null; /// Информация о ценной бумаге.

            await Task.Run(() =>
            {
                try
                {
                    string path = $"https://iss.moex.com/iss/securities/{ seqId }.xml";
                    Uri uri = new Uri(path); /// Путь http-запроса.

                    XmlDocument document = ReqRes.GetDocumentByUri(uri); /// Получение xml-документа.

                    /// Получение элемента rows, который содержит подробную информацию о ценной бумаге.
                    XmlElement rows = document.DocumentElement.FirstChild.LastChild as XmlElement;

                    switch (group)
                    {
                        case "futures_forts":
                            securityInfo = new FuturesFortInfo(rows);
                            break;
                        case "stock_bonds":
                            securityInfo = new StockBondInfo(rows);
                            break;
                        case "stock_shares":
                            securityInfo = new StockShareInfo(rows);
                            break;
                    }
                }
                catch (UriFormatException)
                {
                    App.Os.ShowToastNotification("Не удалось загрузить подробную информацию.");
                }
                catch (InvalidOperationException)
                {
                    App.Os.ShowToastNotification("Не удалось загрузить подробную информацию.");
                }
            });

            return securityInfo;
        }



        // Метод возвращает задачу с обобщенным типом "кортеж: дата начала торгов, дата окончания торгов".
        public static async Task<Tuple<DateTime, DateTime>> GetSecurityAggregatesDates(string secId)
        {
            Tuple<DateTime, DateTime> dates = null; /// Кортеж: дата начала торгов, дата окончания торгов.

            await Task.Run(() =>
            {
                try
                {
                    string path = $"https://iss.moex.com/iss/securities/{ secId }/aggregates.xml";
                    Uri uri = new Uri(path); /// Путь http-запроса.

                    XmlDocument document = ReqRes.GetDocumentByUri(uri); /// Получение xml-документа.

                    /// Получение элемента rows, который содержит даты начала и окончания торгов.
                    XmlElement rows = document.DocumentElement.LastChild.LastChild as XmlElement;

                    /// Если строка row есть, то даты извлекаются.
                    if (rows.HasChildNodes)
                    {
                        XmlNode row = rows.FirstChild; /// Строка row.

                        string fromStr = row.Attributes["from"].Value; /// Начало торгов.
                        string tillStr = row.Attributes["till"].Value; /// Окончание торгов.

                        string format = "yyyy-mm-dd";

                        /// Получение даты начала и окончания торгов.
                        DateTime from = DateTime.ParseExact(fromStr, format, CultureInfo.InvariantCulture);
                        DateTime till = DateTime.ParseExact(tillStr, format, CultureInfo.InvariantCulture);

                        dates = new Tuple<DateTime, DateTime>(from, till);
                    }
                }
                catch { }
            });

            return dates;
        }
    }
}
