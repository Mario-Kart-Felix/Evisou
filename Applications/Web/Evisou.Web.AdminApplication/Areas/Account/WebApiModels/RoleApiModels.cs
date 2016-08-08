using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evisou.Web.AdminApplication.Areas.Account.WebApiModels
{
    public class RoleApiModels : TransactionalInformation
    {
        public IEnumerable<RoleDTO> Roles;
        public RoleDTO Role;
        public RoleApiModels()
        {
            Role = new RoleDTO();
            Roles = new List<RoleDTO>();

        }
    }
    public class BusinessPermission
    {
        public int Value { get; set; }

        public string Name { get; set; }
    }

    public class RoleDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public bool? Assigned { get; set; }

        public List<BusinessPermission> BusinessPermissions { get; set; }

        public List<int> BusinessPermissionValues { get; set; }
        public string BusinessPermissionString { get; set; }
    }

    public class RoleInquiryDTO: InquiryDTO
    {
        public string Name { get; set; }

       
    }
}