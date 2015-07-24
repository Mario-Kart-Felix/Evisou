using Evious.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evious.Ims.Contract.Model.PayPal
{
    [Serializable]
    [Table("PayPalOption")]
   public class OptionType:ModelBase
    {
        public string name { get; set; }
        public string value { get; set; }
        public int  PayPalPaymentItemID { get; set; }
        public virtual PaymentItemType PayPalPaymentItem { get; set; }
    }
}
