using Evious.Framework.Contract;
using PayPal.PayPalAPIInterfaceService.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evious.Ims.Contract.Model.PayPal
{
    [Serializable]
    [Table("PayPalSubscriptionTerms")]
   public class SubscriptionTermsType:ModelBase
    {
        public int AmountID { get; set; }
        public BasicAmountType Amount { get; set; }
        public string period { get; set; }
    }
}
