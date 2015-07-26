using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evisou.Framework.Contract
{
    /// <summary>
    /// 用于BLL方法提传入条件
    /// </summary>
    public class Request : ModelBase
    {
        #region Json
        /// <summary>
        /// Request sequence number sent by DataTable, same value must be returned in response
        /// </summary>       
        //public string sEcho { get; set; }
        public string draw { get; set; }
        /// <summary>
        /// Text used for filtering
        /// </summary>
        //public string sSearch { get; set; }
        public string search { get; set; }
        /// <summary>
        /// Number of records that should be shown in table
        /// </summary>
        //public int iDisplayLength { get; set; }
        public int length { get; set; }
        /// <summary>
        /// First record that should be shown(used for paging)
        /// </summary>
        //public int iDisplayStart { get; set; }
        public int start { get; set; }
        /// <summary>
        /// Number of columns in table
        /// </summary>
        //public int iColumns { get; set; }
        public int columns { get; set; }
        /// <summary>
        /// Number of columns that are used in sorting
        /// </summary>
       // public int iSortingCols { get; set; }
        
        /// <summary>
        /// Comma separated list of column names
        /// </summary>
       // public string sColumns { get; set; }

       // public string sAction { get; set; }
        public string customActionType { get; set; }

        public string customActionName { get; set; }

        //public  List<int> id { get; set; }
        public string action { get; set; }
        #endregion
        public Request()
        {
            PageSize = int.MaxValue;
        }

        public int Top
        {
            set
            {
                this.PageSize = value;
                this.PageIndex = 1;
            }
        }

        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
