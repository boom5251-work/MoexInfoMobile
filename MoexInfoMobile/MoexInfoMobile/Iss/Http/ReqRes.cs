using System;
using System.IO;
using System.Net;
using System.Xml;

namespace MoexInfoMobile.Iss.Http
{
    public static class ReqRes
    {
        public static XmlDocument GetDocumentByUri(Uri uri)
        {
            var request = WebRequest.CreateHttp(uri); /// Http-запрос.
            var response = request.GetResponse() as HttpWebResponse; /// Http-ответ.
            string responseString; /// Xml-строка ответа.

            /// Получение потока http-ответа.
            using (var responseStream = response.GetResponseStream())
            {
                /// Чтение потока http-ответа.
                using (var streamReader = new StreamReader(responseStream))
                {
                    responseString = streamReader.ReadToEnd(); /// Запись потока http-ответа в строку.
                }
            }

            var document = new XmlDocument(); /// Новый документ.
            document.LoadXml(responseString); /// Загрузка документа из строки ответа.

            return document;
        }
    }
}