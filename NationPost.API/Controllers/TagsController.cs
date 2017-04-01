using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using NationPost.API.Models;
using NationPost.DAL;

namespace NationPost.API.Controllers
{
    [Route("api/Tags/{id?}", Name = "api_Tags")]
    public class TagsController : ApiController
    {
        private BlogDBContextLinqDataContext db = new BlogDBContextLinqDataContext();

        // GET api/Tags
        public IQueryable<Models.Tag> GetTags(string search = "")
        {

            var query =   string.IsNullOrWhiteSpace(search)? db.Tags.Take(10):db.Tags.Where(k=> k.Name.StartsWith(search));
            return query.Select(k => new Models.Tag
            {
                Description = k.Description,
                Id = k.Id,
                Name = k.Name
            });

        }

        // GET api/Tags/5
        [ResponseType(typeof(Models.Tag))]
        public IHttpActionResult GetTag(int id)
        {
            DAL.Tag tag = db.Tags.FirstOrDefault(k=> k.Id == id);
            if (tag == null)
            {
                return NotFound();
            }

            return Ok(tag);
        }

        // PUT api/Tags/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTag(int id, Models.Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tag.Id)
            {
                return BadRequest();
            }

            var tagDAL = db.Tags.FirstOrDefault(k => k.Id == id);
            tagDAL.Name = tag.Name;
            tagDAL.Description = tag.Description;

            try
            {
                db.SubmitChanges();
            }
            catch (Exception)
            {
                if (!TagExists(id))
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

        // POST api/Tags
        [ResponseType(typeof(Models.Tag))]
        public IHttpActionResult PostTag(Models.Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DAL.Tag tagDAL = new DAL.Tag();
            tagDAL.Id = tag.Id;
            tagDAL.Name = tag.Name;
            tagDAL.Description = tag.Description;


            db.Tags.InsertOnSubmit(tagDAL);
            db.SubmitChanges();

            return CreatedAtRoute("api_Tags", new { id = tag.Id }, tag);
        }

        // DELETE api/Tags/5
        [ResponseType(typeof(Models.Tag))]
        public IHttpActionResult DeleteTag(int id)
        {
            var tag = db.Tags.FirstOrDefault ( k=> k.Id == id);
            if (tag == null)
            {
                return NotFound();
            }

            db.Tags.DeleteOnSubmit(tag);
            db.SubmitChanges();

            return Ok(tag);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TagExists(int id)
        {
            return db.Tags.Count(e => e.Id == id) > 0;
        }
    }
}