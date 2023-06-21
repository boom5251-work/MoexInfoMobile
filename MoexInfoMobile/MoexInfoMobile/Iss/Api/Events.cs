using MoexInfoMobile.Iss.Data;
using MoexInfoMobile.Iss.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace MoexInfoMobile.Iss.Api
{
    /// <summary>
    /// Работа с API событий биржи.
    /// </summary>
    public static class Events
    {
        /// <summary>
        /// Сообщение об ошибке получения списка событий.
        /// </summary>
        private static readonly string EventsErrorMessage = "Не удалось получить список событий.";

        /// <summary>
        /// Сообщение об ошибке получения информации о событии.
        /// </summary>
        private static readonly string EventInfoErrorMessage = "Не удалось получить подробности события.";



        /// <summary>
        /// Инициирует получение списка событий.
        /// </summary>
        /// <param name="start">Начиная с.</param>
        /// <returns>Задача со списком событий.</returns>
        public static async Task<List<Event>> GetEvents(uint start)
        {
            List<Event> events = new List<Event>();

            await Task.Run(() =>
            {
                try
                {
                    string path = $"https://iss.moex.com/iss/events.xml?start={start}";

                    // Путь HTTP-запроса.
                    var uri = new Uri(path);

                    // Получение XML-документа.
                    var document = HttpRequestManager.GetDocumentByUri(uri); 

                    // Получение элемента rows, который содержит элементы событий (row).
                    var rows = (XmlElement?)document?.DocumentElement.FirstChild.LastChild;

                    // Перебор всех элементов row. Создание экземпляров событий.
                    for (int i = 0; i < rows?.ChildNodes.Count; i++)
                    {
                        var row = rows.ChildNodes[i];

                        var siteEvent = new Event(row);
                        events.Add(siteEvent);
                    }
                }
                catch (UriFormatException)
                {
                    App.Os.ShowToastNotification(EventsErrorMessage);
                }
                catch (InvalidOperationException)
                {
                    App.Os.ShowToastNotification(EventsErrorMessage);
                }
            });

            return events;
        }



        /// <summary>
        /// Инициирует получение информации о событии.
        /// </summary>
        /// <param name="id">Идентификатор события.</param>
        /// <returns>Задача с информацией о событии.</returns>
        public static async Task<EventInfo?> GetEventInfo(ulong id)
        {
            EventInfo? eventInfo = null;

            await Task.Run(() =>
            {
                try
                {
                    string path = $"https://iss.moex.com/iss/events/{id}.xml";

                    // Путь HTTP-запроса.
                    var uri = new Uri(path);

                    // Получение XML-документа.
                    var document = HttpRequestManager.GetDocumentByUri(uri);

                    // Получение элемента rows, который содержит подробности события (row).
                    var rows = (XmlElement?)document?.DocumentElement.FirstChild.LastChild;

                    if (rows != null)
                        eventInfo = new EventInfo(rows.FirstChild);
                }
                catch (UriFormatException)
                {
                    App.Os.ShowToastNotification(EventInfoErrorMessage);
                }
                catch (InvalidOperationException)
                {
                    App.Os.ShowToastNotification(EventInfoErrorMessage);
                }
            });

            return eventInfo;
        }
    }
}