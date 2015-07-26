using Evisou.Framework.Contract;
using PayPal.PayPalAPIInterfaceService.Model;
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
    [Table("PayPalPaymentItem")]
    public class PaymentItemType : ModelBase
    {
        //[Key]
        //public int ID { get; set; }
        public int PayPalPaymentItemInfoID { get; set; }
        public virtual PaymentItemInfoType PayPalPaymentItemInfo { get; set; }
        public BasicAmountType Amount { get; set; }
        public string CouponAmount { get; set; }
        public string CouponAmountCurrency { get; set; }
        public string CouponID { get; set; }
        public string EbayItemTxnId { get; set; }
        public string HandlingAmount { get; set; }

        //public int InvoiceItemDetailsID { get; set; }
        //public InvoiceItemType InvoiceItemDetails { get; set; }
        public string LoyaltyCardDiscountAmount { get; set; }
        public string LoyaltyCardDiscountCurrency { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public List<OptionType> Options { get; set; }
        public string Quantity { get; set; }
        public string SalesTax { get; set; }
        public string ShippingAmount { get; set; }

        public int? AssociationID { get; set; }

        [InverseProperty("PaymentItems")]
        public virtual Association Association { get; set; }
    }
}
