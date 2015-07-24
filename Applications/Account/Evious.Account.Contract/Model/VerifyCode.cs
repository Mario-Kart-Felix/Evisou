using System;
using System.Linq;
using Evious.Framework.Contract;
using System.Collections.Generic;
using Evious.Framework.Utility;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evious.Account.Contract
{
    [Serializable]
    [Table("VerifyCode")]
    public class VerifyCode : ModelBase
    {
        public Guid Guid { get; set; }
        public string VerifyText { get; set; }
    }

}



