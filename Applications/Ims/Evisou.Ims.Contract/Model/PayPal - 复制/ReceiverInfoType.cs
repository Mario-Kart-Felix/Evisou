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
    [Table("PayPalReceiverInfo")]
    public class ReceiverInfoType :ModelBase
    {       
        public string Business { get; set; }
        public string Receiver { get; set; }
        public string ReceiverID { get; set; }
    }
}
