using MoexInfoMobile.Iss.Data;
using MoexInfoMobile.Iss.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace MoexInfoMobile.Iss.Api
{
    public static class Securities
    {
        // Метод возвращает задачу с обобщенным типом "список ценных бумаг".
        public async static Task<List<Security>> GetSecurities(int start, string securityGroup, string q = "")
        {
            List<Security> securities = new List<Security>(); /// Списко ценных бумаг.

            await Task.Run(() =>
            {
                try
                {
                    string path = $"https://iss.moex.com/iss/securities.xml?group_by=group&group_by_filter={ securityGroup }&start={ start }&q={ q }";
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
                    // TODO: Выводить уведомление об ошибке.
                }
                catch (InvalidOperationException)
                {
                    // TODO: Выводить уведомление об ошибке.
                }
            });

            return securities;
        }



        // Метод возвращает задачу с обобщенным типом "информация о ценной бумаге".
        public static async Task<SecurityInfo> GetSecurityInfo(string seqId)
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

                    if (SecurityInfo.CanExtractFromNode(rows, out securityInfo)) { }
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

            return securityInfo;
        }
    }
}
