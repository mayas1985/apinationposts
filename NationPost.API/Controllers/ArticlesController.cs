using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using NationPost.API.Models;
using PagedList;
using NationPost.API.Helper;
using NationPost.DAL;

namespace NationPost.API.Controllers
{
    public class ArticlesController : ApiController
    {
        private BlogDBContextLinqDataContext db = new BlogDBContextLinqDataContext();

        //[Route("api/articles/paged/{pageNumber=pageNumber}/{pageSize=pageSize}/{userId=userId}/{sortOrder=sortOrder}/{searchString=searchString}")]

        public IEnumerable<ArticleDTO> GetArticlesWithLocation(int pageNumber, int pageSize, int? articleTypeId, Guid? userId = null, string sortOrder = "", string searchString = "",
         string Longtitude = "",
        string Latitude = "",
        string Country = "",
        string administrative_area_level_1 = "",
        string administrative_area_level_2 = "",
        string locality = "",
        string sublocality_level_1 = "",
        string sublocality_level_2 = "",
        string sublocality_level_3 = "")
        {
            if (pageNumber <= 0)
            {
                throw new Exception("pagenumber minimum value is 1");
            }

            if (pageSize <= 0)
            {
                throw new Exception("pageSize minimum value is 1");
            }

            sortOrder = String.IsNullOrEmpty(sortOrder) ? "createdon_desc" : sortOrder;


            var articles = db.Articles.Where(k => k.IsValid && k.IsVisible);
            //where s.ArticleTypeId.ArticleTypeId == articleTypeId
            if (articleTypeId != null)
                articles = articles.Where(k => k.ArticleTypeId_ArticleTypeId == articleTypeId);

            if (!String.IsNullOrEmpty(searchString))
            {
                articles = articles.Where(s => s.Title.Contains(searchString)
                                       || s.Body.Contains(searchString)
                                       || s.Description.Contains(searchString)
                                       || s.Summary.Contains(searchString)
                                       );
            }

            if (userId != null)
            {
                articles = articles.Where(k => k.CreatedBy_UserId == userId);
            }

            if (!string.IsNullOrWhiteSpace(Longtitude)) articles = articles.Where(k => k.Longtitude == Longtitude);
            if (!string.IsNullOrWhiteSpace(Latitude)) articles = articles.Where(k => k.Latitude == Latitude);
            if (!string.IsNullOrWhiteSpace(Country)) articles = articles.Where(k => k.Country == Country);
            if (!string.IsNullOrWhiteSpace(administrative_area_level_1)) articles = articles.Where(k => k.administrative_area_level_1 == administrative_area_level_1);
            if (!string.IsNullOrWhiteSpace(administrative_area_level_2)) articles = articles.Where(k => k.administrative_area_level_2 == administrative_area_level_2);
            if (!string.IsNullOrWhiteSpace(locality)) articles = articles.Where(k => k.locality == locality);
            if (!string.IsNullOrWhiteSpace(sublocality_level_1)) articles = articles.Where(k => k.sublocality_level_1 == sublocality_level_1);
            if (!string.IsNullOrWhiteSpace(sublocality_level_2)) articles = articles.Where(k => k.sublocality_level_2 == sublocality_level_2);
            if (!string.IsNullOrWhiteSpace(sublocality_level_3)) articles = articles.Where(k => k.sublocality_level_3 == sublocality_level_3);



            var monthback = DateTime.Now.AddMonths(-1);

            switch (sortOrder)
            {
                case "createdon_desc":
                    articles = articles.OrderByDescending(s => s.CreatedOn);
                    break;
                case "rating_desc":
                    articles = articles.Where(k => k.CreatedOn > monthback && k.Rating > 5).OrderByDescending(s => s.Rating);
                    break;
                case "recommended_desc":
                    articles = articles.OrderByDescending(s => s.TotalRating);
                    break;
                case "trend_desc":
                    articles = articles.Where(k => k.TotalRating > 5).OrderByDescending(s => s.CreatedOn);
                    break;
                case "like_desc":
                    articles = articles.Where(k => k.CreatedOn > monthback).OrderByDescending(s => s.Like);
                    break;

                case "dislike_desc":
                    articles = articles.Where(k => k.CreatedOn > monthback).OrderByDescending(s => s.Dislike);
                    break;

                default:  // Name ascending 
                    articles = articles.OrderBy(s => s.CreatedOn);
                    break;
            }

            var lst = articles/*Include(x => x.User).Include(m => m.ArticleType).Include(j => j.ArticleTags.Select(m => m.Tag))*/.ToPagedList(pageNumber, pageSize).ToList();
            return lst.ToDTO();
        }

