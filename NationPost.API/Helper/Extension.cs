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


        public static Models.User ToDTO(this DAL.User userDAL)
        {
            if (userDAL == null) return null;

            var user = new Models.User();
            user.AboutMe = userDAL.AboutMe;
            user.Contact = userDAL.Contact;
            user.CreatedOn = userDAL.CreatedOn;
            user.Email = userDAL.Email;
            user.FacebookLink = userDAL.FacebookLink;
            user.FirstName = userDAL.FirstName;
            user.GoogleLink = userDAL.GoogleLink;
            user.IsAboutMeVisible = userDAL.IsAboutMeVisible;
            user.IsActive = userDAL.IsActive;
            user.IsContactVisible = userDAL.IsContactVisible;
            user.IsFacebookLinkVisible = userDAL.IsFacebookLinkVisible;
            user.IsGoogleLinkVisible = userDAL.IsGoogleLinkVisible;
            user.IsTwitterLinkVisible = userDAL.IsTwitterLinkVisible;
            user.LastName = userDAL.LastName;
            user.Password = userDAL.Password;
            user.Token = userDAL.Token;
            user.TwitterLink = userDAL.TwitterLink;
            user.UserId = userDAL.UserId;
            user.UserName = userDAL.UserName;
            return user;

        }

        public static DAL.User ToDAL(this Models.User userModel, BlogDBContextLinqDataContext db)
        {
            DAL.User user = new DAL.User();
            if (userModel.UserId != Guid.Empty)
            {
                user = db.Users.FirstOrDefault(k => k.UserId == userModel.UserId);
                if (user == null)
                    user = new DAL.User();
            }
            user.AboutMe = userModel.AboutMe;
            user.Contact = userModel.Contact;
            user.CreatedOn = userModel.CreatedOn;
            user.Email = userModel.Email;
            user.FacebookLink = userModel.FacebookLink;
            user.FirstName = userModel.FirstName;
            user.GoogleLink = userModel.GoogleLink;
            user.IsAboutMeVisible = userModel.IsAboutMeVisible;
            user.IsActive = userModel.IsActive;
            user.IsContactVisible = userModel.IsContactVisible;
            user.IsFacebookLinkVisible = userModel.IsFacebookLinkVisible;
            user.IsGoogleLinkVisible = userModel.IsGoogleLinkVisible;
            user.IsTwitterLinkVisible = userModel.IsTwitterLinkVisible;
            user.LastName = userModel.LastName;
            user.Password = userModel.Password;
            user.Token = userModel.Token;
            user.TwitterLink = userModel.TwitterLink;
            user.UserId = userModel.UserId;
            user.UserName = userModel.UserName;
            return user;

        }

        public static DAL.Article ToDAL(this Models.Article article, BlogDBContextLinqDataContext db)
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


            if (article.CreatedBy != null)
            {
                articleDAL.User = db.Users.FirstOrDefault(k => (k.UserId == article.CreatedBy.UserId && article.CreatedBy.UserId != Guid.Empty)
               || k.UserName == article.CreatedBy.UserName
               || k.Email == article.CreatedBy.Email);
            }

            if (article.ArticleTypeId != null)
            {
                articleDAL.ArticleType = db.ArticleTypes.FirstOrDefault(k => k.ArticleTypeId == article.ArticleTypeId.ArticleTypeId);
            }

            articleDAL.IP = article.IP;

            if (article.Tags != null)
            {
                foreach (var tag in article.Tags)
                {
                    var articleTag = new DAL.ArticleTag();
                    articleTag.Article = articleDAL;
                    articleTag.Tag = db.Tags.FirstOrDefault(k => k.Id == tag.Id);
                    db.ArticleTags.InsertOnSubmit(articleTag);
                }

            }
            articleDAL.Body = articleDAL.Body;
            return articleDAL;
        }

        public static DAL.ArticleRating ToDTO(this Models.ArticleRatings modelArticleRatings, BlogDBContextLinqDataContext db)
        {
            var articleRating = new DAL.ArticleRating();
            if (modelArticleRatings.ArticleRatingId > 0)
            {
                articleRating = db.ArticleRatings.FirstOrDefault(k => k.ArticleRatingId == modelArticleRatings.ArticleRatingId);
                if (articleRating == null)
                {
                    articleRating = new DAL.ArticleRating();
                }
            }

            articleRating.ArticleRatingId = modelArticleRatings.ArticleRatingId;
            articleRating.ArticleId = modelArticleRatings.ArticleId;
            articleRating.UserId = modelArticleRatings.UserId;
            articleRating.Rating = modelArticleRatings.Rating;
            articleRating.ratingType = (int)modelArticleRatings.ratingType;
            articleRating.IPAdditionalInfo = modelArticleRatings.IPAdditionalInfo;

            return articleRating;

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
                var tag = new Models.Tag()
                {
                    Id = k.Tag?.Id ?? 0,
                    Name = k.Tag?.Name,
                    Description = k.Tag?.Description
                };
                articleDTO.Tags.Add(tag);
            }

            articleDTO.Body = WithBody ? articleDTO.Body : string.Empty;
            return articleDTO;
        }
    }
}