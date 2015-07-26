using Evisou.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evisou.Web.AdminApplication.Common
{
    public class AdminUserContext : UserContext
    {
        public AdminUserContext()
            : base(AdminCookieContext.Current)
        {
        }

        public AdminUserContext(IAuthCookie authCookie)
            : base(authCookie)
        {
        }

        public static AdminUserContext Current
        {
            get
            {
                return CacheHelper.GetItem<AdminUserContext>();
            }
        }
    }
}