        // GET api/Articles
        public IEnumerable<Models.Article> GetArticles()
        {
            throw new Exception("This would cause all articles to loaded so this api has been stopped");
            //return db.Articles;
        }


        public IEnumerable<ArticleDTO> GetArticlesByTag(int tagId)
        {
            return db.Articles.Where(k => k.ArticleTags.Any(m => m.Tag.Id == tagId) && k.IsValid && k.IsVisible)
                //.Include(j => j.ArticleType)
                //.Include(m => m.User)
                //.Include(j => j.ArticleTags.Select(m => m.Tag))
                .ToList().ToDTO();
        }



        // GET api/Articles/5
        [ResponseType(typeof(ArticleDTO))]
        public IHttpActionResult GetArticle(Guid id)
        {
            var article = db.Articles.Where(k => k.ArticleId == id)
                //.Include(j => j.ArticleType)
                //.Include(m => m.User)
                //.Include(j => j.ArticleTags.Select(m => m.Tag))
                .FirstOrDefault();
            if (article == null)
            {
                return NotFound();
            }

            return Ok(article.ToDTO(true));
        }

        // PUT api/Articles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutArticle(Guid id, Models.Article article)
        {
            throw new Exception("Not implemented");

        }



        // POST api/Articles
        [ResponseType(typeof(ArticleDTO))]
        public IHttpActionResult PostArticle(Models.Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var articleDAL = article.ToDAL(db);
            articleDAL.ArticleId = Guid.NewGuid();
            articleDAL.CreatedOn = DateTime.UtcNow;
            var mailNeedsToBeSent = false;
            DAL.User newuser = null;

            if (articleDAL.User == null)
            {
                newuser = new DAL.User();
                newuser.CreatedOn = DateTime.Now;
                newuser.UserId = Guid.NewGuid();
                newuser.UserName = article.CreatedBy.UserName;
                newuser.Email = article.CreatedBy.Email;
                newuser.Password = "Password" + new Random().Next(10000, 99999).ToString();

                db.Users.InsertOnSubmit(newuser);
                articleDAL.User = newuser;
                mailNeedsToBeSent = true;
                //db.SaveChanges();

                //throw new Exception("User info not found");



            }
            if (articleDAL.ArticleType == null)
            {
                throw new Exception("Articletype info not found");
            }

            // IN  extension TODAL
            ////map  tags to articleTags
            //foreach (var tag in articleDAL.Tags)
            //{
            //    var articleTag = new DAL.ArticleTags();
            //    articleTag.article = articleDAL;
            //    articleTag.Tag = db.Tags.FirstOrDefault(k => k.Id == tag.Id);
            //    db.ArticleTags.Add(articleTag);
            //}

            db.Articles.InsertOnSubmit(articleDAL);

            try
            {
                db.SubmitChanges();
                if (mailNeedsToBeSent)
                {
                    MailHelper.Send("Your Username is " + newuser.Email + " and password is " + newuser.Password, "NationPost - Password retrieval", "admin@nationpost.com", newuser.Email);
                }
            }
            catch (Exception dex)
            {
                throw;
            }
            ////TODO later: fix self referencing loop
            //article.CreatedBy.Articles = null;
            //article.ArticleTypeId.Articles = null;
            //foreach (var k in article.ArticleTags)
            //{
            //    k.article = null;
            //}

            return CreatedAtRoute("Default", new { id = articleDAL.ArticleId }, articleDAL.ToDTO());
        }

        //// DELETE api/Articles/5
        //[ResponseType(typeof(Article))]
        //public IHttpActionResult DeleteArticle(Guid id)
        //{
        //    Article article = db.Articles.Find(id);
        //    if (article == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Articles.Remove(article);
        //    db.SaveChanges();

        //    return Ok(article);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ArticleExists(Guid id)
        {
            return db.Articles.Count(e => e.ArticleId == id) > 0;
        }
    }
}