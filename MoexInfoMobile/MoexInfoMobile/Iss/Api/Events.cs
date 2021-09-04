using MoexInfoMobile.Iss.Data;
using MoexInfoMobile.Iss.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace MoexInfoMobile.Iss.Api
{
    public static class Events
    {
        // Метод возвращает задачу с обобщенным типом "список событий".
        public static async Task<List<Event>> GetEvents(uint start)
        {
            List<Event> events = new List<Event>();

            await Task.Run(() =>
            {
                try
                {
                    string path = $"https://iss.moex.com/iss/events.xml?start={ start }";
                    Uri uri = new Uri(path); /// Путь http-запроса.

                    XmlDocument document = ReqRes.GetDocumentByUri(uri); /// Получение xml-документа.

                    /// Получение элемента rows, который содержит элементы событий (row).
                    XmlElement rows = document.DocumentElement.FirstChild.LastChild as XmlElement;

                    /// Перебор всех элементов row. Создание экземпляров событий.
                    for (int i = 0; i < rows.ChildNodes.Count; i++)
                    {
                        XmlNode row = rows.ChildNodes[i];

                        Event siteEvent = new Event(row);
                        events.Add(siteEvent);
                    }

                }
                catch (UriFormatException)
                {
                    App.Os.ShowToastNotification("Не удалось получить список cобытий.");
                }
                catch (InvalidOperationException)
                {
                    App.Os.ShowToastNotification("Не удалось получить список событий.");
                }
            });

            return events;
        }



        // Метод возвращает задачу с обобщенным типом "подробности события".
        public static async Task<EventInfo> GetEventInfo(ulong id)
        {
            EventInfo eventInfo = null;

            await Task.Run(() =>
            {
                try
                {
                    string path = $"https://iss.moex.com/iss/events/{ id }.xml";
                    Uri uri = new Uri(path); /// Путь http-запроса.

                    XmlDocument document = ReqRes.GetDocumentByUri(uri); /// Получение xml-документа.

                    /// Получение элемента rows, который содержит подробности события (row).
                    XmlElement rows = document.DocumentElement.FirstChild.LastChild as XmlElement;

                    eventInfo = new EventInfo(rows.FirstChild);
                }
                catch (UriFormatException)
                {
                    App.Os.ShowToastNotification("Не удалось получить подробности события.");
                }
                catch (InvalidOperationException)
                {
                    App.Os.ShowToastNotification("Не удалось получить подробности события.");
                }
            });

            return eventInfo;
        }
    }
}
