using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace NationPost.API.Models
{
    [Table("Articles", Schema = "NP")]
    public class Article
    {
        public Guid ArticleId { get; set; }
        [Required]
        public User CreatedBy { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }

        [MaxLength(250, ErrorMessage = "Only 250 characters are allowed")]
        public String Title { get; set; }

        [MaxLength(250, ErrorMessage = "Only 250 characters are allowed")]
        public String Description { get; set; }

        [MaxLength(250, ErrorMessage = "Only 250 characters are allowed")]
        public String Summary { get; set; }


        public String Body { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public virtual ArticleType ArticleTypeId { get; set; }

        public int Rating { get; set; }
        public int Like { get; set; }
        public int Dislike { get; set; }

        public DbGeography coords { get; set; }

        public virtual ICollection<ArticleTags> ArticleTags { get; set; }

        //Tag as notMapped property, this would be utilized to allow sending tagid as a param for POST
        [NotMapped]
        public ICollection<Tag> Tags { get; set; }

    }

    [NotMapped]
    public class ArticleDTO : Article
    {
        public Guid? CreatedById { get; set; }

        [Required]
        public int ArticleType { get; set; }

        
    }
}