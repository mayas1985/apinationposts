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
using NationPost.API.Helper;

namespace NationPost.API.Controllers
{
    public class RatingsController : ApiController
    {
        private APIContext db = new APIContext();

        // POST api/Ratings
        public ResponseDTO PostArticleRatings(ArticleRatings articleRatings)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseDTO() { IsSuccess = false, Message = "Model not valid, check parameters" };
            }

            Article article = db.Articles.Where(k => k.ArticleId == articleRatings.ArticleId).Include(j => j.ArticleTypeId)
                .Include(m => m.CreatedBy)
                .FirstOrDefault();

            if (article == null)
            {
                return new ResponseDTO() { IsSuccess = false, Message = "article id incorrect" };
            }

            if (articleRatings.ratingType == RatingType.RatingGiven && db.ArticleRatings.Any(k => k.ArticleId == articleRatings.ArticleId && k.IPAdditionalInfo == articleRatings.IPAdditionalInfo && k.UserId == articleRatings.UserId))
            {
                return new ResponseDTO() { IsSuccess = true, Message = "Already Rated" };
            }

            db.ArticleRatings.Add(articleRatings);

            if (articleRatings.ratingType == RatingType.RatingGiven)
            {
                if (articleRatings.Rating > 5)
                    return new ResponseDTO() { IsSuccess = false, Message = "Rating cannot be greater than 5" };

                article.Rating = article.Rating + articleRatings.Rating;
                article.TotalRating = article.TotalRating + 1;
            }

            if (articleRatings.ratingType == RatingType.Liked || articleRatings.ratingType == RatingType.Disliked)

            {
                var articleRating = db.ArticleRatings.FirstOrDefault(k => (k.ratingType == RatingType.Liked || k.ratingType == RatingType.Disliked)
                && k.ArticleId == articleRatings.ArticleId && k.IPAdditionalInfo == articleRatings.IPAdditionalInfo && k.UserId == articleRatings.UserId);
                if (articleRating != null)
                {

                    return new ResponseDTO() { IsSuccess = true, Message = "Already " + (articleRating.ratingType == RatingType.Liked ? "Liked" : "Disliked") };
                }
            }


            if (articleRatings.ratingType == RatingType.Liked)
            {
                article.Like = article.Like + 1;
            }

            if (articleRatings.ratingType == RatingType.Disliked)
            {
                article.Like = article.Dislike + 1;
            }

            db.Articles.Attach(article);
            db.Entry(article).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            return new ResponseDTO() { IsSuccess = true, Message = "Ratings Updated " };

        }



        //[Route("~api/ratings/")]

        // GET api/Ratings
        //public IQueryable<ArticleRatings> GetArticleRatings()
        //{
        //    return db.ArticleRatings;
        //}

        //// GET api/Ratings/5
        //[ResponseType(typeof(ArticleRatings))]
        //public IHttpActionResult GetArticleRatings(int id)
        //{
        //    ArticleRatings articleRatings = db.ArticleRatings.Find(id);
        //    if (articleRatings == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(articleRatings);
        //}

        //// PUT api/Ratings/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutArticleRatings(int id, ArticleRatings articleRatings)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != articleRatings.ArticleRatingId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(articleRatings).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ArticleRatingsExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST api/Ratings
        //[ResponseType(typeof(ArticleRatings))]
        //public IHttpActionResult PostArticleRatings(ArticleRatings articleRatings)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.ArticleRatings.Add(articleRatings);
        //    db.SaveChanges();

        //    return CreatedAtRoute("api_Ratings", new { id = articleRatings.ArticleRatingId }, articleRatings);
        //}

        //// DELETE api/Ratings/5
        //[ResponseType(typeof(ArticleRatings))]
        //public IHttpActionResult DeleteArticleRatings(int id)
        //{
        //    ArticleRatings articleRatings = db.ArticleRatings.Find(id);
        //    if (articleRatings == null)
        //    {
        //        return NotFound();
        //    }

        //    db.ArticleRatings.Remove(articleRatings);
        //    db.SaveChanges();

        //    return Ok(articleRatings);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ArticleRatingsExists(int id)
        {
            return db.ArticleRatings.Count(e => e.ArticleRatingId == id) > 0;
        }
    }
}