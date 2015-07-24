using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evious.Ims.Contract.Model
{
    public class CK1V3Request
    {
        public string Category { set; get; }

        public string Handler { set; get; }

        public string Action { set; get; }

        public Dictionary<String, String> Parameters { set; get; }
    }
}
