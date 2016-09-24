using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NationPost.API.Models
{
    [Table("ArticleType", Schema = "NP")]    
    public class ArticleType
    {
        public int ArticleTypeId { get; set; }
        public String Name { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}