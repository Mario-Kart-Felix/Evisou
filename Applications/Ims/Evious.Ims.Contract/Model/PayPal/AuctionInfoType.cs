using Evious.Framework.Contract;
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
    [Table("PayPalAuctionInfo")]
    public class AuctionInfoType 
    {
        [Key, ForeignKey("PayPalPaymentItemInfo")]
        public int PayPalPaymentItemInfoID { get; set; }
        public PaymentItemInfoType PayPalPaymentItemInfo { get; set; }
        public string BuyerID { get; set; }
        public string ClosingDate { get; set; }
        public string multiItem { get; set; }
    }
}
