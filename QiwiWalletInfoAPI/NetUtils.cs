using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;

namespace QiwiLogic
{
    /// <summary>
    /// Класс, содержащий вспомогательные методы для работы с WEB
    /// </summary>
    public class NetUtils
    {
        /// <summary>
        /// Обёртка над GET запросом
        /// </summary>
        /// <param name="uri">Путь до веб ресурса</param>
        /// <param name="accept">Значение заголовка Accept</param>
        /// <param name="contentType">Значение заголовка Content-Type</param>
        /// <param name="nvc">Добавочные (пользовательские) заголовки (ключ-значение)</param>
        /// <param name="uriParams">Пары ключ-значение для пути веб-ресурса</param>
        /// <returns>Результат выполнения GET запроса с указанными параметрами</returns>
        public static HttpWebResponse Get
        (
            string uri,
            string accept = "",
            string contentType = "",
            NameValueCollection nvc = null,
            NameValueCollection uriParams = null
        )
        {
            if (uriParams != null)
            {
                uri += "?";
                foreach (var key in uriParams.AllKeys) uri += $"{key}={uriParams[key]}&";
                uri = uri.TrimEnd('&');
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            if (accept != "") request.Accept = accept;
            if (contentType != "") request.ContentType = contentType;
            if (nvc != null) request.Headers.Add(nvc);
            return (HttpWebResponse)request.GetResponse();
        }
        /// <summary>
        /// Обёртка над POST запросом для отправки объекта в виде JSON в качестве полезной нагрузки
        /// </summary>
        /// <param name="uri">Путь до веб ресурса</param>
        /// <param name="data">Объект</param>
        /// <param name="accept">Значение заголовка Accept</param>
        /// <param name="contentType">Значение заголовка Content-Type</param>
        /// <param name="nvc">Добавочные (пользовательские) заголовки (ключ-значение)</param>
        /// <param name="uriParams">Пары ключ-значение для пути веб-ресурса</param>
        /// <param name="propsToRemove">Свойства объекта, которые необходимо исключить из JSON</param>
        /// <returns>Результат выполнения POST запроса с указанными параметрами</returns>
        public static HttpWebResponse JsonPost
        (
            string uri,
            object data,
            string accept = "",
            string contentType = "",
            NameValueCollection nvc = null,
            NameValueCollection uriParams = null,
            List<string> propsToRemove = null
        )
        {
            if (uriParams != null)
            {
                uri += "?";
                foreach (var key in uriParams.AllKeys) uri += $"{key}={uriParams[key]}&";
                uri = uri.TrimEnd('&');
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            if (accept != "") request.Accept = accept;
            if (contentType != "") request.ContentType = contentType;
            if (nvc != null) request.Headers.Add(nvc);
            request.Method = "POST";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                var serialized = (Newtonsoft.Json.Linq.JObject)JsonConvert.SerializeObject(data);
                foreach (var prop in propsToRemove) serialized.Remove(prop);
                streamWriter.Write(serialized);
            }
            return (HttpWebResponse)request.GetResponse();
        }
        /// <summary>
        /// Обёртка над результатом выполнения запроса (ответ сервера на запрос). Применяется в методах "Get" и "JsonPost"
        /// </summary>
        /// <param name="response">Ответ сервера из методов "Get" или "JsonPost"</param>
        /// <returns>Строка, содержащая ответ сервера на запрос</returns>
        public static string GetStringFromResponse(HttpWebResponse response)
        {
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
