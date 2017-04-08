using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using NationPost.API.Models;
using NationPost.API.Helper;
using NationPost.DAL;

namespace NationPost.API.Controllers
{
    public class RatingsController : ApiController
    {
        private BlogDBContextLinqDataContext db = new BlogDBContextLinqDataContext();

        // POST api/Ratings
        public ResponseDTO PostArticleRatings(Models.ArticleRatings articleRatingsModel)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseDTO() { IsSuccess = false, Message = "Model not valid, check parameters" };
            }

            DAL.Article article = db.Articles.Where(k => k.ArticleId == articleRatingsModel.ArticleId)
                //.Include(j => j.ArticleType)
                //.Include(m => m.User)
                .FirstOrDefault();

            if (article == null)
            {
                return new ResponseDTO() { IsSuccess = false, Message = "article id incorrect" };
            }

            if (articleRatingsModel.ratingType == RatingType.RatingGiven && 
                        db.ArticleRatings.Any(k => k.ratingType == (int)RatingType.RatingGiven 
                        && k.ArticleId == articleRatingsModel.ArticleId && k.IPAdditionalInfo == articleRatingsModel.IPAdditionalInfo && 
                      (articleRatingsModel.UserId == null || k.UserId == articleRatingsModel.UserId )
                      ))
            {
                return new ResponseDTO() { IsSuccess = true, Message = "Already Rated" };
            }

            db.ArticleRatings.InsertOnSubmit(articleRatingsModel.ToDTO(db));

            if (articleRatingsModel.ratingType == RatingType.RatingGiven)
            {
                if (articleRatingsModel.Rating > 5)
                    return new ResponseDTO() { IsSuccess = false, Message = "Rating cannot be greater than 5" };

                article.Rating = article.Rating + articleRatingsModel.Rating;
                article.TotalRating = article.TotalRating + 1;
            }

            if (articleRatingsModel.ratingType == RatingType.Liked || articleRatingsModel.ratingType == RatingType.Disliked)

            {
                var articleRating = db.ArticleRatings.FirstOrDefault(k => (k.ratingType == (int)RatingType.Liked || k.ratingType == (int)RatingType.Disliked)
                && k.ArticleId == articleRatingsModel.ArticleId && k.IPAdditionalInfo == articleRatingsModel.IPAdditionalInfo && (articleRatingsModel.UserId == null || k.UserId == articleRatingsModel.UserId));
                if (articleRating != null)
                {

                    return new ResponseDTO() { IsSuccess = true, Message = "Already " + (articleRating.ratingType == (int)RatingType.Liked ? "Liked" : "Disliked") };
                }
            }


            if (articleRatingsModel.ratingType == RatingType.Liked)
            {
                article.Like = article.Like + 1;
            }

            if (articleRatingsModel.ratingType == RatingType.Disliked)
            {
                article.Dislike = article.Dislike + 1;
            }

            try
            {
                db.SubmitChanges();
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