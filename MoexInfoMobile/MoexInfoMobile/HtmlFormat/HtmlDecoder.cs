using MoexInfoMobile.Custom;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using Xamarin.Forms;

namespace MoexInfoMobile.HtmlFormat
{
    public sealed class HtmlDecoder
    {
        public HtmlDecoder(ResourceDictionary styles)
        {
            HtmlDocument = new XmlDocument();
            HtmlStyles = styles;
        }



        private XmlDocument HtmlDocument { get; set; } // Документ html.
        private ResourceDictionary HtmlStyles { get; set; } // Стили элементов интерфейса.



        // Метод создает элементы интерфейса на основе кода html.
        public List<View> DecodeHtml(string html)
        {
            html = ToNormalFormat(html);

            /// Дополнение строки html контейнером.
            if (!html.Contains("<html>"))
            {
                html = $"<html>{html}</html>";
            }

            /// Инициализация документа html.
            HtmlDocument.LoadXml(html);

            /// Извлечение элементов интерфейса.
            List<View> views = new List<View>();

            foreach (XmlNode node in HtmlDocument.DocumentElement)
            {
                View view = DecodeElement(node as XmlElement);
                views.Add(view);
            }

            return views;
        }



        // Метод декодирует теги.
        private View DecodeElement(XmlElement element)
        {
            View view;

            switch (element.Name.ToLower())
            {
                /// Текстовый блок (параграф).
                case "p":
                    view = CreateLabel(element.InnerText, (Style)HtmlStyles["Label"]);
                    break;
                /// Таблица.
                case "table":
                    view = CreateTable(element);
                    break;
                /// Заголовок таблицы.
                case "th":
                    view = CreateTableHeadline(element.InnerText);
                    break;
                /// Обычный текст таблицы.
                case "td":
                    view = CreateLabel(element.InnerText);
                    break;
                /// Ничего из этого.
                default:
                    view = null;
                    break;
            }

            return view;
        }



        // Метод проверяет, является ли строка html-разметкой.
        public bool IsHtmlText(string text)
        {
            return Regex.IsMatch(text, "<.+?>");
        }



        // Метод преобразует строку к требуемому формату.
        public string ToNormalFormat(string html)
        {
            Regex encodedSymbols = new Regex("&[a-z]+;", RegexOptions.IgnoreCase);

            foreach (Match match in encodedSymbols.Matches(html))
            {
                string value = match.Value;

                /// Декодирование спецсимволов html.
                if (value != "&amp;" && value != "&quot;")
                {
                    string decodedValue = WebUtility.HtmlDecode(value);
                    html = html.Replace(value, decodedValue);
                }
            }

            html = html.Replace("\t", "");
            html = html.Replace("<br />", "\n");
            html = html.Replace("<br>", "\n");
            return html;
        }



        // Метод создает текстовый блок.
        private Label CreateLabel(string text, Style labelStyle = null)
        {
            text = text.Replace("&amp;", "&"); /// Замена кода на символ &.
            text = text.Replace("&quot;", "\""); /// Замена кода на символ ".

            /// Удаление ненунжных тегов.
            foreach (Match match in Regex.Matches(text, "<.+?>"))
            {
                text = text.Replace(match.Value, string.Empty);
            }

            /// Создание текстового блока.
            Label label = new Label
            {
                Text = text,
                Style = labelStyle
            };

            return label;
        }



        // Метод создает таблицу.
        private View CreateTable(XmlElement element)
        {
            foreach (XmlNode tNode in element.ChildNodes)
            {
                XmlElement tElement = tNode as XmlElement; /// <thead> или <tbody>

                int rowsCount = tElement.ChildNodes.Count; /// Количество полей таблицы.
                int columnsCount = tElement.ChildNodes[0].ChildNodes.Count; /// Количество атрибутов таблицы.

                View[,] cellsContent = new View[rowsCount, columnsCount];

                /// Перебор полей таблицы.
                for (int i = 0; i < rowsCount; i++)
                {
                    /// Перебор ячеек таблицы в стрке.
                    int j = 0;
                    foreach (XmlNode cellNode in tElement.ChildNodes[i])
                    {
                        View cellContent;
                        /// Если содержимое ячейки является html-разметкой...
                        if (IsHtmlText(cellNode.InnerText))
                        {
                            /// То оно декодируется.
                            cellContent = DecodeElement(cellNode as XmlElement);
                        }
                        /// Если содержимое ячейки является обычным текстом...
                        else
                        {
                            /// То создается объект Label.
                            cellContent = new Label { Text = cellNode.InnerText };
                        }

                        cellsContent[i, j] = cellContent;
                        j++;
                    }
                }

                /// Инициализация таблицы.
                Table table = new Table
                {
                    Style = (Style)HtmlStyles["Table"]
                };

                table.InitializeTable(cellsContent);
                return table;
            }

            return null;
        }



        // Метод создает текст заголовка таблицы.
        private Label CreateTableHeadline(string text)
        {
            Label label = CreateLabel(text);
            label.FontAttributes = FontAttributes.Bold;
            return label;
        }
    }
}