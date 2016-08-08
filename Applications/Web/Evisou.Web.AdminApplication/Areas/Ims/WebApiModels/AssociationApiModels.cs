using Evisou.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evisou.Web.AdminApplication.Areas.Ims.WebApiModels
{
    public class AssociationApiModels : TransactionalInformation
    {
        public IEnumerable<AssociationDTO> Associations;
        public AssociationDTO Association;
        public AssociationApiModels()
        {
            Association = new AssociationDTO();
            Associations = new List<AssociationDTO>();

        }
    }

    public class AssociationDTO : ModelBase
    {
        public int ProductID { get; set; }

        public int PaypalApiID { get; set; }
        public int UserId { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [DisplayName("标题")]
        [StringLength(160)]
        public string ItemTitle { get; set; }

        [DisplayName("序号")]
        [StringLength(160)]
        public string ItemNumber { get; set; }


        [DisplayName("储存地点")]
        public string StorePlace { get; set; }

        [DisplayName("销售地点")]
        public string SellingPlace { get; set; }

        
    }

    public class AssociationInquiryDTO : InquiryDTO
    {
        public int ProductID { get; set; }
        public int PaypalApiID { get; set; }
        public string ItemNumber { get; set; }
    }
}