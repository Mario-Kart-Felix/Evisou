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
    [Table("PayPalInstrumentDetails")]
    public class InstrumentDetailsType : ModelBase
    {      
        public string InstrumentCategory { get; set; }
        public string InstrumentID { get; set; }
    }
}
