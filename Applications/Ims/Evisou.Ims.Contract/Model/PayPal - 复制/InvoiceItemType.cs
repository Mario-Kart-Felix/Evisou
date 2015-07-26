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
    [Table("PayPalInvoiceItem")]
    public class InvoiceItemType : ModelBase
    {
        public List<AdditionalFeeType> AdditionalFees { get; set; }
        public string Description { get; set; }
        public List<DiscountType> Discount { get; set; }
        public string EAN { get; set; }
        public string ISBN { get; set; }
        public decimal? ItemCount { get; set; }
        public UnitOfMeasure? ItemCountUnit { get; set; }

        public int ItemPriceID { get; set; }
        public BasicAmountType ItemPrice { get; set; }
        public string ModelNumber { get; set; }
        public string MPN { get; set; }
        public string Name { get; set; }
        public string PLU { get; set; }

        //public int PriceID { get; set; }
        public BasicAmountType Price { get; set; }
        public bool? Reimbursable { get; set; }
        public string ReturnPolicyIdentifier { get; set; }
        public string SKU { get; set; }
        public string StyleNumber { get; set; }
        public bool? Taxable { get; set; }
        public decimal? TaxRate { get; set; }
    }
}
