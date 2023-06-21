using System;
using System.IO;
using System.Net;
using System.Xml;

namespace MoexInfoMobile.Iss.Http
{
    /// <summary>
    /// Менеджер отправки HTTP-запросов к API ИСС.
    /// </summary>
    public static class HttpRequestManager
    {
        /// <summary>
        /// Отправляет HTTP-запрос на получение XML-документа к API ИСС.
        /// </summary>
        /// <param name="uri">URI запроса.</param>
        /// <returns>XML-документ</returns>
        public static XmlDocument? GetDocumentByUri(Uri uri)
        {
            try
            {
                // HTTP-запрос.
                var request = WebRequest.CreateHttp(uri);

                // HTTP-ответ.
                var response = request.GetResponse() as HttpWebResponse;

                // XML-строка ответа.
                string responseString;

                // Чтение потока HTTP-ответа.
                using (var responseStream = response?.GetResponseStream())
                using (var streamReader = new StreamReader(responseStream))
                    responseString = streamReader.ReadToEnd();

                // Загрузка документа из строки ответа.
                var document = new XmlDocument();
                document.LoadXml(responseString);

                return document;
            }
            catch
            {
                return null;
            }
        }
    }
}