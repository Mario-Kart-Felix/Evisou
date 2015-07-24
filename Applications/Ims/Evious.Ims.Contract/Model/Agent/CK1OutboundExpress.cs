using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evious.Ims.Contract.Model
{
    public class CK1OutboundExpress : Express
    {
        [JsonProperty(PropertyName = "code")]
        public override string Code { get; set; }

        [JsonProperty(PropertyName = "name")]
        public override string Name { get; set; }

        [JsonProperty(PropertyName = "force_tracking")]
        public bool ForceTracking { get; set; }

        [JsonProperty(PropertyName = "max_allow_packing_cm")]
        public string MaxAllowPackingCM { get; set; }

        [JsonProperty(PropertyName = "max_allow_weight_in_kg")]
        public string MaxAllowWeightInKG { get; set; }

        [JsonProperty(PropertyName = "max_allow_volume_in_cbm")]
        public string MaxAllowVolumeInCBM { get; set; }

        [JsonProperty(PropertyName = "max_allow_volumeLong_in_cm")]
        public string MaxAllowVolumeLongInCM { get; set; }

        [JsonProperty(PropertyName = "max_allow_perimeter_in_cm")]
        public string MaxAllowPerimeterInCM { get; set; }
    }
}
