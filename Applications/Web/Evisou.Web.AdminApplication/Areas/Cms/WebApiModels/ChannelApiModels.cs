using Evisou.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evisou.Web.AdminApplication.Areas.Cms.WebApiModels
{
    public class ChannelApiModels : TransactionalInformation
    {
        public ChannelDTO Channel;
        public IEnumerable<ChannelDTO> Channels;
        public ChannelApiModels()
        {
            Channel = new ChannelDTO();
            Channels = new List<ChannelDTO>();
        }


    }

    public class ChannelDTO: ModelBase
    {
        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        [StringLength(300)]
        public string CoverPicture { get; set; }

        [StringLength(300)]
        public string Desc { get; set; }

        public bool IsActive { get; set; }

        public int Hits { get; set; }

        public virtual List<ArticleDTO> Articles { get; set; }
    }


    public class ChannelInquiryDTO : InquiryDTO
    {
        public string Name { get; set; }

        [StringLength(300)]
        public string CoverPicture { get; set; }

        [StringLength(300)]
        public string Desc { get; set; }

        public bool IsActive { get; set; }

        public int Hits { get; set; }
    }
}