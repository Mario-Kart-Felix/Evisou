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
    [Table("PayPalReceiverInfo")]
    public class ReceiverInfoType 
    {
        [Key, ForeignKey("PayPalPaymentTransaction")]
        public int PayPalPaymentTransactionID { get; set; }
        public PaymentTransactionType PayPalPaymentTransaction { get; set; }

        public string Business { get; set; }
        public string Receiver { get; set; }
        public string ReceiverID { get; set; }
    }
}
