using Evisou.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evisou.Ims.Contract.Model
{
    [Auditable]
    [Table("PaypalApi")]
    public class PaypalApi : ModelBase
    {

        public PaypalApi() { }

        public int UserId { get; set; }
        [StringLength(50)]
        public string UserName { get; set; }


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
}
