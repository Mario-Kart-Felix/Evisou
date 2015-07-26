using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evisou.Web.AdminApplication.Areas.Crm
{
    public class CrmAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Crm";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Crm_default",
                "Crm/{controller}/{action}/{id}",
                new { action = "Default", id = UrlParameter.Optional }
            );
        }
    }
}