using MoexInfoMobile.Iss.Data;
using MoexInfoMobile.Iss.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace MoexInfoMobile.Iss.Api
{
    /// <summary>
    /// Работа с API новостей биржи.
    /// </summary>
    public static class SiteNews
    {
        /// <summary>
        /// Инициирует получение списка новостных заголовков.
        /// </summary>
        /// <param name="start">Начиная с.</param>
        /// <returns>Задача со списком новостных заголовков.</returns>
        public static async Task<List<Headline>> GetHeadlines(uint start)
        {
            List<Headline> headlines = new List<Headline>();

            await Task.Run(() =>
            {
                string path = $"https://iss.moex.com/iss/sitenews.xml?start={start}";

                // Путь HTTP-запроса.
                var uri = new Uri(path);

                // Получение XML-документа.
                var document = HttpRequestManager.GetDocumentByUri(uri);

                // Получение элемента rows, который содержит элементы новостных заголовков (row).
                var rows = (XmlElement?)document?.DocumentElement.FirstChild.LastChild;

                // Перебор всех элементов row. Создание экземпляров новостных заголовков.
                for (int i = 0; i < rows?.ChildNodes.Count; i++)
                {
                    var row = rows.ChildNodes[i];

                    if (Headline.CanCreateInstance(row, out Headline? headline) && headline != null)
                        headlines.Add(headline);
                }
            });

            return headlines;
        }

        
        /// <summary>
        /// Инициирует получение информацию о новости.
        /// </summary>
        /// <param name="id">Идентификатор новости.</param>
        /// <returns>Задача с информацией о новости.</returns>
        public static async Task<HeadlineInfo?> GetHeadlineInfo(ulong id)
        {
            HeadlineInfo? newsInfo = null;

            await Task.Run(() =>
            {
                string path = $"https://iss.moex.com/iss/sitenews/{id}.xml";

                // Путь HTTP-запроса.
                var uri = new Uri(path);

                // Получение XML-документа.
                var document = HttpRequestManager.GetDocumentByUri(uri);

                // Получение элемента rows, который содержит элемент-новость (row).
                var rows = (XmlElement?)document?.DocumentElement.FirstChild.LastChild;

                if (rows != null)
                    HeadlineInfo.CanCreateInstance(rows.FirstChild, out newsInfo);
            });

            return newsInfo;
        }
    }
}