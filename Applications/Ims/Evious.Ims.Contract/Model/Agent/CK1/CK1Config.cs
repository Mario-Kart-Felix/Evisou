using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evious.Ims.Contract.Model
{
    public class CK1Config
    {
        private static readonly string userKey = "nacvo0gi7g";//填写你的TOKEN
        private static readonly string token = "5D14B692A4CDFAD8ADC2D9975E5CAC38";

        #region soap auth
        public static string getUserKey() { return userKey; }
        public static string getToken() { return token; }
        #endregion

    }
}