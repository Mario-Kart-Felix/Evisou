using Evisou.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evisou.Ims.Contract.Model
{
    [Serializable]
    [Table("PayPalTransactionPaymentItem")]
    public class PayPalTransactionPaymentItem : ModelBase
    {
        public int PayPalTransactionID { get; set; }

        public virtual PayPalTransaction PayPalTransaction { get; set; }
        public string PaymentItemEbayItemTxnId { get; set; }
        public string PaymentItemName { get; set; }       
        public string PaymentItemNumber { get; set; }
        public string PaymentItemQuantity { get; set; }

        public decimal PaymentItemAmount { get; set; }

        public int? AssociationID { get; set; }

        [InverseProperty("PayPalTransactionPaymentItems")]
        public virtual Association Association { get; set; }
    }
}
