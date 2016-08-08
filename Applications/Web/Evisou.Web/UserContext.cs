using System;
using Evisou.Account.Contract;
using Evisou.Core.Cache;

namespace Evisou.Web
{
    public class UserContext
    {
        protected IAuthCookie authCookie;

        public UserContext(IAuthCookie authCookie)
        {
            this.authCookie = authCookie;
        }

        public LoginInfo LoginInfo
        {
            get
            {
                return CacheHelper.GetItem<LoginInfo>("LoginInfo", () =>
                {
                    var cc = authCookie.UserToken;
                    if (authCookie.UserToken == Guid.Empty)
                        return null;

                   
                    var bb = ServiceContext.Current.AccountService.GetLoginInfo(authCookie.UserToken);
                    var loginInfo = ServiceContext.Current.AccountService.GetLoginInfo(authCookie.UserToken);
                    var aa = loginInfo;
                    if (loginInfo != null && loginInfo.UserID > 0 && loginInfo.UserID != this.authCookie.UserId)
                        throw new Exception("非法操作，试图通过网站修改Cookie取得用户信息！");

                    return loginInfo;
                });
            }
        }
    }
}
