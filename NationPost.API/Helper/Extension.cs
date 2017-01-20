using NationPost.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NationPost.API.Helper
{
    public static class Extension
    {
        public static List<ArticleDTO> ToDTO(this List<Article> articles, bool WithBody = false)
        {
            var articlesDTO = new List<ArticleDTO>();
            foreach (var article in articles)
            {
                articlesDTO.Add(article.ToDTO(WithBody));
            }
            return articlesDTO;

        }

        public static ArticleDTO ToDTO(this Article article, bool WithBody = false)
        {
            ArticleDTO articleDTO = new ArticleDTO();
            articleDTO.ArticleId = article.ArticleId;
            articleDTO.CreatedOn = article.CreatedOn;
            articleDTO.Title = article.Title;
            articleDTO.Description = article.Description;
            articleDTO.Summary = article.Summary;
            articleDTO.Body = article.Body;
            articleDTO.IsValid = article.IsValid;
            articleDTO.IsVisible = article.IsVisible;

            articleDTO.Rating = article.Rating;
            articleDTO.TotalRating = articleDTO.TotalRating;
            articleDTO.Like = article.Like;
            articleDTO.Dislike = article.Dislike;

            articleDTO.Longtitude = article.Longtitude;
            articleDTO.Latitude = article.Latitude;
            articleDTO.Country = article.Country;
            articleDTO.administrative_area_level_1 = article.administrative_area_level_1;
            articleDTO.administrative_area_level_2 = article.administrative_area_level_2;
            articleDTO.locality = article.locality;
            articleDTO.sublocality_level_1 = article.sublocality_level_1;
            articleDTO.sublocality_level_2 = article.sublocality_level_2;
            articleDTO.sublocality_level_3 = article.sublocality_level_3;


            articleDTO.CreatedById = article.CreatedBy?.UserId;
            articleDTO.ArticleType = article.ArticleTypeId.ArticleTypeId;

            articleDTO.Tags = new List<Tag>();
            foreach (var k in article.ArticleTags)
            {
                articleDTO.Tags.Add(k.Tag);
            }

            articleDTO.Body = WithBody ? articleDTO.Body : string.Empty;
            return articleDTO;
        }
    }
}