using Evious.Web.Admin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Evious.Core.Config;

namespace Evious.Web.Admin.Areas.Ims.Controllers
{
    public class TestController : AdminControllerBase
    {
        //
        // GET: /Ims/Test/
        public ActionResult Index()
        {
            var adminMenuConfig = CachedConfigContext.Current.AdminMenuConfig;
            var permissions = AdminUserContext.Current.LoginInfo.BusinessPermissionList.Select(p => p.ToString());
            foreach (var group in adminMenuConfig.AdminMenuGroups)
            {
                if (!string.IsNullOrEmpty(group.Permission) && !permissions.Contains(group.Permission))
                {
                    continue;
                }

                var menus = group.AdminMenuArray.Where(m => string.IsNullOrEmpty(m.Permission) || permissions.Contains(m.Permission));
                var hasSub = menus.Count() > 0;

                if (!hasSub && string.IsNullOrEmpty(group.Url))
                {
                    continue;
                }

                Response.Write(group.Name + "<br/>");

                if (hasSub)
                {
                    foreach (var menu in menus)
                    {
                        Response.Write("***" + menu.Name + "<br/>");
                    }
                           
                }

                Response.Write("<hr/>");
            }
            return View();
        }
	}
}