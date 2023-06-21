using MoexInfoMobile.Iss.Data;
using MoexInfoMobile.Iss.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Xml;

namespace MoexInfoMobile.Iss.Api
{
    /// <summary>
    /// Работа с API Ценных бумаг.
    /// </summary>
    public static class Securities
    {
        /// <summary>
        /// Акции.
        /// </summary>
        public const string StockBondsGroup = "stock_bonds";

        /// <summary>
        /// Облигации.
        /// </summary>
        public const string StockSharesGroup = "stock_shares";

        /// <summary>
        /// Фьючерсы.
        /// </summary>
        public const string FuturesFortsGroup = "futures_forts";


        /// <summary>
        /// Сообщение об ошибке получения списка ценных бумаг.
        /// </summary>
        private static readonly string SecuritiesErrorMessage = "Не удалось получить список ценных бумаг.";

        /// <summary>
        /// Сообщение об ошибке получения информации о ценной бумаге.
        /// </summary>
        private static readonly string SecurityInfoErrorMessage = "Не удалось загрузить подробную информацию.";
        


        /// <summary>
        /// Инициирует получение списка ценных бумаг.
        /// </summary>
        /// <param name="start">Начиная с.</param>
        /// <param name="securityGroup">Тип ценных бумаг.</param>
        /// <param name="q">Соответствует значению.</param>
        /// <returns>Задача со списком ценных бумаг.</returns>
        public static async Task<List<Security>> GetSecurities(uint start, string securityGroup, string q = "")
        {
            // Список ценных бумаг.
            var securities = new List<Security>(); 

            await Task.Run(() =>
            {
                try
                {
                    string path = "https://iss.moex.com/iss/securities.xml?limit=20&group_by=group" + 
                        $"&group_by_filter={securityGroup}&start={start}&q={q}";

                    // Путь http-запроса.
                    var uri = new Uri(path);

                    // Получение xml-документа.
                    var document = HttpRequestManager.GetDocumentByUri(uri); 

                    // Получение элемента rows, который содержит элементы ценных бумаг (row).
                    var rows = document?.DocumentElement.FirstChild.LastChild as XmlElement;

                    // Перебор всех элементов row. Создание экземпляров ценных бумаг.
                    for (int i = 0; i < rows?.ChildNodes.Count; i++)
                    {
                        var row = rows.ChildNodes[i];

                        // Если данные ценной бумаги можно извлечь, то она добавляется в список.
                        if (Security.CanCreateInstance(row, out Security? security) && security != null)
                            securities.Add(security);
                    }
                }
                catch (UriFormatException)
                {
                    App.Os.ShowToastNotification(SecuritiesErrorMessage);
                }
                catch (InvalidOperationException)
                {
                    App.Os.ShowToastNotification(SecuritiesErrorMessage);
                }
            });

            return securities;
        }



        /// <summary>
        /// Инициирует получение информации о ценной бумаге.</summary>
        /// <param name="seqId">Идентификатор ценной бумаги.</param>
        /// <param name="group">Группа ценной бумаги.</param>
        /// <returns>Задача с информацией о ценной бумаге.</returns>
        public static async Task<SecurityInfo?> GetSecurityInfo(string seqId, string group)
        {
            // Информация о ценной бумаге.
            SecurityInfo? securityInfo = null; 

            await Task.Run(() =>
            {
                try
                {
                    string path = $"https://iss.moex.com/iss/securities/{seqId}.xml";

                    // Путь http-запроса.
                    var uri = new Uri(path);

                    // Получение xml-документа.
                    var document = HttpRequestManager.GetDocumentByUri(uri);

                    // Получение элемента rows, который содержит подробную информацию о ценной бумаге.
                    var rows = document?.DocumentElement.FirstChild.LastChild as XmlElement;

                    if (rows != null)
                    {
                        switch (group)
                        {
                            case FuturesFortsGroup:
                                securityInfo = new FuturesFortInfo(rows);
                                break;
                            case StockBondsGroup:
                                securityInfo = new StockBondInfo(rows);
                                break;
                            case StockSharesGroup:
                                securityInfo = new StockShareInfo(rows);
                                break;
                        }
                    }
                }
                catch (UriFormatException)
                {
                    App.Os.ShowToastNotification(SecurityInfoErrorMessage);
                }
                catch (InvalidOperationException)
                {
                    App.Os.ShowToastNotification(SecurityInfoErrorMessage);
                }
            });

            return securityInfo;
        }



        /// <summary>
        /// Инициирует получение дат начала и окончания торгов.
        /// </summary>
        /// <param name="secId">Идентификатор ценной бумаги.</param>
        /// <returns>Задача с датой начала и окончания торгов.</returns>
        public static async Task<Tuple<DateTime, DateTime>?> GetSecurityAggregatesDates(string secId)
        {
            // Кортеж: дата начала торгов, дата окончания торгов.
            Tuple<DateTime, DateTime>? dates = null;

            await Task.Run(() =>
            {
                try
                {
                    string path = $"https://iss.moex.com/iss/securities/{secId}/aggregates.xml";

                    // Путь http-запроса.
                    var uri = new Uri(path);

                    // Получение xml-документа.
                    var document = HttpRequestManager.GetDocumentByUri(uri);

                    // Получение элемента rows, который содержит даты начала и окончания торгов.
                    var rows = document?.DocumentElement.LastChild.LastChild as XmlElement;

                    // Если строка row есть, то даты извлекаются.
                    if (rows != null && rows.HasChildNodes)
                    {
                        // Строка row.
                        var row = rows.FirstChild;

                        // Начало торгов.
                        string fromStr = row.Attributes["from"].Value;

                        // Окончание торгов.
                        string tillStr = row.Attributes["till"].Value;

                        string format = "yyyy-mm-dd";

                        // Получение даты начала и окончания торгов.
                        var from = DateTime.ParseExact(fromStr, format, CultureInfo.InvariantCulture);
                        var till = DateTime.ParseExact(tillStr, format, CultureInfo.InvariantCulture);

                        dates = new Tuple<DateTime, DateTime>(from, till);
                    }
                }
                catch
                {
                    // TODO: Реализовать.
                }
            });

            return dates;
        }
    }
}