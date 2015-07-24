using Evious.Framework.Contract;
using PayPal.PayPalAPIInterfaceService.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evious.Ims.Contract.Model.PayPal
{
    [Serializable]
    [Table("PayPalSubscriptionInfo")]
    public class SubscriptionInfoType
    {
        [Key, ForeignKey("PayPalPaymentItemInfo")]
        public int PayPalPaymentItemInfoID { get; set; }
        public PaymentItemInfoType PayPalPaymentItemInfo { get; set; }
        public string EffectiveDate { get; set; }
        public string Password { get; set; }
        public string reattempt { get; set; }
        public string Recurrences { get; set; }
        public string recurring { get; set; }
        public string RetryTime { get; set; }
        public string SubscriptionDate { get; set; }
        public string SubscriptionID { get; set; }
        public List<SubscriptionTermsType> Terms { get; set; }
        public string Username { get; set; }
    }
}
