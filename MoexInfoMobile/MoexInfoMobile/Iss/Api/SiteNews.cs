using MoexInfoMobile.Iss.Data;
using MoexInfoMobile.Iss.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace MoexInfoMobile.Iss.Api
{
    public static class SiteNews
    {
        // Метод возвращает задачу с обобщенным типом "список новостных заголовков".
        public static async Task<List<NewsItem>> GetHeadlines(uint start)
        {
            List<NewsItem> headlines = new List<NewsItem>();

            await Task.Run(() =>
            {
                try
                {
                    string path = $"https://iss.moex.com/iss/sitenews.xml?start={ start }";
                    Uri uri = new Uri(path); /// Путь http-запроса.

                    XmlDocument document = ReqRes.GetDocumentByUri(uri); /// Получение xml-документа.

                    /// Получение элемента rows, который содержит элементы новостных заголовков (row).
                    XmlElement rows = document.DocumentElement.FirstChild.LastChild as XmlElement;

                    /// Перебор всех элементов row. Создание экземпляров новостных заголовков.
                    for (int i = 0; i < rows.ChildNodes.Count; i++)
                    {
                        XmlNode row = rows.ChildNodes[i];

                        NewsItem headline = new NewsItem(row);
                        headlines.Add(headline);
                    }
                }
                catch (UriFormatException)
                {
                    App.Os.ShowToastNotification("Не удалось получить список новостей.");
                }
                catch (InvalidOperationException)
                {
                    App.Os.ShowToastNotification("Не удалось получить список новостей.");
                }
            });

            return headlines;
        }


        
        // Метод возвращает задачу с обобщенным типом "новость сайта".
        public static async Task<NewsInfo> GetNewsInfo(ulong id)
        {
            NewsInfo newsInfo = null;

            await Task.Run(() =>
            {
                try
                {
                    string path = $"https://iss.moex.com/iss/sitenews/{ id }.xml";
                    Uri uri = new Uri(path); /// Путь http-запроса.

                    XmlDocument document = ReqRes.GetDocumentByUri(uri); /// Получение xml-документа.

                    /// Получение элемента rows, который содержит элемент-новость (row).
                    XmlElement rows = document.DocumentElement.FirstChild.LastChild as XmlElement;

                    newsInfo = new NewsInfo(rows.FirstChild);
                }
                catch (UriFormatException)
                {
                    App.Os.ShowToastNotification("Не удалось загрузить новость.");
                }
                catch (InvalidOperationException)
                {
                    App.Os.ShowToastNotification("Не удалось загрузать новость.");
                }
            });

            return newsInfo;
        }
    }
}
