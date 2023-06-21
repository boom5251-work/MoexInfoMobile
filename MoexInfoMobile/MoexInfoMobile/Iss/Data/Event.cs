using MoexInfoMobile.StringPatterns;
using System;
using System.Globalization;
using System.Xml;

namespace MoexInfoMobile.Iss.Data
{
    /// <summary>
    /// Событие биржи.
    /// </summary>
    public class Event
    {
        public Event(XmlNode row)
        {
            string id = row.Attributes["id"].Value;
            Id = ulong.Parse(id);

            Title = row.Attributes["title"].Value;

            string from = row.Attributes["from"].Value;
            //string modifiedAt = row.Attributes["modified_at"].Value;

            From = DateTime.ParseExact(from, DateTimePatterns.IssDateTimeFormat, CultureInfo.InvariantCulture);
            //ModifiedAt = DateTime.ParseExact(modifiedAt, DateTimePatterns.IssDateTimeFormat, CultureInfo.InvariantCulture);
        }



        /// <summary>
        /// Идентификатор события.
        /// </summary>
        public ulong Id { get; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Дата и время начала события.
        /// </summary>
        public DateTime From { get; }

        /// <summary>
        /// Дата изменения.
        /// </summary>
        public DateTime ModifiedAt { get; }
    }
}