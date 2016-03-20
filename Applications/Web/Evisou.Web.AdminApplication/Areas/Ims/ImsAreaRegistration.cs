using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Evisou.Web.AdminApplication.Areas.Ims
{
   

    public class ImsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Ims";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            /**
            要API的URI配置放在前面才行，
            context.Routes.MapHttpRoute在前面，context.MapRoute在后面
            具体参考
              http://stackoverflow.com/questions/11074117/asp-net-webapi-area-support
             Supporting areas in the web API URLs
              **/
            //context.MapHttpRoute(
            //    name: "Ims_DefaultApi",
            //    routeTemplate: "Ims/api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            context.Routes.MapHttpRoute(
                "Ims_DefaultApi",
                //"Ims/api/{controller}/{id}",
                "api/Ims/{controller}/{id}",
                new { id = RouteParameter.Optional }
                );

            context.MapRoute(
                "Ims_default",
                "Ims/{controller}/{action}/{id}",
                new { action = "Default", id = UrlParameter.Optional }
            );




        }

       
    }
}