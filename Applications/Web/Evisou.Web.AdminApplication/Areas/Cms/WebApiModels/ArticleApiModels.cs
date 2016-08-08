using Evisou.Cms.Contract;
using Evisou.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evisou.Web.AdminApplication.Areas.Cms.WebApiModels
{
   
    public class ArticleApiModels : TransactionalInformation
    {
        public ArticleDTO Article;
        public IEnumerable<ArticleDTO> Articles;

        public ArticleApiModels()
        {
            Article = new ArticleDTO();
            Articles = new List<ArticleDTO>();
        }
    }

    public class ArticleDTO: ModelBase
    {

        [StringLength(100)]
        [Required]
        public string Title { get; set; }

        [StringLength(300)]
        public string CoverPicture { get; set; }

        [StringLength(int.MaxValue)]
        public string Content { get; set; }

        public int Hits { get; set; }

        public int Diggs { get; set; }

        public bool IsActive { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public int SelectedChannelId { get; set; }
        public virtual ChannelDTO Channel { get; set; }
        public virtual List<ChannelDTO> Channels { get; set; }
        public virtual List<TagDTO> Tags { get; set; }
        public string TagString
        {
            get
            {
                if (Tags != null)
                    return string.Join(",", Tags.Select(t => t.Name));
                else
                    return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    this.Tags = value.Split(',').Select(t => new TagDTO() { Name = t }).ToList();
                else
                    this.Tags = new List<TagDTO>();
            }
        }
    }

    public class ArticleInquiryDTO : InquiryDTO
    {
        public string Title { get; set; }

        public string Content { get; set; }


    }

    public class TagDTO
    {
        [StringLength(100)]
        [Required]
        public string Name { get; set; }
        public int Hits { get; set; }

        public virtual List<ArticleDTO> Articles { get; set; }
    }

    
}