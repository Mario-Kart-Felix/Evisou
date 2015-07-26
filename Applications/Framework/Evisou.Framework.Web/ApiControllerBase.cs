using Evisou.Framework.Contract;
using Evisou.Framework.Utility;
using System;
using System.Web.Http;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace Evisou.Framework.Web
{
    public class ApiControllerBase : ApiController
    {
        /// <summary>
        /// 操作人，传IP....到后端记录
        /// </summary>
        public virtual Operater Operater
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// 分页大小
        /// </summary>
        public virtual int PageSize
        {
            get
            {
                return 15;
            }
        }

        public virtual void ClearOperater()
        {
            //TODO
        }


        /// <summary>
        /// 当前Http上下文信息，用于写Log或其他作用
        /// </summary>
        public WebExceptionContext WebExceptionContext
        {
            get
            {
               
                
                /*  var exceptionContext = new WebExceptionContext
                {
                    IP = Fetch.UserIp,
                    CurrentUrl = Fetch.CurrentUrl,
                    RefUrl = (Request == null || Request.UrlReferrer == null) ? string.Empty : Request.UrlReferrer.AbsoluteUri,
                    IsAjaxRequest = (Request == null) ? false : Request.IsAjaxRequest(),
                    FormData = (Request == null) ? null : Request.Form,
                    QueryData = (Request == null) ? null : Request.QueryString,
                    RouteData = (Request == null || Request.RequestContext == null || Request.RequestContext.RouteData == null) ? null : Request.RequestContext.RouteData.Values
                
                    
                    };*/

                  var exceptionContext = new WebExceptionContext();
                return exceptionContext;
            }
        }

        protected virtual void LogException(Exception exception, WebExceptionContext exceptionContext = null)
        {
            //do nothing!
        }
    }

    /*public class WebExceptionContext
    {
        public string IP { get; set; }
        public string CurrentUrl { get; set; }
        public string RefUrl { get; set; }
        public bool IsAjaxRequest { get; set; }
        public NameValueCollection FormData { get; set; }
        public NameValueCollection QueryData { get; set; }
        public RouteValueDictionary RouteData { get; set; }
    }*/
}
