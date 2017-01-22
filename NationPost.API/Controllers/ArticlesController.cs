using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using NationPost.API.Context;
using NationPost.API.Models;
using PagedList;
using NationPost.API.Helper;
using System.Data.Entity.Validation;

namespace NationPost.API.Controllers
{
    public class ArticlesController : ApiController
    {
        private APIContext db = new APIContext();

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


            var articles = from s in db.Articles
                               //where s.ArticleTypeId.ArticleTypeId == articleTypeId
                           select s;
            if (articleTypeId != null)
                articles = articles.Where(k => k.ArticleTypeId.ArticleTypeId == articleTypeId);

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
                articles = articles.Where(k => k.CreatedBy.UserId == userId);
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
                    articles = articles.OrderByDescending(s => s.Rating);
                    break;
                case "trend_desc":
                    articles = articles.Where(k => k.CreatedOn > monthback && k.Rating > 5).OrderByDescending(s => s.Rating);
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

            var lst = articles.Include(m => m.ArticleTypeId).Include(j => j.ArticleTags.Select(m => m.Tag)).ToPagedList(pageNumber, pageSize).ToList();
            return lst.ToDTO();
        }

        // GET api/Articles
        public IEnumerable<Article> GetArticles()
        {
            throw new Exception("This would cause all articles to loaded so this api has been stopped");
            //return db.Articles;
        }


        public IEnumerable<ArticleDTO> GetArticlesByTag(int tagId)
        {
            return db.Articles.Where(k => k.ArticleTags.Any(m => m.Tag.TagId == tagId))
                .Include(j => j.ArticleTypeId)
                .Include(m => m.CreatedBy)
                .Include(j => j.ArticleTags.Select(m => m.Tag))
                .ToList().ToDTO();
        }



        // GET api/Articles/5
        [ResponseType(typeof(ArticleDTO))]
        public IHttpActionResult GetArticle(Guid id)
        {
            Article article = db.Articles.Where(k => k.ArticleId == id)
                .Include(j => j.ArticleTypeId)
                .Include(m => m.CreatedBy)
                .Include(j => j.ArticleTags.Select(m => m.Tag)).FirstOrDefault();
            if (article == null)
            {
                return NotFound();
            }

            return Ok(article.ToDTO(true));
        }

        // PUT api/Articles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutArticle(Guid id, Article article)
        {
            throw new Exception("Not implemented");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != article.ArticleId)
            {
                return BadRequest();
            }

            db.Entry(article).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }



        // POST api/Articles
        [ResponseType(typeof(ArticleDTO))]
        public IHttpActionResult PostArticle(Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            article.ArticleId = Guid.NewGuid();
            article.CreatedOn = DateTime.Now;
            var mailNeedsToBeSent = false;
            User newuser = null;

            var user = db.Users.FirstOrDefault(j => j.UserId == article.CreatedBy.UserId || j.Email == article.CreatedBy.Email);
            if (user != null)
            {
                article.CreatedBy = user;

            }
            else
            {
                newuser = new User();
                newuser.CreatedOn = DateTime.Now;
                newuser.UserId = Guid.NewGuid();
                //newuser.UserName = article.CreatedBy.Email;
                newuser.Email = article.CreatedBy.Email;
                newuser.Password = "Password" + new Random().Next(10000, 99999).ToString();

                db.Users.Add(newuser);
                article.CreatedBy = newuser;
                mailNeedsToBeSent = true;
                //db.SaveChanges();

                //throw new Exception("User info not found");



            }
            if (article.ArticleTypeId != null)
            {
                var articleType = db.ArticleTypes.FirstOrDefault(j => j.ArticleTypeId == article.ArticleTypeId.ArticleTypeId);
                if (articleType != null)
                {
                    article.ArticleTypeId = articleType;

                }
                else
                {
                    throw new Exception("Articletype info not found");
                }
            }
            else
            {
                throw new Exception("Articletype info not found");
            }


            //map  tags to articleTags
            foreach (var tag in article.Tags)
            {
                var articleTag = new ArticleTags();
                articleTag.article = article;
                articleTag.Tag = db.Tags.FirstOrDefault(k => k.TagId == tag.TagId);
                db.ArticleTags.Add(articleTag);
            }

            db.Articles.Add(article);

            try
            {
                db.SaveChanges();
                if (mailNeedsToBeSent)
                {
                    MailHelper.Send("Your Username is " + newuser.Email + " and password is " + newuser.Password, "NationPost - Password retrieval", "admin@nationpost.com", newuser.Email);
                }
            }
            catch (DbEntityValidationException dex)
            {
                throw;
            }
            catch (DbUpdateException)
            {
                if (ArticleExists(article.ArticleId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            ////TODO later: fix self referencing loop
            //article.CreatedBy.Articles = null;
            //article.ArticleTypeId.Articles = null;
            //foreach (var k in article.ArticleTags)
            //{
            //    k.article = null;
            //}

            return CreatedAtRoute("Default", new { id = article.ArticleId }, article.ToDTO());
        }

        // DELETE api/Articles/5
        [ResponseType(typeof(Article))]
        public IHttpActionResult DeleteArticle(Guid id)
        {
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return NotFound();
            }

            db.Articles.Remove(article);
            db.SaveChanges();

            return Ok(article);
        }

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