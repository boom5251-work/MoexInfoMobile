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



        public ulong Id { get; private set; } /// Идентификатор события.
        public string Title { get; private set; } /// Заголовок.
        public DateTime From { get; private set; } /// Дата и время начала события.
        public DateTime ModifiedAt { get; private set; } /// Дата изменения.
    }
}
