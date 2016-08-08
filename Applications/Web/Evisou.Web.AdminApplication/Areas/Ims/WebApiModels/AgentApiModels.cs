using Evisou.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evisou.Web.AdminApplication.Areas.Ims.WebApiModels
{
    public class AgentApiModels : TransactionalInformation
    {
        public IEnumerable<AgentDTO> Agents;
        public AgentDTO Agent;
        public AgentApiModels()
        {
            Agent = new AgentDTO();
            Agents = new List<AgentDTO>();

        }
    }

    public class AgentDTO : ModelBase
    {
        public int UserId { get; set; }
        [StringLength(50)]
        public string UserName { get; set; }
        [Required]
        [DisplayName("物流代理")]
        public string AgentName { get; set; }
        [Required]
        [DisplayName("代理编号")]
        public string AgentCode { get; set; }

        [DisplayName("物流代理地址")]
        public string Address { get; set; }

        [Url]
        [DisplayName("物流代理官网")]
        public string WebURL { get; set; }


        [DisplayName("销售经理手机")]
        public string SaleMangerMobile { get; set; }

        [DisplayName("销售经理")]
        public string SaleManager { get; set; }

        [DisplayName("收件人")]
        public string Consignee { get; set; }

        [DisplayName("仓库固话")]
        public string ConsigneeTel { get; set; }

        [DisplayName("电邮")]
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("客服QQ号码")]
        public string CSQQ { get; set; }

        [DisplayName("客服")]
        public string CS { get; set; }
    }

    public class AgentInquiryDTO : InquiryDTO
    {
        public string AgentName { get; set; }

        public string AgentCode { get; set; }
    }
}