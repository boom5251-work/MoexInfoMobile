using System;
using System.Globalization;
using System.Xml;

namespace MoexInfoMobile.Iss.Data
{
    public class NewsItem
    {
        public NewsItem(XmlNode row)
        {
            try
            {
                /// Извелчение индентификатора и заголовка.
                string idStr = row.Attributes["id"].Value;
                Id = long.Parse(idStr);

                Title = row.Attributes["title"].Value;

                /// Извлечение дат публикации и редактирования.
                string publishedAtStr = row.Attributes["published_at"].Value;
                string format = "yyyy-mm-dd";

                PublishedAt = DateTime.ParseExact(publishedAtStr, format, CultureInfo.InvariantCulture);
            }
            catch { }
        }



        public long Id { get; } /// Идентификатор записи.
        public string Title { get; } /// Заголовок.
        public DateTime PublishedAt { get; } /// Дата публикации.
    }
}
