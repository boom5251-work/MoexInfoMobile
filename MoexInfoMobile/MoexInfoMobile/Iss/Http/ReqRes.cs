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
            HttpWebRequest request = WebRequest.CreateHttp(uri); /// Http-запрос.
            HttpWebResponse response = request.GetResponse() as HttpWebResponse; /// Http-ответ.
            string responseString; /// Xml-строка ответа.

            /// Получение потока http-ответа.
            using (Stream responseStream = response.GetResponseStream())
            {
                /// Чтение потока http-ответа.
                using (StreamReader streamReader = new StreamReader(responseStream))
                {
                    responseString = streamReader.ReadToEnd(); /// Запись потока http-ответа в строку.
                }
            }

            XmlDocument document = new XmlDocument(); /// Новый документ.
            document.LoadXml(responseString); /// Загрузка документа из строки ответа.

            return document;
        }
    }
}
