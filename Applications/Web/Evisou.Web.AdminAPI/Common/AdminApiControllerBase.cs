using Evious.Account.Contract;
using Evious.Core.Config;
using Evious.Framework.Contract;
using Evious.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evious.Web.AdminAPI.Common
{
    public abstract class AdminApiControllerBase : ApiControllerBase
    {
        public AdminCookieContext CookieContext
        {
            get
            {
                return AdminCookieContext.Current;
            }
        }

        public AdminUserContext UserContext
        {
            get
            {
                return AdminUserContext.Current;
            }
        }

        public CachedConfigContext ConfigContext
        {
            get
            {
                return CachedConfigContext.Current;
            }
        }

        /// <summary>
        /// 重写分页Size
        /// </summary>
        public override int PageSize
        {
            get
            {
                return 12;
            }
        }

        /// <summary>
        /// 操作人，为了记录操作历史
        /// </summary>
        public override Operater Operater
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

        /// <summary>
        /// 用户Token，每次页面都会把这个UserToken标识发送到服务端认证
        /// </summary>
        public virtual Guid UserToken
        {
            get
            {
                return CookieContext.UserToken;
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

        /// <summary>
        /// 登录后用户信息里的用户权限
        /// </summary>
        public virtual List<EnumBusinessPermission> PermissionList
        {
            get
            {
                var permissionList = new List<EnumBusinessPermission>();

                if (this.LoginInfo != null)
                    permissionList = this.LoginInfo.BusinessPermissionList;

                return permissionList;
            }
        }
    }
}