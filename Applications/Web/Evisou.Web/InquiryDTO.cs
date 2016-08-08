using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evisou.Web
{
    public class InquiryDTO
    {
        public int PageSize { get; set; }
        public int CurrentPageNumber { get; set; }
        public string SortDirection { get; set; }
        public string SortExpression { get; set; }
    }
}
