using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace MoexInfoMobile.Html
{
    public sealed class HtmlDecoder
    {
        // Словарь ресурсов со стилями элементов.
        public ResourceDictionary Resources { get; set; }

        // Цвета для элементов таблиц.
        public Color CellsBackgroundColor { get; set; }
        public Color EmptyCellsBackgroundColor { get; set; }



        // Метод возвращает список элементов, извлеченных из html-строки.
        public bool EncodeHtml(string html, out LinkedList<View> elements)
        {
            Regex tagRegex = new Regex("<.+?>", RegexOptions.IgnoreCase);
            elements = new LinkedList<View>();

            html = ReplaceHtmlEncodedSymbols(html);
            html = html.Replace("<br>", "\n");
            html = html.Replace("\t", "");

            /// Если в строке есть html-разметка, то данные извлекаются.
            if (tagRegex.IsMatch(html))
            {
                /// Перебор совпадений.
                MatchCollection tagMatches = tagRegex.Matches(html);

                for (int i = 0; i < tagMatches.Count; i++)
                {

                }
                return true;
            }
            else
            {
                return false;
            }
        }



        // Метод декодирует символы html для всей строки.
        private string ReplaceHtmlEncodedSymbols(string html)
        {
            /// Регуляраное выражение для всех закодированных симвовлов.
            Regex htmlEncodedRegex = new Regex("&[a-z]+;", RegexOptions.IgnoreCase);

            /// Замена кодов декодированными символами.
            foreach (Match match in htmlEncodedRegex.Matches(html))
            {
                string encoded = WebUtility.HtmlDecode(match.Value);
                html = html.Replace(match.Value, encoded);
            }
            return html;
        }



        // Метод удаляет теги из строки.
        private string RemoveTags(string html)
        {
            Regex tagRegex = new Regex("<.+?>", RegexOptions.IgnoreCase);

            /// Удаление совпадений (html-тегов) из строки. 
            foreach (Match match in tagRegex.Matches(html))
            {
                html = html.Replace(match.Value, string.Empty);
            }

            return html;
        }
    }
}
