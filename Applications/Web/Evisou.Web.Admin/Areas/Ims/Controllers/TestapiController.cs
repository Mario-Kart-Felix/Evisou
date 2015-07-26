using Evious.Core.Config;
using Evious.Web.Admin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Evious.Web.Admin.Areas.Ims.Controllers
{
    public class TestapiController : ApiController
    {
        public IEnumerable<AdminMenuGroup> Get()
        {
             var adminMenuConfig = CachedConfigContext.Current.AdminMenuConfig;
             var permissions = AdminUserContext.Current.LoginInfo.BusinessPermissionList.Select(p => p.ToString());


             return adminMenuConfig.AdminMenuGroups;
        }

    }
}
