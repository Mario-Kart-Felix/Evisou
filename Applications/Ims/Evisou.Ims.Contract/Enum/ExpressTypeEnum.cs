using Evisou.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evisou.Ims.Contract.Enum
{
    public enum ExpressTypeEnum
    {
        [EnumTitle("[无]", IsDisplay = false)]
        None = 0,

        [EnumTitle("海外仓")]
        Outbound = 1,

        [EnumTitle("国内直发")]
        DirectExpress = 2
    }
}
