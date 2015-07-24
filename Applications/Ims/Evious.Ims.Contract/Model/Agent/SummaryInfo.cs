using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evious.Ims.Contract.Model
{
    public class SummaryInfo
    {
        [JsonProperty(PropertyName = "money")]
        public string Money { get; set; }

        [JsonProperty(PropertyName = "currency_unit")]
        public string CurrencyUnit { get; set; }

        public string Description { get; set; }

        public int Code { get; set; }
    }
}
