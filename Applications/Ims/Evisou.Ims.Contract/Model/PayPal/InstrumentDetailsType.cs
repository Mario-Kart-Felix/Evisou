using Evisou.Framework.Contract;
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
    [Table("PayPalInstrumentDetails")]
    public class InstrumentDetailsType
    {
        [Key, ForeignKey("PayPalPaymentInfo")]
        public int PayPalPaymentInfoID { get; set; }
        public PaymentInfoType PayPalPaymentInfo { get; set; }
        public string InstrumentCategory { get; set; }
        public string InstrumentID { get; set; }
    }
}
