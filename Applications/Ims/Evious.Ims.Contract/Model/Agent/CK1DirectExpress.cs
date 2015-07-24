using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evious.Ims.Contract.Model
{
    public class CK1DirectExpress : Express
    {
        [JsonProperty(PropertyName = "symbol_code")]
        public override string Code { get; set; }

        [JsonProperty(PropertyName = "name")]
        public override string Name { get; set; }

        [JsonProperty(PropertyName = "force_tracking")]
        public bool ForceTracking { get; set; }

        [JsonProperty(PropertyName = "can_import_tracking")]
        public bool CanImportTracking { get; set; }


        [JsonProperty(PropertyName = "can_show")]
        public bool CanShow { get; set; }


        [JsonProperty(PropertyName = "in_service")]
        public bool InService { get; set; }

        [JsonProperty(PropertyName = "max_allow_packing_cm")]
        public string MaxAllowPackingCM { get; set; }

        [JsonProperty(PropertyName = "max_allow_weight_in_kg")]
        public string MaxAllowWeightInKG { get; set; }

        [JsonProperty(PropertyName = "limitation_day_max")]
        public string LimitationDayMax { get; set; }

        [JsonProperty(PropertyName = "limitation_day_min")]
        public string LimitationDayMin { get; set; }

        [JsonProperty(PropertyName = "need_calculate_volumeweight")]
        public bool NeedCalculateVolumeweight { get; set; }

        [JsonProperty(PropertyName = "volumn_weight_factor_cm_to_gram")]
        public int VolumnWeightFactorCMToGram { get; set; }


        [JsonProperty(PropertyName = "type_id")]
        public int TypeID { get; set; }

        [JsonProperty(PropertyName = "express_type")]
        public string ExpressType { get; set; }

    }
}
