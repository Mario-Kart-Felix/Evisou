using Evisou.Framework.Contract;
using PayPal.PayPalAPIInterfaceService.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evisou.Ims.Contract.Model.PayPal
{
    [Serializable]
    [Table("PayPalAdditionalFee")]
   public class AdditionalFeeType : ModelBase
    {
        public BasicAmountType Amount { get; set; }
        public string Type { get; set; }
    }
}
