using MoexInfoMobile.Custom;
using MoexInfoMobile.Custom.Html;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using Xamarin.Forms;

namespace MoexInfoMobile.HtmlFormat
{
    /// <summary>
    /// Дешифратор html-разметки.
    /// </summary>
    public sealed class HtmlDecoder
    {
        public HtmlDecoder(ResourceDictionary styles)
        {
            HtmlDocument = new XmlDocument();
            HtmlStyles = styles;
        }



        /// <summary>Документ html.</summary>
        private XmlDocument HtmlDocument { get; set; }

        /// <summary>Стили элементов интерфейса.</summary>
        private ResourceDictionary HtmlStyles { get; set; }



        /// <summary>
        /// Создает элементы интерфейса на основе кода html.
        /// </summary>
        /// <param name="html">Строка html.</param>
        /// <returns>Список элементов управления.</returns>
        public List<View> DecodeHtml(string html)
        {
            html = ToNormalFormat(html);

            // Дополнение строки html контейнером.
            if (!html.Contains("<html>"))
            {
                html = $"<html>{html}</html>";
            }

            // Инициализация документа html.
            HtmlDocument.LoadXml(html);

            // Извлечение элементов интерфейса.
            List<View> views = new List<View>();

            foreach (XmlNode node in HtmlDocument.DocumentElement)
            {
                var view = DecodeElement(node as XmlElement);
                views.Add(view);
            }

            return views;
        }



        /// <summary>
        /// Декодоирует теги.
        /// </summary>
        /// <param name="element">Элемент html.</param>
        /// <returns>Представление, созданное на основе тегов.</returns>
        private View DecodeElement(XmlElement element)
        {
            View view;

            switch (element.Name.ToLower())
            {
                // Текстовый блок (параграф).
                case "p":
                    view = CreateTextElement(element);
                    break;
                // Ссылка.
                case "a":
                    view = CreateLinkLabel(element);
                    break;
                // Таблица.
                case "table":
                    view = CreateTable(element);
                    break;
                // Заголовок таблицы.
                case "th":
                    view = CreateTableHeadline(element.InnerText);
                    break;
                // Обычный текст таблицы.
                case "td":
                    view = CreateLabel(element.InnerText);
                    break;
                // Неупорядоченный список.
                case "ul":
                    view = CreateUnorderedList(element);
                    break;
                // Нумерованный список.
                case "ol":
                    view = CreateNumberedList(element);
                    break;
                // Ничего из этого.
                default:
                    view = null;
                    break;
            }

            return view;
        }



        /// <summary>
        /// Проверяет, является ли строка html-разметкой.
        /// </summary>
        /// <param name="text">Проверяема строка.</param>
        /// <returns>True, если строка является html-разметкой.</returns>
        public static bool IsHtmlText(string text) => Regex.IsMatch(text, "<.+?>");



        /// <summary>
        /// Преобразует строку к требуемому формату.
        /// </summary>
        /// <param name="html">Форматирует html-строку.</param>
        /// <returns></returns>
        public string ToNormalFormat(string html)
        {
            var encodedSymbols = new Regex("&[a-z]+;", RegexOptions.IgnoreCase);

            foreach (Match match in encodedSymbols.Matches(html))
            {
                string value = match.Value;

                // Декодирование некоторых спецсимволов html.
                if (value != "&amp;" && value != "&quot;")
                {
                    string decodedValue = WebUtility.HtmlDecode(value);
                    html = html.Replace(value, decodedValue);
                }
            }

            return html.Replace("\t", "")
                .Replace("<br />", "\n")
                .Replace("<br>", "\n");
        }



        /// <summary>
        /// Создает текстовый блок.
        /// </summary>
        /// <param name="element">Элемент html.</param>
        /// <returns>Текстовый блок.</returns>
        private View CreateTextElement(XmlElement element)
        {
            View result;

            if (element.InnerXml.ToLower().Contains("</a>"))
            {
                result = CreateFormattedText(element);
            }
            else
            {
                result = CreateLabel(element.InnerText);
            }

            return result;
        }



        /// <summary>
        /// Создает контейнер с форматированным текстом.
        /// </summary>
        /// <param name="element">Элемент html.</param>
        /// <returns>Блок форматированного текста.</returns>
        private StackLayout CreateFormattedText(XmlElement element)
        {
            var layout = new StackLayout();

            foreach (XmlNode node in element)
            {
                if (node.NodeType == XmlNodeType.Text)
                {
                    var label = CreateLabel(node.InnerText);
                    layout.Children.Add(label);
                }
                else if (node.Name.ToLower() == "a")
                {
                    var linkLabel = CreateLinkLabel(node as XmlElement);
                    layout.Children.Add(linkLabel);
                }
            }

            return layout;
        }



