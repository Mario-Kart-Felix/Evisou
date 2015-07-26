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
    [Table("PayPalPayerInfo")]
    public class PayerInfoType 
    {
        
        [Key, ForeignKey("PayPalPaymentTransaction")]
        public int PayPalPaymentTransactionID { get; set; }

        public PaymentTransactionType PayPalPaymentTransaction { get; set; }
       // public int AddressID { get; set; }
        public AddressType Address { get; set; }
        public string ContactPhone { get; set; }
       
       // public EnhancedPayerInfoType EnhancedPayerInfo { get; set; }
        public string Payer { get; set; }
        public string PayerBusiness { get; set; }
        public CountryCodeType? PayerCountry { get; set; }
        public string PayerID { get; set; }     

       /* public int PayerNameID { get; set; }
        public PersonNameType PayerName { get; set; }
        public PayPalUserStatusCodeType? PayerStatus { get; set; }
        public int TaxIdDetailsID { get; set; }
        public TaxIdDetailsType TaxIdDetails { get; set; }*/
    }
}
