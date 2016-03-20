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

        public List<int> IDs { get; set; }


    }

    public class UserInquiryDTO
    {
        public int ID { get; set; }
        public string LoginName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public bool? IsActive { get; set; }
        public int PageSize { get; set; }
        public int CurrentPageNumber { get; set; }
        public string SortDirection { get; set; }
        public string SortExpression { get; set; }

        public string CustomActionType { get; set; }

        public string CustomActionName { get; set; }

        public List<int> IDs { get; set; }

    }
}