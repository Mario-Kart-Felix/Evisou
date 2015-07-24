using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evious.Web.AdminApplication.Areas.Ims
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
            context.MapRoute(
                "Ims_default",
                "Ims/{controller}/{action}/{id}",
                new { action = "Default", id = UrlParameter.Optional }
            );
        }
    }
}