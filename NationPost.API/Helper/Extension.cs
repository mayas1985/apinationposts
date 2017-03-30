using NationPost.API.Models;
using NationPost.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NationPost.API.Helper
{
    public static class Extension
    {

        public static DAL.Article ToDAL ( this Models.Article article, BlogDBContextLinqDataContext db)
        {
            var articleDAL = new DAL.Article();
            articleDAL.ArticleId = article.ArticleId;
            articleDAL.CreatedOn = article.CreatedOn;
            articleDAL.Title = article.Title;
            articleDAL.Description = article.Description;
            articleDAL.Summary = article.Summary;
            articleDAL.Body = article.Body;
            articleDAL.IsValid = article.IsValid;
            articleDAL.IsVisible = article.IsVisible;

            articleDAL.Rating = article.Rating;
            articleDAL.TotalRating = articleDAL.TotalRating;
            articleDAL.Like = article.Like;
            articleDAL.Dislike = article.Dislike;

            articleDAL.Longtitude = article.Longtitude;
            articleDAL.Latitude = article.Latitude;
            articleDAL.Country = article.Country;
            articleDAL.administrative_area_level_1 = article.administrative_area_level_1;
            articleDAL.administrative_area_level_2 = article.administrative_area_level_2;
            articleDAL.locality = article.locality;
            articleDAL.sublocality_level_1 = article.sublocality_level_1;
            articleDAL.sublocality_level_2 = article.sublocality_level_2;
            articleDAL.sublocality_level_3 = article.sublocality_level_3;


            articleDAL.CreatedBy_UserId = article.CreatedBy?.UserId?? Guid.Empty;
            articleDAL.User.UserName = article.CreatedBy?.UserName;
            articleDAL.User.Email = article.CreatedBy?.Email;


            articleDAL.ArticleTypeId_ArticleTypeId = article.ArticleTypeId.ArticleTypeId;
            
            articleDAL.IP = article.IP;

            foreach (var tag in article.Tags)
            {
                var articleTag = new DAL.ArticleTag();
                articleTag.Article = articleDAL;
                articleTag.Tag = db.Tags.FirstOrDefault(k => k.Id == tag.Id);
                db.ArticleTags.InsertOnSubmit(articleTag);
            }

            articleDAL.Body =  articleDAL.Body ;
            return articleDAL;
        }

        public static List<ArticleDTO> ToDTO(this List<Models.Article> articles, bool WithBody = false)
        {
            var articlesDTO = new List<ArticleDTO>();
            foreach (var article in articles)
            {
                articlesDTO.Add(article.ToDTO(WithBody));
            }
            return articlesDTO;

        }

        public static ArticleDTO ToDTO(this Models.Article article, bool WithBody = false)
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
            articleDTO.CreatedByUserName = article.CreatedBy?.UserName;
            articleDTO.ArticleType = article.ArticleTypeId.ArticleTypeId;
            articleDTO.IP = article.IP;

            articleDTO.Tags = new List<Models.Tag>();
            foreach (var k in article.ArticleTags)
            {
                articleDTO.Tags.Add(k.Tag);
            }

            articleDTO.Body = WithBody ? articleDTO.Body : string.Empty;
            return articleDTO;
        }


        public static List<ArticleDTO> ToDTO(this List<DAL.Article> articles, bool WithBody = false)
        {
            var articlesDTO = new List<ArticleDTO>();
            foreach (var article in articles)
            {
                articlesDTO.Add(article.ToDTO(WithBody));
            }
            return articlesDTO;

        }

        public static ArticleDTO ToDTO(this DAL.Article article, bool WithBody = false)
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


            articleDTO.CreatedById = article.User?.UserId;
            articleDTO.CreatedByUserName = article.User?.UserName;
            articleDTO.ArticleType = article.ArticleType.ArticleTypeId;
            articleDTO.IP = article.IP;

            articleDTO.Tags = new List<Models.Tag>();
            foreach (var k in article.ArticleTags)
            {
                var tag = new Models.Tag() { Id= k.Tag?.Id??0, Name = k.Tag?.Name,
                Description = k.Tag?.Description};
                articleDTO.Tags.Add(tag);
            }

            articleDTO.Body = WithBody ? articleDTO.Body : string.Empty;
            return articleDTO;
        }
    }
}