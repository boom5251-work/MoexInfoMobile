using System.Xml;

namespace MoexInfoMobile.Iss.Data
{
    public sealed class HeadlineInfo : Headline
    {
        public HeadlineInfo(XmlNode row) : base(row)
        {
            try
            {
                HtmlBody = row.Attributes["body"].Value;
            }
            catch { }
        }



        /// <summary>Текст новости.</summary>
        public string HtmlBody { get; } = string.Empty;
    }
}