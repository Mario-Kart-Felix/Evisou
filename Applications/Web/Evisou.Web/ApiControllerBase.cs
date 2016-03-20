using Evisou.Account.Contract;
using Evisou.Cms.Contract;
using Evisou.Core.Log;
using Evisou.Crm.Contract;
using Evisou.Framework.Web;
using Evisou.Ims.Contract;
using Evisou.OA.Contract;
using System;
using System.Net.Http;
using System.Web.Http;

namespace Evisou.Web
{
    public abstract class ApiControllerBase  : Evisou.Framework.Web.ApiControllerBase
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
