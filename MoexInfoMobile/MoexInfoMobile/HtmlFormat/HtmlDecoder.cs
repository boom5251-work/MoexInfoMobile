using MoexInfoMobile.Custom;
using System;
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
                    view = CreateTextElement(element);
                    break;
                /// Ссылка.
                case "a":
                    view = CreateLinkLabel(element);
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
                /// Неупорядоченный список.
                case "ul":
                    view = CreateUnorderedList(element);
                    break;
                /// Нумерованный список.
                case "ol":
                    view = CreateNumberedList(element);
                    break;
                /// Ничего из этого.
                default:
                    view = null;
                    break;
            }

            return view;
        }



        // Метод проверяет, является ли строка html-разметкой.
        public static bool IsHtmlText(string text)
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



        // Метод создает контейнер с форматированным текстом.
        private StackLayout CreateFormattedText(XmlElement element)
        {
            StackLayout layout = new StackLayout();

            foreach (XmlNode node in element)
            {
                if (node.NodeType == XmlNodeType.Text)
                {
                    Label label = CreateLabel(node.InnerText);
                    layout.Children.Add(label);
                }
                else if (node.Name.ToLower() == "a")
                {
                    LinkLabel linkLabel = CreateLinkLabel(node as XmlElement);
                    layout.Children.Add(linkLabel);
                }
            }

            return layout;
        }



        // Метод создает текстовый блок.
        private Label CreateLabel(string text)
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
                Style = (Style)HtmlStyles["Label"]
            };

            return label;
        }



        // Метод создает кнопку для переходу по ссылке.
        private LinkLabel CreateLinkLabel(XmlElement element)
        {
            try
            {
                string path = element.GetAttribute("href");
                string text = element.InnerText;
                Uri uri = new Uri(path);

                LinkLabel linkLabel = new LinkLabel
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



        // Метод создает таблицу.
        private View CreateTable(XmlElement tableElement)
        {
            try
            {
                /// Первый элемент <thead> или <tbody>.
                XmlElement firstTElement = tableElement.ChildNodes[0] as XmlElement;
                /// Количество полей и атрибутов таблицы.
                int rowsCount = firstTElement.ChildNodes.Count;
                int columnsCount = firstTElement.ChildNodes[0].ChildNodes.Count;

                /// Массив содержимого ячеек таблицы.
                View[,] cellsContent = new View[rowsCount, columnsCount];

                foreach (XmlNode tNode in tableElement.ChildNodes)
                {
                    /// Перебор полей таблицы.
                    for (int i = 0; i < rowsCount; i++)
                    {
                        /// Перебор ячеек таблицы в стрке.
                        int j = 0;
                        foreach (XmlNode cellNode in tNode.ChildNodes[i])
                        {
                            View cellContent = CreateCellContentElement(cellNode as XmlElement);
                            cellsContent[i, j] = cellContent;
                            j++;
                        }
                    }
                }

                /// Создание и инициализация таблицы.
                HtmlTableView table = new HtmlTableView();
                table.Style = (Style)HtmlStyles["Table"];
                table.InitializeTable(cellsContent);
                return table;
            }
            catch
            {
                // TODO: Добавить обработку исключений.
            }

            return null;
        }



        // Метод создает содержимое ячейки таблицы.
        private View CreateCellContentElement(XmlElement cellElement)
        {
            View cellContent;

            /// Если содержимое ячейки является html-разметкой...
            if (IsHtmlText(cellElement.InnerText))
            {
                /// То оно декодируется.
                cellContent = DecodeElement(cellElement as XmlElement);
            }
            /// Если содержимое ячейки является обычным текстом...
            else
            {
                /// То создается объект Label.
                cellContent = new Label { Text = cellElement.InnerText };
            }

            return cellContent;
        }



        // Метод создает текст заголовка таблицы.
        private Label CreateTableHeadline(string text)
        {
            Label label = CreateLabel(text);
            label.FontAttributes = FontAttributes.Bold;
            return label;
        }



        // Метод создает неупорядоченный список.
        private View CreateUnorderedList(XmlElement ulElement)
        {
            try
            {
                HtmlUnorderedListView ulView = new HtmlUnorderedListView();
                ulView.Style = (Style)HtmlStyles["UnorderedList"];
                ulView.InitializeList(GetLiValues(ulElement));
                return ulView;
            }
            catch
            {
                // TODO: Добавить обработку исключений.
                return null;
            }
        }



        // Метод создает нумерованный список.
        private View CreateNumberedList(XmlElement olElement)
        {
            try
            {
                HtmlNumberedListView olView = new HtmlNumberedListView
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



        // Метод извелкает значения элементов списка.
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