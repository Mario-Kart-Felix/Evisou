using Evious.Core.Cache;
using Evious.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evious.Web.AdminApplication.Common
{
    public class AdminCookieContext : CookieContext
    {
        public static AdminCookieContext Current
        {
            get
            {
                return CacheHelper.GetItem<AdminCookieContext>();
            }
        }

        public override string KeyPrefix
        {
            get
            {
                return Fetch.ServerDomain + "_AdminContext_";
            }
        }
    }
}