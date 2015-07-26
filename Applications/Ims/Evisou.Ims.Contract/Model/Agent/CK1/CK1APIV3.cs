using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace Evisou.Ims.Contract.Model
{
    public class CK1APIV3
    {
        public class CK1API_SDK_Base
        {
            private static string API_BASE_URL
            {
                get
                {
                    var url = ConfigurationManager.AppSettings["ck1:apiv3:url"];
                    if (string.IsNullOrEmpty(url))
                    {
                        url = "http://api.chukou1.com/v3/";
                    }
                    return url;
                }
            }

            protected static Dictionary<string, string> AuthParams =
                new Dictionary<string, string>
                {
                    {"token", CK1Config.getToken()},
                    {"user_key", CK1Config.getUserKey()},
                };

            public static string CreateRequestUrl(Dictionary<string, string> dispatcher)
            {
                var paramStr = new StringBuilder();

                foreach (var pair in AuthParams)
                {
                    paramStr.AppendFormat("{0}={1}&", EncodingHelper.UrlEncodeU8(pair.Key.Trim()), EncodingHelper.UrlEncodeU8(pair.Value.Trim()));
                }

                return string.Format("{0}{1}?{2}", API_BASE_URL,
                                        string.Join("/", dispatcher.Values.ToArray()), paramStr);
            }

            static void Main(string[] args)
            {
            }
        }

        [JsonObject(MemberSerialization.OptIn)]
        public class API_V1_ResponseMeta
        {
            public static readonly int OK_STATUS = 200;
            public static readonly int AUTH_FAIL_STATUS = 401;
            public static readonly int FORBIDDEN_STATUS = 403;
            public static readonly int MALFORM_REQ_STATUS = 400;
            public static readonly int NO_EXIST_STATUS = 404;
            public static readonly int SERVER_ERROR = 500;
            public static readonly String OK_REMARK = "OK";
            public static readonly String DEFAULT_CALLER = "N/A";

            [JsonProperty(PropertyName = "code")]
            public int code { get; set; }

            [JsonProperty(PropertyName = "link")]
            public String link { get; set; }

            [JsonProperty(PropertyName = "description")]
            public String description { get; set; }

            [JsonProperty(PropertyName = "caller")]
            public String caller { get; internal set; }

            public API_V1_ResponseMeta()
            {
                code = OK_STATUS;
                link = "";
                caller = DEFAULT_CALLER;
                description = OK_REMARK;
            }

            public bool IsOKMeta()
            {
                return code == OK_STATUS;
            }
        }

        [JsonObject(MemberSerialization.OptIn)]
        public class API_V3_Response
        {
            [JsonProperty(PropertyName = "meta")]
            public API_V1_ResponseMeta meta { get; set; }

            [JsonProperty(PropertyName = "body")]
            public Object body { get; set; }
        }
    }
}