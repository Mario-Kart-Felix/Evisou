using Evisou.Ims.Contract.chukou1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evisou.Ims.Contract.Model
{
    public class PriceRequest
    {
        public string ExpressService { set; get; }

        public string ShiptoCity { get; set; }

        public string ShipToState { get; set; }

        public string ShipToCountryName { get; set; }

        public string ShipToZip { get; set; }

        public float Weight { get; set; }

        public Packing Packing { get; set; }

    }
}
