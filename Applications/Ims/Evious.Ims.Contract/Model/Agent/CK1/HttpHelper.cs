using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Evious.Ims.Contract.Model
{
    public class HttpHelper
    {
        public static String HttpGet(string url, Dictionary<String, String> paramters)
        {
            var paramStr = new StringBuilder("");

            foreach (KeyValuePair<String, String> pair in paramters)
            {
                if (pair.Value != null)
                {
                    paramStr.AppendFormat("{0}={1}&", EncodingHelper.UrlEncodeU8(pair.Key.Trim()), EncodingHelper.UrlEncodeU8(pair.Value.Trim()));
                }
            }

            string requestAddress = string.Format("{0}{1}", url, paramStr);
            var webRequest = WebRequest.Create(requestAddress);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "GET";

            //byte[] bytes = Encoding.ASCII.GetBytes(paramStr.ToString());
            Stream stream = null;
            try
            {
                webRequest.ContentLength = 0;
                //stream = webRequest.GetRequestStream();
                //stream.Write(bytes, 0, bytes.Length);

                var webResponse = webRequest.GetResponse();
                var sr = new StreamReader(webResponse.GetResponseStream());
                return sr.ReadToEnd().Trim();
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        public static String HttpPost(string uri, Dictionary<String, String> paramters)
        {
            var webRequest = WebRequest.Create(uri);
            var paramStr = new StringBuilder("");

            foreach (KeyValuePair<String, String> pair in paramters)
            {
                if (pair.Value != null)
                {
                    paramStr.AppendFormat("{0}={1}&", EncodingHelper.UrlEncodeU8(pair.Key.Trim()), EncodingHelper.UrlEncodeU8(pair.Value.Trim()));
                }
            }

            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";

            byte[] bytes = Encoding.ASCII.GetBytes(paramStr.ToString());
            Stream stream = null;
            try
            {
                webRequest.ContentLength = bytes.Length;
                stream = webRequest.GetRequestStream();
                stream.Write(bytes, 0, bytes.Length);         //Send it

                var webResponse = webRequest.GetResponse();
                var sr = new StreamReader(webResponse.GetResponseStream());
                return sr.ReadToEnd().Trim();
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }
    }
}