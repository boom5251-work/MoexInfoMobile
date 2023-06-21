using MoexInfoMobile.Custom.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using Xamarin.Forms;

namespace MoexInfoMobile.HtmlFormat
{
    /// <summary>
    /// Дешифратор HTML-разметки.
    /// </summary>
    public sealed class HtmlDecoder
    {
        public HtmlDecoder(ResourceDictionary styles)
        {
            HtmlDocument = new XmlDocument();
            HtmlStyles = styles;
        }



        /// <summary>
        /// Документ HTML.
        /// </summary>
        public XmlDocument HtmlDocument { get; }

        /// <summary>
        /// Стили элементов интерфейса.
        /// </summary>
        public ResourceDictionary HtmlStyles { get; }



        /// <summary>
        /// Создает элементы интерфейса на основе кода HTML.
        /// </summary>
        /// <param name="html">Строка HTML.</param>
        /// <returns>Список элементов управления.</returns>
        public List<View> DecodeHtml(string html)
        {
            html = ToNormalFormat(html);

            // Дополнение строки HTML контейнером.
            if (!Regex.IsMatch(html, @"<html>.<html>"))
                html = $"<html>{html}</html>";


            // Инициализация документа HTML.
            HtmlDocument.LoadXml(html);

            // Извлечение элементов интерфейса.
            var views = new List<View>();

            foreach (XmlNode node in HtmlDocument.DocumentElement)
            {
                var view = DecodeElement((XmlElement)node);

                if (view != null)
                    views.Add(view);
            }

            return views;
        }



        /// <summary>
        /// Декодирует теги.
        /// </summary>
        /// <param name="element">Элемент html.</param>
        /// <returns>Представление, созданное на основе тегов.</returns>
        private View? DecodeElement(XmlElement element)
        {
            return element.Name.ToLower() switch
            {
                // Текстовый блок (параграф).
                "p" => CreateTextElement(element),
                // Ссылка.
                "a" => CreateLinkLabel(element),
                // Таблица.
                "table" => CreateTable(element),
                // Заголовок таблицы.
                "th" => CreateTableHeadline(element.InnerText),
                // Обычный текст таблицы.
                "td" => CreateLabel(element.InnerText),
                // Неупорядоченный список.
                "ul" => CreateUnorderedList(element),
                // Нумерованный список.
                "ol" => CreateNumberedList(element),
                // Ничего из этого.
                _ => null,
            };
        }



        /// <summary>
        /// Проверяет, является ли строка HTML-разметкой.
        /// </summary>
        /// <param name="text">Проверяема строка.</param>
        /// <returns>True, если строка является HTML-разметкой.</returns>
        public static bool IsHtmlText(string text) =>
            Regex.IsMatch(text, "<.+?>");



        /// <summary>
        /// Преобразует строку к требуемому формату.
        /// </summary>
        /// <param name="html">Форматирует HTML-строку.</param>
        /// <returns></returns>
        public string ToNormalFormat(string html)
        {
            var encodedSymbols = new Regex("&[a-z]+;", RegexOptions.IgnoreCase);

            foreach (Match match in encodedSymbols.Matches(html).Cast<Match>())
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
                result = CreateFormattedText(element);
            else
                result = CreateLabel(element.InnerText);

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
                    var linkLabel = CreateLinkLabel((XmlElement)node);
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

            // Удаление ненужных тегов.
            foreach (Match match in Regex.Matches(text, "<.+?>").Cast<Match>())
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
        private HtmlLinkLabel? CreateLinkLabel(XmlElement element)
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
        private View? CreateTable(XmlElement tableElement)
        {
            try
            {
                // Первый элемент <thead> или <tbody>.
                var firstTElement = (XmlElement)tableElement.ChildNodes[0];

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
                        // Перебор ячеек таблицы в строке.
                        int j = 0;
                        foreach (XmlNode cellNode in tNode.ChildNodes[i])
                        {
                            var cellContent = DecodeElement((XmlElement)cellNode);

                            if (cellContent != null)
                                cellsContent[i, j] = cellContent;

                            j++;
                        }
                    }
                }

                // Создание и инициализация таблицы.
                var table = new HtmlTableView(cellsContent)
                {
                    Style = (Style)HtmlStyles["Table"]
                };

                return table;
            }
            catch
            {
                // TODO: Добавить обработку исключений.
                return null;
            }
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
        private View? CreateUnorderedList(XmlElement ulElement)
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
        private View? CreateNumberedList(XmlElement olElement)
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
        /// Извлекает значения элементов списка.
        /// </summary>
        /// <param name="element">Элемент html.</param>
        /// <returns>Строки списка.</returns>
        private string[] GetLiValues(XmlNode element)
        {
            var values = new List<string>();

            foreach (XmlNode liNode in element)
                values.Add(liNode.InnerText);

            return values.ToArray();
        }
    }
}