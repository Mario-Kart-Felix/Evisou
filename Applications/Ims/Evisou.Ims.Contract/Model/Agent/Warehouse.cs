using Newtonsoft.Json;

namespace Evisou.Ims.Contract.Model
{
    public class Warehouse
    {
        [JsonProperty(PropertyName = "code")]
        public virtual string Code { get; set; }
        [JsonProperty(PropertyName = "name")]
        public virtual string Name { get; set; }
    }
}
