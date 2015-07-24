using System;
using System.Web.Mvc;
using System.Collections.Generic;
using Evious.Account.Contract;
using Evious.Framework.Web;
using Evious.Core.Log;
using Evious.Cms.Contract;
using Evious.Crm.Contract;
using Evious.OA.Contract;
using Evious.Ims.Contract;

namespace Evious.Web
{
    public abstract class ControllerBase : Evious.Framework.Web.ControllerBase
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
