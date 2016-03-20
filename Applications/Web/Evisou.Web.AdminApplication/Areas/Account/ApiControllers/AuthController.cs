using Evisou.Core.Config;
using Evisou.Web.AdminApplication.Areas.Ims.WebApiModels;
using Evisou.Web.AdminApplication.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace Evisou.Web.AdminApplication.Areas.Account.ApiControllers
{
    public class AuthController : AdminApiControllerBase
    {
        public HttpResponseMessage GetAllAuth()//AdminMenu()
        {
            AdminMenuConfig adminMenuConfig = CachedConfigContext.Current.AdminMenuConfig;
            IEnumerable<string> permissions = AdminUserContext.Current.LoginInfo.BusinessPermissionList.Select(p => p.ToString());
            
            List<AdminMenuApiDTO> AdminMenuGroups = new List<AdminMenuApiDTO>();
            AdminMenuApiModel adminMenu = new AdminMenuApiModel();
            foreach (var group in adminMenuConfig.AdminMenuGroups)
            {
                List<Ims.WebApiModels.AdminMenu> adminMenuList = new List<Ims.WebApiModels.AdminMenu>();
                foreach (var array in group.AdminMenuArray)
                {
                    adminMenuList.Add(new Ims.WebApiModels.AdminMenu
                    {
                        Icon = array.Icon ?? null,
                        Id = array.Id ?? null,
                        Info = array.Info ?? null,
                        Name = array.Name ?? null,
                        Permission = array.Permission ?? null,
                        Url = array.Url ?? null
                    });
                };

                AdminMenuGroups.Add(new AdminMenuApiDTO
                {
                    Icon = group.Icon ?? null,
                    Id = group.Id ?? null,
                    Info = group.Info ?? null,
                    Name = group.Name ?? null,
                    Permission = group.Permission ?? null,
                    Url = group.Url ?? "javascript:;",
                    AdminMenuArray = adminMenuList ?? null
                });

            }
            adminMenu.AdminMenuGroups = AdminMenuGroups;
            var response = Request.CreateResponse(HttpStatusCode.OK, adminMenu);
            return response;
        }
    }
}