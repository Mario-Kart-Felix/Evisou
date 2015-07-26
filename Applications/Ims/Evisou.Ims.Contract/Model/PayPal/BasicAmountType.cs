using PayPal.PayPalAPIInterfaceService.Model;
using System;

namespace Evisou.Ims.Contract.Model.PayPal
{
    [Serializable]
    //[Table("PayPalBasicAmount")]
    public class BasicAmountType//:ModelBase
    { 
        public CurrencyCodeType? currencyID { get; set; }
        public string value { get; set; }
    }
}
