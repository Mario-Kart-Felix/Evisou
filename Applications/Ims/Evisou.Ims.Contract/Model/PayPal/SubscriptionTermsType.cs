using Evisou.Framework.Contract;
//using PayPal.PayPalAPIInterfaceService.Model;
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
    [Table("PayPalSubscriptionTerms")]
   public class SubscriptionTermsType
    {
        [Key]
        public int ID { get; set; }
        public int PayPalSubscriptionInfoID { get; set; }
        public virtual SubscriptionInfoType PayPalSubscriptionInfo { get; set; }
        public BasicAmountType Amount { get; set; }
        public string period { get; set; }
    }
}
