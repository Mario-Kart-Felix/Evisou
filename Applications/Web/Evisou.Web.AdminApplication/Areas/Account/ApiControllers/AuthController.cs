using Evisou.Core.Config;
using Evisou.Web.AdminApplication.Areas.Account.WebApiModels;
using Evisou.Web.AdminApplication.Areas.Ims.WebApiModels;
using Evisou.Web.AdminApplication.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Evisou.Web.AdminApplication.Areas.Account.ApiControllers
{
    public class AuthController : AdminApiControllerBase
    {
        [HttpGet]
        public HttpResponseMessage GetAllAuth()//AdminMenu()
        {
            if (AdminUserContext.Current.LoginInfo == null)
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                return response;
            }
            else
            {
                AdminMenuConfig adminMenuConfig = CachedConfigContext.Current.AdminMenuConfig;
                IEnumerable<string> permissions = AdminUserContext.Current.LoginInfo.BusinessPermissionList.Select(p => p.ToString());

                List<AdminMenuApiDTO> AdminMenuGroups = new List<AdminMenuApiDTO>();
                AdminMenuApiModel adminMenu = new AdminMenuApiModel();
                foreach (var group in adminMenuConfig.AdminMenuGroups)
                {
                    List<Ims.WebApiModels.AdminMenu> adminMenuList = new List<Ims.WebApiModels.AdminMenu>();
                    foreach (var array in group.AdminMenuArray)
                    {
                        adminMenuList.Add(new Ims.WebApiModels.AdminMenu
                        {
                            Icon = array.Icon ?? null,
                            Id = array.Id ?? null,
                            Info = array.Info ?? null,
                            Name = array.Name ?? null,
                            Permission = array.Permission ?? null,
                            Url = array.Url ?? null
                        });
                    };

                    AdminMenuGroups.Add(new AdminMenuApiDTO
                    {
                        Icon = group.Icon ?? null,
                        Id = group.Id ?? null,
                        Info = group.Info ?? null,
                        Name = group.Name ?? null,
                        Permission = group.Permission ?? null,
                        Url = group.Url ?? "javascript:;",
                        AdminMenuArray = adminMenuList ?? null
                    });

                }
                adminMenu.AdminMenuGroups = AdminMenuGroups;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, adminMenu);
                return response;
            }
            
            
        }

        [HttpPost]
        public HttpResponseMessage Login(HttpRequestMessage request, [FromBody] LoginUserDTO loginUserDTO)
        {
            UserApiModels userWebApiModel = new UserApiModels();
            List<string> Errors = new List<string>();
            if (!VerifyCodeHelper.CheckVerifyCode(loginUserDTO.VerifyCode, this.CookieContext.VerifyCodeGuid))
            {
                Errors.Clear();
                Errors.Add("验证码错误");
                userWebApiModel.ReturnMessage = Errors;
                var badResponse = Request.CreateResponse<UserApiModels>(HttpStatusCode.BadRequest, userWebApiModel);
                return badResponse;
            }
            if(string.IsNullOrEmpty(loginUserDTO.UserName)) loginUserDTO.UserName = "";
            if(string.IsNullOrEmpty(loginUserDTO.Password)) loginUserDTO.Password = "";
            var loginInfo = this.AccountService.Login(loginUserDTO.UserName, loginUserDTO.Password);

            if (loginInfo != null)
            {
                this.CookieContext.UserToken = loginInfo.LoginToken;
                this.CookieContext.UserName = loginInfo.LoginName;
                this.CookieContext.UserId = loginInfo.UserID;
              
                userWebApiModel.IsAuthenicated = true;
                userWebApiModel.ReturnMessage.Add("登录成功");
                var response = Request.CreateResponse<UserApiModels>(HttpStatusCode.OK, userWebApiModel);
                return response;
            }
            else
            {
                Errors.Clear();
                Errors.Add("用户名或密码错误");
                userWebApiModel.ReturnMessage = Errors;
                var badResponse = Request.CreateResponse<UserApiModels>(HttpStatusCode.BadRequest, userWebApiModel);
                return badResponse;
            }
            
            
        }

        [HttpDelete]
        public HttpResponseMessage Logout()
        {
            this.AccountService.Logout(this.CookieContext.UserToken);
            this.CookieContext.UserToken = Guid.Empty;
            this.CookieContext.UserName = string.Empty;
            this.CookieContext.UserId = 0;
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;

        }


    }
}