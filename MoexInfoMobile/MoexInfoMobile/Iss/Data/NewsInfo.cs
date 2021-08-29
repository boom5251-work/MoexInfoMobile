using System.Xml;

namespace MoexInfoMobile.Iss.Data
{
    public sealed class NewsInfo : NewsItem
    {
        public NewsInfo(XmlNode row) : base(row)
        {
            try
            {
                Body = row.Attributes["body"].Value;
            }
            catch { }
        }


        public string Body { get; } /// Текст новости.
    }
}
