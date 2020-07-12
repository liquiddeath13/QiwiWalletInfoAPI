using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;

namespace QiwiLogic
{
    public class NetUtils
    {
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
        public static string GetStringFromResponse(HttpWebResponse response)
        {
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
