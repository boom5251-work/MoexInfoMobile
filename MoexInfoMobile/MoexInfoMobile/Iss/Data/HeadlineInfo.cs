using System.Xml;

namespace MoexInfoMobile.Iss.Data
{
    public sealed class HeadlineInfo : Headline
    {
        public HeadlineInfo(XmlNode row) : base(row)
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
