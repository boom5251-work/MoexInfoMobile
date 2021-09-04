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
            string from = row.Attributes["from"].Value;
            string till = row.Attributes["till"].Value;

            Till = DateTime.ParseExact(till, format, CultureInfo.InvariantCulture);

            Orginizer = row.Attributes["orginizer"].Value;
            Place = row.Attributes["place"].Value;

            HtmlBody = row.Attributes["body"].Value;
        }



        public DateTime Till { get; private set; } /// Дата и время окончания события.
        public string Orginizer { get; private set; } /// Организатор события.
        public string Place { get; private set; } /// Место проведения события.

        public string HtmlBody { get; private set; } /// Информация о событии в формате html-кода.
    }
}
