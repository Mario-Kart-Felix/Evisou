using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Evisou.Web.AdminApplication.Areas.Account
{
    public class AccountAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Account";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.MapHttpRoute(
                "Account_DefaultApi",
                "api/Account/{controller}/{id}",
                new { id = RouteParameter.Optional }
                );

            //context.Routes.MapHttpRoute(
            //    "User",
            //    "api/Account/{controller}/{action}/{id}",
            //    new { id = RouteParameter.Optional }
            //    );

            context.MapRoute(
                "Account_default",
                "Account/{controller}/{action}/{id}",
                new { action = "Default", id = UrlParameter.Optional }
            );
        }
    }
}