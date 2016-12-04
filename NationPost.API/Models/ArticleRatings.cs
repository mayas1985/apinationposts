using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NationPost.API.Models
{
    [Table("ArticleRatings", Schema = "NP")]
    public class ArticleRatings
    {
        [Key]
        public int ArticleRatingId { get; set; }

        public Guid ArticleId { get; set; }

        public Guid? UserId { get; set; }

        public int Rating { get; set; }

        public RatingType ratingType { get; set; }

        public string IPAdditionalInfo { get; set; }
    }

    public enum RatingType
    {
        RatingGiven,
        Liked,
        Disliked
    }
}