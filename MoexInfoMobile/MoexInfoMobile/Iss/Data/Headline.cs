using MoexInfoMobile.StringPatterns;
using System;
using System.Globalization;
using System.Xml;

namespace MoexInfoMobile.Iss.Data
{
    /// <summary>
    /// Новостной заголовок.
    /// </summary>
    public class Headline
    {
        /// <summary>
        /// Конструктор копирования экземпляра.
        /// </summary>
        /// <param name="headline">Новостной заголовок.</param>
        protected Headline(Headline headline)
        {
            Id = headline.Id;
            Title = headline.Title;
            PublishedAt = headline.PublishedAt;
        }


        /// <summary>
        /// Защищенный конструктор с параметрами.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="publishedAt">Дата и время публикации.</param>
        private Headline(ulong id, string title, DateTime publishedAt)
        {
            Id = id;
            Title = title;
            PublishedAt = publishedAt;
        }



        /// <summary>
        /// Идентификатор записи.
        /// </summary>
        public ulong Id { get; protected set; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        public string Title { get; protected set; }

        /// <summary>
        /// Дата публикации.
        /// </summary>
        public DateTime PublishedAt { get; protected set; }




        /// <summary>
        /// Проверяет, можно ли создать экземпляр класса на основе данных строки XML.
        /// </summary>
        /// <param name="row">Строка XML.</param>
        /// <param name="headline">Экземпляр класса.</param>
        /// <returns>True, если удалось создать экземпляр. False - нет.</returns>
        public static bool CanCreateInstance(XmlNode row, out Headline? headline)
        {
            try
            {
                // Получение идентификатора.
                string idStr = row.Attributes["id"].Value;
                ulong id = ulong.Parse(idStr);

                // Получение заголовка.
                string title = row.Attributes["title"].Value;

                // Получение времени публикации.
                string publishedAtStr = row.Attributes["published_at"].Value;
                var publishedAt = DateTime.ParseExact(publishedAtStr, DateTimePatterns.IssDateTimeFormat, CultureInfo.InvariantCulture);

                headline = new Headline(id, title, publishedAt);
                return true;
            }
            catch
            {
                headline = null;
                return false;
            }
        }
    }
}