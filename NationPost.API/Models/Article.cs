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
        public ArticleType ArticleTypeId { get; set; }

        /// <summary>
        /// Rating will be submission of different rating given by peoples
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Is equalt to rating given by number of people ..eg if 5 people had given rating then TotalRating will be 5
        /// </summary>
        public int TotalRating { get; set; }
        public int Like { get; set; }
        public int Dislike { get; set; }

        //public DbGeography coords { get; set; }

        public virtual ICollection<ArticleTags> ArticleTags { get; set; }

        //Tag as notMapped property, this would be utilized to allow sending tagid as a param for POST
        [NotMapped]
        public ICollection<Tag> Tags { get; set; }

        public string Longtitude { get; set; }
        public string Latitude { get; set; }
        public string Country { get; set; }
        public string administrative_area_level_1 { get; set; }
        public string administrative_area_level_2 { get; set; }
        public string locality { get; set; }
        public string sublocality_level_1 { get; set; }
        public string sublocality_level_2 { get; set; }
        public string sublocality_level_3 { get; set; }


    }

    [NotMapped]
    public class ArticleDTO : Article
    {
        public Guid? CreatedById { get; set; }

        [Required]
        public int ArticleType { get; set; }


    }
}