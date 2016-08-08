using Evisou.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evisou.Web.AdminApplication.Areas.Ims.WebApiModels
{
    public class PurchaseApiModels : TransactionalInformation
    {
        public IEnumerable<PurchaseDTO> Purchases;
        public PurchaseDTO Purchase;
        public PurchaseApiModels()
        {
            Purchase = new PurchaseDTO();
            Purchases = new List<PurchaseDTO>();

        }
    }

    public class PurchaseDTO : ModelBase
    {
        public int UserId { get; set; }
        [StringLength(50)]
        public string UserName { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [DisplayName("采购日期")]
        public DateTime? PurchaseDate { get; set; }

        [DisplayName("采购交易号")]
        public string PurchaseTransactionNumber { get; set; }

        public int SupplierID { get; set; }
    }

    public class PurchaseInquiryDTO : InquiryDTO
    {
        public string PurchaseTransactionNumber { get; set; }
    }
}