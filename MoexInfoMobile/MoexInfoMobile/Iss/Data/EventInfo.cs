using System;
using System.Globalization;
using System.Xml;

namespace MoexInfoMobile.Iss.Data
{
    public sealed class EventInfo : Event
    {
        public EventInfo(XmlNode row) : base(row)
        {
            string format = "yyyy-MM-dd hh:mm:ss";
            string till = row.Attributes["till"].Value;

            Till = DateTime.ParseExact(till, format, CultureInfo.InvariantCulture);

            Orginizer = row.Attributes["orginizer"].Value;
            Place = row.Attributes["place"].Value;

            HtmlBody = row.Attributes["body"].Value;
        }


        /// <summary>Дата и время окончания события.</summary>
        public DateTime Till { get; private set; }

        /// <summary>Организатор события.</summary>
        public string Orginizer { get; private set; } = string.Empty;

        /// <summary>Место проведения события.</summary>
        public string Place { get; private set; } = string.Empty;

        /// <summary>Информация о событии в формате html-кода.</summary>
        public string HtmlBody { get; private set; } = string.Empty;
    }
}