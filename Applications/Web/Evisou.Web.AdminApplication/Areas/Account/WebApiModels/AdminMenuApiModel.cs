using Evisou.Ims.Contract.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evisou.Web.AdminApplication.Areas.Ims.WebApiModels
{
    public class AdminMenuApiModel : TransactionalInformation
    {
        public AdminMenuApiModel()
        {

        }

        public List<AdminMenuApiDTO> AdminMenuGroups { get; set; }
    }

    public class AdminMenuApiDTO
    {
        public List<AdminMenu> AdminMenuArray { get; set; }
        public string Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Icon { get; set; }

        public string Permission { get; set; }

        public string Info { get; set; }

       
     }
    public class AdminMenu
        {
            public string Id { get; set; }

            public string Name { get; set; }
           
            public string Url { get; set; }
           
            public string Info { get; set; }
          
            public string Icon { get; set; }

            public string Permission { get; set; }
        }
        public class AdminMenuApiInquiryDTO
        {
           
        }

     
}