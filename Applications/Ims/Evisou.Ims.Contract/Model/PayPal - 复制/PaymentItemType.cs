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
    [Table("PayPalPaymentItem")]
    public class PaymentItemType : ModelBase
    {
        public int AmountID { get; set; }
        public BasicAmountType Amount { get; set; }
        public string CouponAmount { get; set; }
        public string CouponAmountCurrency { get; set; }
        public string CouponID { get; set; }
        public string EbayItemTxnId { get; set; }
        public string HandlingAmount { get; set; }

        //public int InvoiceItemDetailsID { get; set; }
        public InvoiceItemType InvoiceItemDetails { get; set; }
        public string LoyaltyCardDiscountAmount { get; set; }
        public string LoyaltyCardDiscountCurrency { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public List<OptionType> Options { get; set; }
        public string Quantity { get; set; }
        public string SalesTax { get; set; }
        public string ShippingAmount { get; set; }
    }
}
