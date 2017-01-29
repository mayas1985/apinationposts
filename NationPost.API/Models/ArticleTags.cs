using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NationPost.API.Models
{
    [Table("ArticleTags", Schema = "NP")]    
    public class ArticleTags
    {
        [Key]
        public int ArticleTagId { get; set; }
        public virtual Article article { get; set; }
        public virtual Tag Tag { get; set; }
    }
}

