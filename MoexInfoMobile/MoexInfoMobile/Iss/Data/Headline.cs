using System;
using System.Globalization;
using System.Xml;

namespace MoexInfoMobile.Iss.Data
{
    public class Headline
    {
        public Headline(XmlNode row)
        {
            try
            {
                /// Извелчение индентификатора и заголовка.
                string idStr = row.Attributes["id"].Value;
                Id = ulong.Parse(idStr);

                Title = row.Attributes["title"].Value;

                /// Извлечение дат публикации и редактирования.
                string publishedAtStr = row.Attributes["published_at"].Value;
                string format = "yyyy-MM-dd HH:mm:ss";

                PublishedAt = DateTime.ParseExact(publishedAtStr, format, CultureInfo.InvariantCulture);
            }
            catch { }
        }



        public ulong Id { get; } /// Идентификатор записи.
        public string Title { get; } /// Заголовок.
        public DateTime PublishedAt { get; } /// Дата публикации.
    }
}
