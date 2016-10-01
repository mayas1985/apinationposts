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

namespace NationPost.API.Controllers
{
    [Route("api/Articles/{id?}", Name = "api_Articles")]
    public class ArticlesController : ApiController
    {
        private APIContext db = new APIContext();

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

            return CreatedAtRoute("api_Articles", new { id = article.ArticleId }, article);
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