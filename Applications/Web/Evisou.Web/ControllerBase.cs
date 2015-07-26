using System;
using System.Web.Mvc;
using System.Collections.Generic;
using Evisou.Account.Contract;
using Evisou.Framework.Web;
using Evisou.Core.Log;
using Evisou.Cms.Contract;
using Evisou.Crm.Contract;
using Evisou.OA.Contract;
using Evisou.Ims.Contract;

namespace Evisou.Web
{
    public abstract class ControllerBase : Evisou.Framework.Web.ControllerBase
    {
        public virtual IAccountService AccountService
        {
            get
            {
                return ServiceContext.Current.AccountService;
            }
        }

        public virtual ICmsService CmsService
        {
            get
            {
                return ServiceContext.Current.CmsService;
            }
        }

        public virtual ICrmService CrmService
        {
            get
            {
                return ServiceContext.Current.CrmService;
            }
        }

        public virtual IOAService OAService
        {
            get
            {
                return ServiceContext.Current.OAService;
            }
        }

        public virtual IImsService ImsService
        {
            get 
            {
                return ServiceContext.Current.ImsService;
            }
        }
        protected override void LogException(Exception exception, 
            WebExceptionContext exceptionContext = null)
        {
            base.LogException(exception);

            var message = new
            {
                exception = exception.Message,
                exceptionContext = exceptionContext,
            };

            Log4NetHelper.Error(LoggerType.WebExceptionLog, message, exception);
        }

        public IDictionary<string, object> CurrentActionParameters { get; set; }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
    }
}
