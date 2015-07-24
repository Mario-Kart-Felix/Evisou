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
    [Table("PayPalPaymentItemInfo")]
   public class PaymentItemInfoType: ModelBase
    {
       /* public int AuctionID { get; set; }
        public AuctionInfoType Auction { get; set; }*/
        public string Custom { get; set; }
        public string InvoiceID { get; set; }
        public string Memo { get; set; }
        public List<PaymentItemType> PaymentItem { get; set; }
        public string SalesTax { get; set; }

       /* public int SubscriptionID { get; set; }
        public SubscriptionInfoType Subscription { get; set; }*/

    }
}
