using Evisou.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evisou.Ims.Contract.Model.PayPal
{
    [Serializable]
    [Table("PayPalSellerDetails")]
    public class SellerDetailsType 
    {
        [Key, ForeignKey("PayPalPaymentInfo")]
        public int PayPalPaymentInfoID { get; set; }
        public PaymentInfoType PayPalPaymentInfo { get; set; }
        public string PayPalAccountID { get; set; }
        public string SecureMerchantAccountID { get; set; }
        public string SellerId { get; set; }
        public string SellerRegistrationDate { get; set; }
        public string SellerUserName { get; set; }

    }
}
