using Evisou.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evisou.Ims.Contract.Enum
{
   public enum ExpressAgentEnum
    {
       [EnumTitle("[无]", IsDisplay = false)]
       None = 0,

       [EnumTitle("出口易")]
       出口易 = 1,

       [EnumTitle("三态速递")]
       三态速递 = 2
    }
}
