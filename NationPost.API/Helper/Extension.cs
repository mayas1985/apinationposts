using NationPost.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NationPost.API.Helper
{
    public static class Extension
    {
        public static List<ArticleDTO> ToDTO(this List<Article> articles)
        {
            var articlesDTO = new List<ArticleDTO>();
            foreach (var article in articles)
            {
                articlesDTO.Add(article.ToDTO());
            }
            return articlesDTO;

        }

        public static ArticleDTO ToDTO(this Article article)
        {
            ArticleDTO articleDTO = new ArticleDTO();
            articleDTO.ArticleId = article.ArticleId;
            articleDTO.CreatedOn = article.CreatedOn;
            articleDTO.Title = article.Title;
            articleDTO.Description = article.Description;
            articleDTO.Summary = article.Summary;
            articleDTO.Body = article.Body;
            articleDTO.IsActive = article.IsActive;
            articleDTO.Rating = article.Rating;
            articleDTO.Like = article.Like;
            articleDTO.Dislike = article.Dislike;

            articleDTO.CreatedById = article.CreatedBy?.UserId;
            articleDTO.ArticleType = article.ArticleTypeId.ArticleTypeId;

            articleDTO.Tags = new List<Tag>();
            foreach (var k in article.ArticleTags)
            {
                articleDTO.Tags.Add(k.Tag);
            }

            return articleDTO;
        }
    }
}