
using System;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace Evisou.Account.Contract
{
    public interface IAccountService
    {
        LoginInfo GetLoginInfo(Guid token);
        LoginInfo Login(string loginName, string password);
        void Logout(Guid token);
        void ModifyPwd(User user);

        User GetUser(int id);
        IEnumerable<User> GetUserList(UserRequest request = null);
        void SaveUser(User user);
        void DeleteUser(List<int> ids);
        //void ChangeStatus(List<int> ids,string status);
        HttpResponseMessage UserDataExport(List<int> ids, string type);
   
        Role GetRole(int id);
        IEnumerable<Role> GetRoleList(RoleRequest request = null);
        void SaveRole(Role role);
        void DeleteRole(List<int> ids);
        HttpResponseMessage RoleDataExport(List<int> ids, string type);

        Guid SaveVerifyCode(string verifyCodeText);
        bool CheckVerifyCode(string verifyCodeText, Guid guid);


    }
}
