using System;
using System.Xml;

namespace MoexInfoMobile.Iss.Data
{
    /// <summary>
    /// Новостная запись.
    /// </summary>
    public sealed class HeadlineInfo : Headline
    {
        /// <summary>
        /// Закрытый конструктор с параметрами.
        /// </summary>
        /// <param name="headline">Новостной заголовок.</param>
        /// <param name="htmlBody">HTML-тело.</param>
        private HeadlineInfo(Headline headline, string htmlBody) : base(headline)
        {
            HtmlBody = htmlBody;
        }



        /// <summary>
        /// Текст новости.
        /// </summary>
        public string HtmlBody { get; }



        /// <summary>
        /// Проверяет, можно ли создать экземпляр класса на основе данных строки XML.
        /// </summary>
        /// <param name="row">Строка XML.</param>
        /// <param name="headlineInfo">Новостная запись.</param>
        /// <returns>True, если удалось создать экземпляр. False - нет.</returns>
        public static bool CanCreateInstance(XmlNode row, out HeadlineInfo? headlineInfo)
        {
            try
            {
                if (CanCreateInstance(row, out Headline? headline) && headline != null)
                {
                    string htmlBody = row.Attributes["body"].Value;

                    headlineInfo = new HeadlineInfo(headline, htmlBody);
                    return true;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            catch
            {
                headlineInfo = null;
                return false;
            }
        }
    }
}