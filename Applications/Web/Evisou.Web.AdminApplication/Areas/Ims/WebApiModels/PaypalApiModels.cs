using Evisou.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evisou.Web.AdminApplication.Areas.Ims.WebApiModels
{
    public class PaypalApiModels : TransactionalInformation
    {
        public IEnumerable<PaypalApiDTO> PaypalApis;
        public PaypalApiDTO PaypalApi;
        public PaypalApiModels()
        {
            PaypalApi = new PaypalApiDTO();
            PaypalApis = new List<PaypalApiDTO>();

        }
    }

    public class PaypalApiDTO : ModelBase
    {
        public int UserId { get; set; }
        [StringLength(50)]

        [DisplayName("API用户名")]
        [MaxLength(50)]
        [Required]
        public string ApiUserName { get; set; }

        [DisplayName("是否激活")]
        public bool IsActive { get; set; }

        [DisplayName("API密码")]
        [MaxLength(50)]
        [Required]
        public string ApiPassword { get; set; }

        [DisplayName("PP账号")]
        [EmailAddress]
        [Required]
        public string PPAccount { get; set; }

        [DisplayName("Paypal签名")]
        [Required]
        public string Signature { get; set; }
    }

    public class PaypalApiInquiryDTO : InquiryDTO
    {
        public string ApiUserName { get; set; }
        public string PPAccount { get; set; }
    }
}