using Evisou.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evisou.Web.AdminApplication.Areas.Ims.WebApiModels
{
    public class SupplierApiModels : TransactionalInformation
    {
        public IEnumerable<SupplierDTO> Suppliers;
        public SupplierDTO Supplier;
        public SupplierApiModels()
        {
            Supplier = new SupplierDTO();
            Suppliers = new List<SupplierDTO>();

        }
    }

    public class SupplierDTO : ModelBase
    {
        public int UserId { get; set; }
        [StringLength(50)]
        public string UserName { get; set; }

        [DisplayName("平台")]
        public string Platform { get; set; }

        [DisplayName("供应商")]
        public string Name { get; set; }
        [Url]
        [DisplayName("网址")]
        public string URL { get; set; }

        [DisplayName("客服")]
        public string CS { get; set; }

        [DisplayName("客服电话")]
        [DataType(DataType.PhoneNumber)]
        public string CSPhone { get; set; }

        [DisplayName("联系人")]
        public string Contact { get; set; }

        [DisplayName("联系人电话")]
        [DataType(DataType.PhoneNumber)]
        public string ContactPhone { get; set; }

        [DisplayName("地址")]
        public string Address { get; set; }
    }

    public class SupplierInquiryDTO : InquiryDTO
    {
        public string Name { get; set; }
    }
}