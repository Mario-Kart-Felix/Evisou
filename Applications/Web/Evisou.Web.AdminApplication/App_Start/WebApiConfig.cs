using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Evisou.Web.AdminApplication
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            // config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "Ims_DefaultApi",
            //    routeTemplate: "Ims/api/{controller}/{id}",
            //    defaults: new { area = "Ims",id = RouteParameter.Optional }
            //);
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

        }
    }
}
