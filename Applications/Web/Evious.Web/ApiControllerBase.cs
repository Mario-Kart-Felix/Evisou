using Evious.Account.Contract;
using Evious.Cms.Contract;
using Evious.Core.Log;
using Evious.Crm.Contract;
using Evious.Framework.Web;
using Evious.Ims.Contract;
using Evious.OA.Contract;
using System;
using System.Web.Http;

namespace Evious.Web
{
    public abstract class ApiControllerBase  : Evious.Framework.Web.ApiControllerBase
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

    }
}
