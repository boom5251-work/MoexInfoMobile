using MoexInfoMobile.StringPatterns;
using System;
using System.Globalization;
using System.Xml;

namespace MoexInfoMobile.Iss.Data
{
    /// <summary>
    /// Информация о событии.
    /// </summary>
    public sealed class EventInfo : Event
    {
        public EventInfo(XmlNode row) : base(row)
        {
            string till = row.Attributes["till"].Value;

            Till = DateTime.ParseExact(till, DateTimePatterns.IssDateTimeFormat, CultureInfo.InvariantCulture);

            Organizer = row.Attributes["organizer"].Value;
            Place = row.Attributes["place"].Value;

            HtmlBody = row.Attributes["body"].Value;
        }



        /// <summary>
        /// Дата и время окончания события.
        /// </summary>
        public DateTime Till { get; }

        /// <summary>
        /// Организатор события.
        /// </summary>
        public string Organizer { get; }

        /// <summary>
        /// Место проведения события.
        /// </summary>
        public string Place { get; }

        /// <summary>
        /// Информация о событии в формате html-кода.
        /// </summary>
        public string HtmlBody { get; }
    }
}