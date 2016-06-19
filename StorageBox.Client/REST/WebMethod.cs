using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.IO;

namespace StorageBox.Client.REST
{
    public class WebMethod
    {
        public string EndPoint { get; set; }
        public Dictionary<string, string> RequestHeaders { get; set; }
        public Dictionary<string, string> ResponseHeaders { get; set; }

        public WebMethod(string uri)
        {
            this.EndPoint = uri;
            this.RequestHeaders = new Dictionary<string, string>();
            this.ResponseHeaders = new Dictionary<string, string>();
        }

        public string SubmitAndRetrieveJson(string jsonData, bool logHeaders = false, string requestMethod = "POST")
        {
            string result = "";
            var webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(this.EndPoint);
            webRequest.Method = requestMethod;
            webRequest.ContentType = "application/json;charset=UTF-8";

            foreach (string headerKey in this.RequestHeaders.Keys)
            {
                webRequest.Headers.Add(headerKey, this.RequestHeaders[headerKey]);
            }

            Stream reqStream = webRequest.GetRequestStream();
            byte[] postArray = Encoding.UTF8.GetBytes(jsonData);
            reqStream.Write(postArray, 0, postArray.Length);
            reqStream.Close();

            WebResponse webResponse = webRequest.GetResponse();
            StreamReader sr = new StreamReader(webResponse.GetResponseStream());
            result = sr.ReadToEnd();

            //This is used to retrieve authentication header response:
            if (logHeaders)
            {
                foreach (string headerKey in webResponse.Headers.AllKeys)
                {
                    this.ResponseHeaders.Add(headerKey, webResponse.Headers[headerKey]);
                }
            }
            return result;
        }

        /// <summary>
        /// Given a json string, submits data to the given end point
        /// </summary>
        /// <param name="jsonData">The json string to be submitted</param>
        /// <param name="logHeaders">Indicates if response headers should be retrieved from response</param>
        /// <returns></returns>
        public T SubmitJson<T>(string jsonData, bool logHeaders = false)
        {
            object response = null;
            string result = this.SubmitAndRetrieveJson(jsonData, logHeaders);
            if (!string.IsNullOrEmpty(result))
                response = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);

            return (T)response;
        }
    }

}
