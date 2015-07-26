using Evisou.Framework.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evisou.Web
{
    public static class DatatableHelper
    {

        public static List<int> ArrayStringToListInt(string[] param)
        {
            string[] id_Array = param;
            List<int> ids = new List<int>();
            foreach (string i in id_Array)
            {
                ids.Add(int.Parse(i));
            }
            return ids;
        }
    }
}
