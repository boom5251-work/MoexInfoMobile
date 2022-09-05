using System;
using System.Globalization;
using System.Xml;

namespace MoexInfoMobile.Iss.Data
{
    public class Event
    {
        public Event(XmlNode row)
        {
            string id = row.Attributes["id"].Value;
            Id = ulong.Parse(id);

            Title = row.Attributes["title"].Value;

            string format = "yyyy-MM-dd HH:mm:ss";
            string from = row.Attributes["from"].Value;
            string modifiedAt = row.Attributes["modified_at"].Value;

            From = DateTime.ParseExact(from, format, CultureInfo.InvariantCulture);
            ModifiedAt = DateTime.ParseExact(modifiedAt, format, CultureInfo.InvariantCulture);
        }



        /// <summary>Идентификатор события.</summary>
        public ulong Id { get; private set; }

        /// <summary>Заголовок.</summary>
        public string Title { get; private set; } = string.Empty;

        /// <summary>Дата и время начала события.</summary>
        public DateTime From { get; private set; }

        /// <summary>Дата изменения.</summary>
        public DateTime ModifiedAt { get; private set; }
    }
}