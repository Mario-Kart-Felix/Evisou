using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evisou.Ims.Contract.Model
{
   public class Express
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }

        [JsonProperty(PropertyName = "detail")]
        public List<DetailInfo> Details { get; set; }

        [JsonProperty(PropertyName = "summary")]
        public List<SummaryInfo> Summary { get; set; }
    }
}
