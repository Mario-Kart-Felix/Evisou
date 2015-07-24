using Evious.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evious.Ims.Contract.Model.PayPal
{
    [Serializable]
    [Table("PayPalTaxIdDetails")]
    public class TaxIdDetailsType:ModelBase
    {
        public string TaxId { get; set; }
        public string TaxIdType { get; set; }
    }
}
