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
    [Table("PayPalUserSelectedOption")]
    public class UserSelectedOptionType : ModelBase
    {
        public int PayPalPaymentTransactionID { get; set; }
        public string InsuranceOptionSelected { get; set; }
        public string ShippingCalculationMode { get; set; }

        //public int PaymentItemInfoType { get; set; }
        public BasicAmountType ShippingOptionAmount { get; set; }
        public string ShippingOptionIsDefault { get; set; }
        public string ShippingOptionName { get; set; }
    }
}
