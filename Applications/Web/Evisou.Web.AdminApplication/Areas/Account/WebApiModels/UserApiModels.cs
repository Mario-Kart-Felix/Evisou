using Evisou.Account.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evisou.Web.AdminApplication.Areas.Account.WebApiModels
{
    public class UserApiModels : TransactionalInformation
    {
        public IEnumerable<UserDTO> Users;
        public UserDTO User;
        public UserApiModels()
        {
            User = new UserDTO();
            Users = new List<UserDTO>();
            
        }

        
    }

    public class UserDTO
    {
        public int ID { get; set; }
        public string LoginName { get; set; }

        public string Password { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Url { get; set; }
        public string CityOrState { get; set; }
        public string CountryCode { get; set; }
        public string ProfileImg { get; set; }
        public bool IsActive { get; set; }
        public string Roles { get; set; }
        public List<RoleDTO> RoleList { get; set; }
        public List<int> IDs { get; set; }
        public List<int> RoleIds { get; set; }


    }
    public class LoginUserDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public string VerifyCode { get; set; }
    }
    public class UserInquiryDTO: InquiryDTO
    {
        public int ID { get; set; }
        public string LoginName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public bool? IsActive { get; set; }
        

        public string CustomActionType { get; set; }

        public string CustomActionName { get; set; }

        public List<int> IDs { get; set; }
        public List<int> RoleIds { get; set; }
        public int Draw { get; set; }

        public int Start{ get; set; }
    }
}