        /// <summary>
        /// Создает текстовый элемент.
        /// </summary>
        /// <param name="text">Текст.</param>
        /// <returns>Текстовый элемент.</returns>
        private Label CreateLabel(string text)
        {
            // Замена некоторых кодов на спецсимволы.
            text = text.Replace("&amp;", "&")
                .Replace("&quot;", "\"");

            // Удаление ненунжных тегов.
            foreach (Match match in Regex.Matches(text, "<.+?>"))
            {
                text = text.Replace(match.Value, string.Empty);
            }

            // Создание текстового элемента.
            var label = new Label
            {
                Text = text,
                Style = (Style)HtmlStyles["Label"]
            };

            return label;
        }



        /// <summary>
        /// Создает кнопку для переходу по ссылке.
        /// </summary>
        /// <param name="element">Элемент html.</param>
        /// <returns></returns>
        private HtmlLinkLabel CreateLinkLabel(XmlElement element)
        {
            try
            {
                string path = element.GetAttribute("href");
                string text = element.InnerText;

                var uri = new Uri(path);

                var linkLabel = new HtmlLinkLabel
                {
                    Uri = uri,
                    Text = text,
                    Style = (Style)HtmlStyles["LinkLabel"]
                };
                
                return linkLabel;
            }
            catch
            {
                // TODO: Добавить обработку исключений.
                return null;
            }
        }



        /// <summary>
        /// Создает таблицу.
        /// </summary>
        /// <param name="tableElement">Элемент html (таблица).</param>
        /// <returns>Таблица.</returns>
        private View CreateTable(XmlElement tableElement)
        {
            try
            {
                // Первый элемент <thead> или <tbody>.
                var firstTElement = tableElement.ChildNodes[0] as XmlElement;

                // Количество полей и атрибутов таблицы.
                int rowsCount = firstTElement.ChildNodes.Count;
                int columnsCount = firstTElement.ChildNodes[0].ChildNodes.Count;

                // Массив содержимого ячеек таблицы.
                View[,] cellsContent = new View[rowsCount, columnsCount];

                foreach (XmlNode tNode in tableElement.ChildNodes)
                {
                    // Перебор полей таблицы.
                    for (int i = 0; i < rowsCount; i++)
                    {
                        // Перебор ячеек таблицы в стрке.
                        int j = 0;
                        foreach (XmlNode cellNode in tNode.ChildNodes[i])
                        {
                            var cellContent = DecodeElement(cellNode as XmlElement);
                            cellsContent[i, j] = cellContent;
                            j++;
                        }
                    }
                }

                // Создание и инициализация таблицы.
                var table = new HtmlTableView
                {
                    Style = (Style)HtmlStyles["Table"]
                };

                table.InitializeTable(cellsContent);
                return table;
            }
            catch
            {
                // TODO: Добавить обработку исключений.
            }

            return null;
        }



        /// <summary>
        /// Создает текст заголовка таблицы.
        /// </summary>
        /// <param name="text">Текст заголовка.</param>
        /// <returns>Текстовый элемент.</returns>
        private Label CreateTableHeadline(string text)
        {
            var label = CreateLabel(text);
            label.FontAttributes = FontAttributes.Bold;
            return label;
        }



        /// <summary>
        /// Создает неупорядоченный список.
        /// </summary>
        /// <param name="ulElement">Элемент html (неупорядоченный список).</param>
        /// <returns>Неупорядоченный список.</returns>
        private View CreateUnorderedList(XmlElement ulElement)
        {
            try
            {
                var ulView = new HtmlUnorderedListView
                {
                    Style = (Style)HtmlStyles["UnorderedList"]
                };

                ulView.InitializeList(GetLiValues(ulElement));
                return ulView;
            }
            catch
            {
                // TODO: Добавить обработку исключений.
                return null;
            }
        }



        /// <summary>
        /// Создает нумерованный список.
        /// </summary>
        /// <param name="olElement">Элемент html (нумерованный список).</param>
        /// <returns></returns>
        private View CreateNumberedList(XmlElement olElement)
        {
            try
            {
                var olView = new HtmlNumberedListView
                {
                    MarkerCharacter = '.',
                    Style = (Style)HtmlStyles["NumberedList"]
                };
                
                olView.InitializeList(GetLiValues(olElement));
                return olView;
            }
            catch
            {
                // TODO: Добавить обработку исключений.
                return null;
            }
            
        }



        /// <summary>
        /// Извелкает значения элементов списка.
        /// </summary>
        /// <param name="element">Элемент html.</param>
        /// <returns>Строки списка.</returns>
        private string[] GetLiValues(XmlNode element)
        {
            List<string> values = new List<string>();

            foreach (XmlNode liNode in element)
            {
                values.Add(liNode.InnerText);
            }

            return values.ToArray();
        }
    }
}