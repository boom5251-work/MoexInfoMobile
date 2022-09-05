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
        /// <summary>
        /// Инициализирует получение списка новостных заголовков.
        /// </summary>
        /// <param name="start">Начиная с.</param>
        /// <returns>Задача со списком новостных заголовков.</returns>
        public static async Task<List<Headline>> GetHeadlines(uint start)
        {
            List<Headline> headlines = new List<Headline>();

            await Task.Run(() =>
            {
                try
                {
                    string path = $"https://iss.moex.com/iss/sitenews.xml?start={ start }";

                    // Путь http-запроса.
                    var uri = new Uri(path);

                    // Получение xml-документа.
                    var document = ReqRes.GetDocumentByUri(uri);

                    // Получение элемента rows, который содержит элементы новостных заголовков (row).
                    var rows = document.DocumentElement.FirstChild.LastChild as XmlElement;

                    // Перебор всех элементов row. Создание экземпляров новостных заголовков.
                    for (int i = 0; i < rows.ChildNodes.Count; i++)
                    {
                        var row = rows.ChildNodes[i];

                        var headline = new Headline(row);
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


        
        /// <summary>
        /// Инициализирует получение иноформацию о новости.
        /// </summary>
        /// <param name="id">Идентификатор новости.</param>
        /// <returns>Задача с информацией о новости.</returns>
        public static async Task<HeadlineInfo> GetNewsInfo(ulong id)
        {
            HeadlineInfo newsInfo = null;

            await Task.Run(() =>
            {
                try
                {
                    string path = $"https://iss.moex.com/iss/sitenews/{ id }.xml";

                    // Путь http-запроса.
                    var uri = new Uri(path);

                    // Получение xml-документа.
                    var document = ReqRes.GetDocumentByUri(uri);

                    // Получение элемента rows, который содержит элемент-новость (row).
                    var rows = document.DocumentElement.FirstChild.LastChild as XmlElement;

                    newsInfo = new HeadlineInfo(rows.FirstChild);
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