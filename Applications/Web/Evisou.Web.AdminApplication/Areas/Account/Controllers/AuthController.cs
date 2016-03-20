using Evisou.Account.Contract;
using Evisou.Framework.Contract;
using Evisou.Framework.Utility;
using Evisou.Framework.Web;
using Evisou.Web.AdminApplication.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evisou.Web.AdminApplication.Areas.Account.Controllers
{
    public class AuthController : AdminControllerBase
    {
        [AuthorizeIgnore]
        public ActionResult Login()
        {

            // Response.Write(this.CookieContext.VerifyCodeGuid.ToString());
            return View();
        }

        [HttpPost]
        [AuthorizeIgnore]
        public ActionResult Login(string username, string password, string verifycode)
        {

            if (!VerifyCodeHelper.CheckVerifyCode(verifycode, this.CookieContext.VerifyCodeGuid))
            {
                ModelState.AddModelError("error", "验证码错误");
                return View();
            }

            var loginInfo = this.AccountService.Login(username, password);

            if (loginInfo != null)
            {
                Session["SessionForLogin"] = loginInfo.LoginName;
                this.CookieContext.UserToken = loginInfo.LoginToken;
                this.CookieContext.UserName = loginInfo.LoginName;
                this.CookieContext.UserId = loginInfo.UserID;
                return RedirectToAction("Index");
            }
            else
            {               
                ModelState.AddModelError("error", "用户名或密码错误");
                return View();
            }
        }

        [AuthorizeIgnore]
        public ActionResult Logout()
        {
            Session["SessionForLogin"] = null;
            this.AccountService.Logout(this.CookieContext.UserToken);
            this.CookieContext.UserToken = Guid.Empty;
            this.CookieContext.UserName = string.Empty;
            this.CookieContext.UserId = 0;
            return RedirectToAction("Login");
        }

        public ActionResult ModifyPwd()
        {
            var model = this.AccountService.GetUser(this.LoginInfo.UserID);
            return View(model);
        }

        [HttpPost]
        public ActionResult ModifyPwd(FormCollection collection)
        {
            var model = this.AccountService.GetUser(this.LoginInfo.UserID);
            this.TryUpdateModel<User>(model);

            try
            {
                this.AccountService.ModifyPwd(model);
            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                return View(model);
            }

            return this.RefreshParent();
        }

        public ActionResult Index()
        {
            
            return View();
        }

        [AuthorizeIgnore]
        public ActionResult VerifyImage()
        {
            var s1 = new ValidateCode_Style4();
            string code = "6666";
            byte[] bytes = s1.CreateImage(out code);

            this.CookieContext.VerifyCode = code;

            return File(bytes, @"image/jpeg");

        }
        [AuthorizeIgnore]
        public ActionResult Lock()
        {
            Session["SessionForLogin"] = null;
            return RedirectToAction("UnLock"); 
        }



        [AuthorizeIgnore]
        public ActionResult UnLock()
        {
            if (this.CookieContext.UserToken != Guid.Empty&& this.CookieContext.UserName != string.Empty &&  this.CookieContext.UserId != 0)
            {
                //if (Session["SessionForLogin"] != null)
                //{
                //    JsUtil.JavaScriptLocationHref(Url.Action("Index"));
                //}
                
               // Session["SessionForLogin"] = null;               
                return View();
            }
            else
            {              
                return RedirectToAction("Logout", "Auth", new { Area = "Account"}); 
            }
           
        }

        [HttpPost]
        [AuthorizeIgnore]
        public ActionResult UnLock(string username, string password)
        {
            if (this.CookieContext.UserToken != Guid.Empty
                && this.CookieContext.UserName != string.Empty
                && this.CookieContext.UserId != 0)
            {
                var loginInfo = this.AccountService.Login(username, password);
                if (loginInfo != null)
                {
                    Session["SessionForLogin"] = loginInfo.LoginName;
                    this.CookieContext.UserName = loginInfo.LoginName;                
                    JsUtil.GoHistory(-2);
                    return View();                              
                }
                else
                {
                    ModelState.AddModelError("error", "用户名或密码错误");
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
            
        }

        [AuthorizeIgnore]
        public string KeepAlive()
        {
            Session["SessionForLogin"] = Common.AdminUserContext.Current.LoginInfo.LoginName;
            return "OK";
            //if (Convert.ToString(Session["SessionForLogin"]) == Common.AdminUserContext.Current.LoginInfo.LoginName)
            //{
            //    Session["SessionForLogin"] = Common.AdminUserContext.Current.LoginInfo.LoginName;
            //    return "OK";
            //}
            //else
            //{
            //    Session["SessionForLogin"] = null;
            //    return "False";
            //}
            
        }

        [AuthorizeIgnore]
        public ActionResult test()
        {
            return View();
        }
    }
}