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

namespace NationPost.API.Controllers
{
    public class ArticlesController : ApiController
    {
        private APIContext db = new APIContext();

        //[Route("api/articles/paged/{pageNumber=pageNumber}/{pageSize=pageSize}/{userId=userId}/{sortOrder=sortOrder}/{searchString=searchString}")]
        public IEnumerable<Article> GetArticles( int pageNumber, int pageSize,  Guid? userId = null, string sortOrder = "", string searchString = "")
        {

            if (pageNumber <= 0)
            {
                throw new Exception("pagenumber minimum value is 1");
            }

            if (pageSize <= 0)
            {
                throw new Exception("pageSize minimum value is 1");
            }

            sortOrder = String.IsNullOrEmpty(sortOrder) ? "createdon_desc" : "";


            var articles = from s in db.Articles
                           select s;
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

            switch (sortOrder)
            {
                case "createdon_desc":
                    articles = articles.OrderByDescending(s => s.CreatedOn);
                    break;

                default:  // Name ascending 
                    articles = articles.OrderBy(s => s.CreatedOn);
                    break;
            }

            return articles.ToPagedList(pageNumber, pageSize);
        }

        // GET api/Articles
        public IEnumerable<Article> GetArticles()
        {
            return db.Articles;
        }

        // GET api/Articles/5
        [ResponseType(typeof(Article))]
        public IHttpActionResult GetArticle(Guid id)
        {
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
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
        [ResponseType(typeof(Article))]
        public IHttpActionResult PostArticle(Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            article.ArticleId = Guid.NewGuid();
            article.CreatedOn = DateTime.Now;
            var user = db.Users.FirstOrDefault(j => j.UserId == article.CreatedBy.UserId);
            if (user != null)
            {
                article.CreatedBy = user;

            }
            else
            {
                throw new Exception("User info not found");
            }

            var articleType = db.ArticleTypes.FirstOrDefault(j => j.ArticleTypeId == article.ArticleTypeId.ArticleTypeId);
            if (articleType != null)
            {
                article.ArticleTypeId = articleType;

            }
            else
            {
                throw new Exception("Articletype info not found");
            }


            db.Articles.Add(article);

            try
            {
                db.SaveChanges();
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
            //TODO later: fix self referencing loop
            article.CreatedBy.Articles = null;
            article.ArticleTypeId.Articles = null;
            return CreatedAtRoute("Default", new { id = article.ArticleId }, article);
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