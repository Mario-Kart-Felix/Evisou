using Evious.Framework.Contract;
using PayPal.PayPalAPIInterfaceService.Model;
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
    [Table("PayPalAddress")]
    public class AddressType
    {

        [Key, ForeignKey("PayPalPayerInfo")]
        public int PayPalPayerInfoID { get; set; }

        public PayerInfoType PayPalPayerInfo { get; set; }

        public string AddressID { get; set; }
        public AddressNormalizationStatusCodeType? AddressNormalizationStatus { get; set; }
        public AddressOwnerCodeType? AddressOwner { get; set; }
        public AddressStatusCodeType? AddressStatus { get; set; }
        public string CityName { get; set; }
        public CountryCodeType? Country { get; set; }
        public string CountryName { get; set; }
        public string ExternalAddressID { get; set; }
        public string InternationalName { get; set; }
        public string InternationalStateAndCity { get; set; }
        public string InternationalStreet { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string PostalCode { get; set; }
        public string StateOrProvince { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }

    }
}
