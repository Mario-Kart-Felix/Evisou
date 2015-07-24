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
    [Table("PayPalDiscount")]
    public class DiscountType : ModelBase
    {
        public BasicAmountType Amount { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string RedeemedOfferID { get; set; }
        public RedeemedOfferType? RedeemedOfferType { get; set; }
    }
}
