using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Evisou.Framework.Web;
using Evisou.Account.Contract;
using Evisou.Framework.Contract;
using Evisou.Framework.Utility;

namespace Evisou.Web.AdminApplication.Common
{
    /// <summary>
    /// Authenicate User
    /// </summary>
    public class WebApiPermissionAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {

        public List<EnumBusinessPermission> Permissions { get; set; }

        public WebApiPermissionAttribute(params EnumBusinessPermission[] parameters)
        {
            Permissions = parameters.ToList();

        }


        public AdminUserContext UserContext
        {
            get
            {
                return AdminUserContext.Current;
            }
        }
        /// <summary>
        /// 登录后用户信息
        /// </summary>
        public virtual LoginInfo LoginInfo
        {
            get
            {
                return UserContext.LoginInfo;
            }
        }

        public virtual Operater Operater
        {
            get
            {
                return new Operater()
                {
                    Name = this.LoginInfo == null ? "" : this.LoginInfo.LoginName,
                    Token = this.LoginInfo == null ? Guid.Empty : this.LoginInfo.LoginToken,
                    UserId = this.LoginInfo == null ? 0 : this.LoginInfo.UserID,
                    Time = DateTime.Now,
                    IP = Fetch.UserIp
                };

            }
        }

        public virtual void UpdateOperater(System.Web.Http.Controllers.HttpActionContext filterContext)
        {
            if (this.Operater == null)
                return;

            WCFContext.Current.Operater = this.Operater;
        }

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext filterContext)
        {
            

            var request = filterContext.Request;
            var headers = request.Headers;

            if (!headers.Contains("X-Requested-With") || headers.GetValues("X-Requested-With").FirstOrDefault() != "XMLHttpRequest")
           // if(false)
            {
                TransactionalInformation transactionInformation = new TransactionalInformation();
                transactionInformation.ReturnMessage.Add("非法进入");
                transactionInformation.ReturnStatus = false;
                filterContext.Response = request.CreateResponse<TransactionalInformation>(HttpStatusCode.BadRequest, transactionInformation);
            }
            else {
                bool hasPermission = true;
                this.UpdateOperater(filterContext);
                foreach (var permission in this.Permissions)
                {
                    if (!this.LoginInfo.BusinessPermissionList.Contains(permission))
                    {

                        hasPermission = false;
                        break;
                    }
                }

                if (!hasPermission)
                {
                    TransactionalInformation transactionInformation = new TransactionalInformation();
                    transactionInformation.ReturnMessage.Add("没有权限访问");
                    transactionInformation.ReturnStatus = false;
                    filterContext.Response = request.CreateResponse<TransactionalInformation>(HttpStatusCode.BadRequest, transactionInformation);
                   
                }
            }


         /*@   bool hasPermission = true;
            foreach (var permission in this.Permissions)
            {
                if (!this.LoginInfo.BusinessPermissionList.Contains(permission))
                {
                    hasPermission = false;
                    break;
                }
            }

            if (!hasPermission)
            {
                TransactionalInformation transactionInformation = new TransactionalInformation();
                transactionInformation.ReturnMessage.Add("没有权限访问");
                transactionInformation.ReturnStatus = false;
                filterContext.Response = request.CreateResponse<TransactionalInformation>(HttpStatusCode.BadRequest, transactionInformation);
                //if (Request.UrlReferrer != null)
                //    filterContext.Result = this.Stop("没有权限！", Request.UrlReferrer.AbsoluteUri);
                //else
                //    filterContext.Result = Content("没有权限！");
            }@*/





            //if (!headers.Contains("X-Requested-With") || headers.GetValues("X-Requested-With").FirstOrDefault() != "XMLHttpRequest")
            //{

            //   // TransactionalInformation transactionInformation = new TransactionalInformation();
            //   // transactionInformation.ReturnMessage.Add("Access has been denied.");
            //   // transactionInformation.ReturnStatus = false;
            //    actionContext.Response = request.CreateResponse(HttpStatusCode.BadRequest);
            //}
            //else
            //{
            //    HttpContext ctx = default(HttpContext);
            //    ctx = HttpContext.Current;
            //    if (ctx.User.Identity.IsAuthenticated == false)
            //    {
            //        //TransactionalInformation transactionInformation = new TransactionalInformation();
            //        //transactionInformation.ReturnMessage.Add("Your session has expired.");
            //        // transactionInformation.ReturnStatus = false;
            //        //actionContext.Response = request.CreateResponse<TransactionalInformation>(HttpStatusCode.BadRequest, transactionInformation);
            //        actionContext.Response = request.CreateResponse(HttpStatusCode.BadRequest);
            //    }

            //}


        }

    }
}
