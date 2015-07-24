using PayPal.PayPalAPIInterfaceService.Model;
using System;

namespace Evious.Ims.Contract.Model.PayPal
{
    [Serializable]
    //[Table("PayPalBasicAmount")]
    public class BasicAmountType//:ModelBase
    { 
        public CurrencyCodeType? currencyID { get; set; }
        public string value { get; set; }
    }
}
